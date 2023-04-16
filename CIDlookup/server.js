/*             *CID lookup server* ( url/?phone=xxxxx )
 * ┌─────────────────┐                ┌──────────────┐
 * │                 │                │  CID SERVER  │
 * │                 │  <- CID LOOKUP │    CRM       │
 * │ PBX - ASTERISK  │ ─────────────► │ WINLIFT      │
 * │                 │                │       OTHER  │
 * │                 │                │  11888       │
 * └┬────────────────┘           ┌────┴──────────────┘
 *  │ INF                        │INF
 *  │           TTS - STT A.I.'s │
 *  └─►┌────────────┐            ▼
 *     │            │         ┌──────────────┐
 *     │      PHONE │         │              │
 *     └────────────┘         │              │
 *                 ▲  CMD CALL│ TERMINAL PC  │ 
 *                 └──────────┤              │
 *                            │              │
 *                            └──────────────┘
 */
var config = require('./config.json');


//-----------------------------------------------------------
//phonebookFile = "./phonebook.json";

var express = require('express');
var app = express();
var net = require('net');
var server = net.createServer();
const Fs = require('fs/promises')
const fs = require('fs');
const sql = require('mssql/msnodesqlv8')
const https = require('https');
const { Console } = require('console');
const connected_ips  = []; 
const pool = new sql.ConnectionPool({
    database: config.DATABASE,
    server: config.SERVER_NAME,
    driver: 'msnodesqlv8',
    options: {
        trustedConnection: true
    }
})

checkForFile("./cache.json");

checkForFile(config.PHONEBOOK);

function find_CustomerID(s) {

    let ContactID = null;
    return new Promise((resolve, reject) => {
        //NO JOIN or UNION*
        pool.request().query("select Contact_ID  FROM PHONES  WHERE REPLACE(trim(PhoneNumber), ' ', '')   LIKE " + s, (err, result) => {

            if (typeof result !== "undefined" && result.recordset.length > 0) {
                resolve(result.recordset[0]);
            } else {

                if (ContactID == null) {

                    pool.request().query("select Contact_ID  FROM ContactsPhones WHERE  REPLACE(trim(Mobile), ' ', '')   LIKE " + s, (err, result) => {

                        if (typeof result !== "undefined" && result.recordset.length > 0) {
                            resolve(result.recordset[0]);
                        } else {
                            pool.request().query("select Contact_ID FROM ContactsPhones2 WHERE  REPLACE(trim(Tilefona), ' ', '')    LIKE '%s" + s + "%'", (err, result) => {
                                if (typeof result !== "undefined" && result.recordset.length > 0) {
                                    resolve(result.recordset[0]);
                                } else {

                                    let newstr = s.substr(0, 4) + '%' + s.substr(4, 10);
                                    newstr = newstr.substr(0, 3) + '%' + newstr.substr(3, 10);

                                    pool.request().query("select Contact_ID FROM CONTACTS WHERE  Notes  LIKE '%" + newstr + "%' OR Notes like '%" + s + "%'", (err, result) => {
                                        if (typeof result !== "undefined") {
                                            resolve(result.recordset[0]);
                                        }
                                    })
                                }

                            })

                        }
                    })
                }
            }
        })
    });

}
async function insert_new_task(contactid) {





}

async function contact_id_to_branch_id(contactid) {
   
    return new Promise(async (resolve, reject) => {
    

        pool.request().query('select Branch_ID from BRANCHES WHERE Contact_ID=' + contactid, (err, result) => {
            if (result.recordset[0]) {
                resolve(result.recordset[0].Branch_ID);
            }
            resolve(contactid);
             
        })
    })



}
async function Anagnorisi(s) {
    return new Promise(async (resolve, reject) => {
        let custid = await find_CustomerID(s);
        let CustOb = {
            address: '',
            name: '',
            notes: '',
            id: '',
        }
        if (typeof custid === "undefined") {
            resolve(false);
            return false;
        }

        pool.request().query('select Contact_ID, Area_ID, Address, Notes, SureName from CONTACTS WHERE Contact_ID=' + custid.Contact_ID, (err, result) => {
            CustOb.address = result.recordset[0].Address;
            CustOb.address = CustOb.address.replace(/\s+/g, ' ').trim();
            CustOb.address = capitalizeFirstLetter(CustOb.address.trim().toLowerCase())

            CustOb.name = capitalizeFirstLetter(result.recordset[0].SureName.trim().toLowerCase());
            CustOb.notes = result.recordset[0].Notes;
            CustOb.id = custid.Contact_ID;

            if (result.recordset[0].Area_ID == null) {
                resolve(CustOb);
                return;
            }
            pool.request().query('select AreaName from AREAS WHERE Area_ID=' + result.recordset[0].Area_ID, (err, result) => {
                CustOb.address = capitalizeFirstLetter(CustOb.address.trim().toLowerCase()) + ' ' + capitalizeFirstLetter(result.recordset[0].AreaName.trim().toLowerCase());
                CustOb.address = CustOb.address.replace(/\s+/g, ' ').trim()

                resolve(CustOb);
            })
        })
    })
}


pool.connect().then(async () => {


    console.log("Connected to db. starting server...")
    let port = config.SOCKET_PORT;
    let host = '0.0.0.0';
    var sockets = [];
    server.listen(port, host, () => {
        console.log(`TCP server listening on ${host}:${port}`);
    });


    server.on('connection', (socket) => {
        var clientAddress = `${socket.remoteAddress}:${socket.remotePort}`;
        console.log(`New Windows App client connected: ${clientAddress}`);
        
        //1 cli per ip
        if (connected_ips.indexOf(socket.remoteAddress) > -1) {
            socket.destroy();
        } else {
            connected_ips.push(socket.remoteAddress);
        }
    
 
        socket.write('AUTH:0\r\n');
 
        socket.write('PONG:' + Math.round((new Date()).getTime() / 1000)+ '\r\n');
 
        socket.on('close', (data) => {

            console.log(`connection closed: ${clientAddress}`);
            connected_ips.splice( connected_ips.indexOf(socket.remoteAddress),1);
            sockets.splice(sockets.indexOf(socket),1);
        });

        socket.on('data', function (data) {

            for (let i = 0; i <= data.toString().split("\r\n").length; i++) {
                let ndata = data.toString().split("\r\n")[i];
                if (typeof ndata  == "undefined") return;

            if (ndata.split(':')[0] == "KEY") {

          
        if (config.SERVER_KEY == ndata.toString().split(':')[1]) {
            console.log('authenticated');
            sockets.push(socket)
            socket.write("AUTHENTICATED-OK:0"+ '\r\n');
        } else {
            socket.destroy();
            connected_ips.splice( connected_ips.indexOf(socket.remoteAddress),1);
            console.log('Login failed, disconnecting');
        }
             
    }  
    if (ndata.toString().split(':')[0] == "PING") {
        console.log('recieved ping '  );
      //  socket.write('PONG:' + Math.round((new Date()).getTime() / 1000)+ '\r\n');
      socket.write('PONG:' + ndata.toString().split(':')[1]+ '\r\n');
    }


}
        })


    });

 

    app.get('/phonebook', async function (req, res) {
        if (req.query.key != config.SERVER_KEY) {
            res.status(404).send()
            return;

        }
        if (decodeURIComponent(req.query.load) == 1) {

            let json = await Fs.readFile(config.PHONEBOOK)
            let phonebook = {};
            try {
         phonebook = JSON.parse(json);
            } catch {

            }

            res.status(200).send(phonebook)
            


        }  
        else if (decodeURIComponent(req.query.delete) > 0) {
            console.log( req.query.change + ' ' + req.query.name + ' '  + req.query.address);
            let json = await Fs.readFile(config.PHONEBOOK)
            let phonebook = JSON.parse(json)
           delete phonebook[req.query.delete] 

            fs.writeFile(config.PHONEBOOK, JSON.stringify(phonebook), (err) => {
                if (err)
                    console.log(err);
                else {
                    res.status(200).send("1");
                }
            });;


        }
       else if (decodeURIComponent(req.query.change) > 0) {
          console.log( req.query.change + ' ' + req.query.name + ' '  + req.query.address);

          let phonebook = {};
          let json = await Fs.readFile(config.PHONEBOOK)
       try {
          phonebook = JSON.parse(json);
       } catch {
        phonebook = {};
       }
         
          phonebook[req.query.change] = {
            name: req.query.name,
            address: req.query.address,
            phone: req.query.change
          }
          
          fs.writeFile(config.PHONEBOOK, JSON.stringify(phonebook), (err) => {
            if (err)
                console.log(err);
            else {
                res.status(200).send("1");
            }
        });;

        } else {
            res.status(404).send("");
        }

    
    });



    app.get('/', async function (req, res) {
        let num = decodeURIComponent(req.query.phone);
        let isServer = decodeURIComponent(req.query.server);
        num = num.trim().replace('30', '');
        num = num.replace('+', '');
        console.log("Look up request: " + num);
      


        try {
            let json = await Fs.readFile(config.PHONEBOOK)
            let phonebook = JSON.parse(json)


            if (typeof phonebook[num] !== "undefined") {


                console.log('Found Phonebook entry: ' + phonebook[num]);
                res.status(200).send(phonebook[num].name);


                sockets.forEach((socket) => {
                    socket.write("CLIENT-DATA:" + phonebook[num].name + ":" + phonebook[num].address + ":" + num + ":Phonebook:" + "0");
                    socket.write("SHOW-WINDOW:0");
                    socket.write("SHOW-WINDOW:0");
         
                })

                return;
            }



        }
        catch (err) {
            console.log(err.message);
        }
        //then SELECT mssql stuff...

        ress = await Anagnorisi(num);

        if (typeof ress.address !== "undefined") {
            console.log('Found Winlift entry: ' + ress.address + ' | ' + ress.name);
            if (isServer != "true") 
            {
            res.status(200).send(ress.address + ' | ' + ress.name);
            sockets.forEach((socket) => {
              //  console.log("CLIENT-DATA:" + ress.name + ":" + ress.address + ":" + num + ":WinLift:" + ress.id);
                socket.write("CLIENT-DATA:" + ress.name + ":" + ress.address + ":" + num + ":WinLift:" + ress.id);
                socket.write("SHOW-WINDOW:0");
                socket.write("SHOW-WINDOW:0");
            })
        } else {
            let branch = await contact_id_to_branch_id(ress.id);
            let myObj = {
                name: ress.name,
                address: ress.address,
                winlift: true,
                id:  ress.id,
                branch:  branch
            }
        
      
            res.status(200).json(myObj);
        }

            //ress.id contains contact id...

        } else {
            if (!config.ENABLE_11888) {
                if (isServer != "true") 
                {
                res.status(404).send();
                sockets.forEach((socket) => {
                socket.write("CLIENT-DATA:Unknown:-:" + num + ":Not found:" + "0");
                socket.write("SHOW-WINDOW:0");
                socket.write("SHOW-WINDOW:0");
            })
        }
                return;
            }

            console.log(num + ' :Not found, trying 11888')
            ress = await api_11888(num);
            if (ress) {
                if (isServer != "true") 
                {
                
                    res.status(200).send(ress.name + " | " + ress.address);
                sockets.forEach((socket) => {
                    socket.write("CLIENT-DATA:" + ress.name + ":" + ress.address + ":" + num + ":11888:" + "0");
                    socket.write("SHOW-WINDOW:0");
                    socket.write("SHOW-WINDOW:0");
                })
                } else {

                    let myObj = {
                        name: ress.name,
                        address: ress.address,
                        winlift: false,
                        id:  0
                    }

                    res.status(200).json(myObj);
                }

            } else {
                if (isServer != "true") 
                {
                     
                res.status(404).send();
                sockets.forEach((socket) => {
                socket.write("CLIENT-DATA:Unknown:-:" + num + ":Not found:" + "0");
                socket.write("SHOW-WINDOW:0");
                socket.write("SHOW-WINDOW:0");
            })
        }
            }




        }
    });

    app.listen(config.PORT, function () {
        console.log('Server is running on PORT:', config.PORT);
    });





})
async function api_11888(s) {
    let cache = {};

    return new Promise(async (resolve, reject) => {

        //try a cache 



        try {
            let json = await Fs.readFile("./cache.json")
            cache = JSON.parse(json)

            if (typeof cache[s] !== "undefined") {

                if ( cache[s] != false) {
                console.log('Found cache entry: ' + cache[s].split(":cAdd10:")[0] + ' ' + cache[s].split(":cAdd10:")[1]);
            
                resolve({
                    name: cache[s].split(":cAdd10:")[0],
                    address: cache[s].split(":cAdd10:")[1]
                });
            } else {
                resolve(false);
            }
            }
        }
        catch (err) {
            console.log(err.message);
        }
        //try the real


        let url = "https://www.11888.gr/search/reverse/?phone=" + s;

        https.get(url, (res) => {
            let body = "";
            let getit = false;;
            res.on("data", (chunk) => {
                body += chunk;
            });

            res.on("end", () => {

                let Obj = {
                    name: '',
                    address: ''
                }
                try {
                    let json = JSON.parse(body);
                    if (typeof json.data.wp[0].name.last != 'undefined') {
                        if (json.data.wp[0].name.last) getit = capitalizeFirstLetter(json.data.wp[0].name.last.trim().toLowerCase()) + ' ';
                    }
                    if (typeof json.data.wp[0].name.first != 'undefined') {
                        if (typeof json.data.wp[0].name.last != 'undefined') {
                            if (json.data.wp[0].name.first) getit = capitalizeFirstLetter(json.data.wp[0].name.first.trim().toLowerCase()).substr(0, 1) + '. ' + getit;
                        } else {
                            if (json.data.wp[0].name.first) getit = capitalizeFirstLetter(json.data.wp[0].name.first.trim().toLowerCase());
                        }
                    }
                    Obj.name = getit;
                    getit = '';
                    if (typeof json.data.wp[0].address.street1 != 'undefined') {
                        (json.data.wp[0].address.street1) ? getit = getit.substr(0, 15) + shortStreet(capitalizeFirstLetter(json.data.wp[0].address.street1.trim().toLowerCase())) + ' ' : '';
                    }
                    if (typeof json.data.wp[0].address.number1 != 'undefined') {
                        if (json.data.wp[0].address.number1) getit = getit + capitalizeFirstLetter(json.data.wp[0].address.number1.trim().toLowerCase()) + ' ';
                    }
                    if (typeof json.data.wp[0].address.municipality.name != 'undefined') {
                        if (json.data.wp[0].address.municipality.name) getit = getit + capitalizeFirstLetter(json.data.wp[0].address.municipality.name.trim().toLowerCase());
                    }
                    Obj.address = getit;





                    //save a cache



                    cache[s] =  Obj.name + ':cAdd10:' +  Obj.address;

                    fs.writeFile("./cache.json", JSON.stringify(cache), (err) => {
                        if (err)
                            console.log(err);
                        else {
                            //ok
                        }
                    });




                    //end saving a cache.

                    resolve(Obj);
                } catch (error) {

                    cache[s] = false;

                    fs.writeFile("./cache.json", JSON.stringify(cache), (err) => {
                        if (err)
                            console.log(err);
                        else {
                            //ok
                        }
                    });


                    resolve(false);
                    //console.error(error.message);
                };
            });

        }).on("error", (error) => {
            console.error(error.message);
        });
    });
}
function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function checkForFile(fileName, callback) {
    fs.exists(fileName, function (exists) {
        if (exists) {
            //
        } else {
            fs.closeSync(fs.openSync(fileName, 'w'));
        }
    });
}
function shortStreet(s) {
    if (s.split(' ').length > 1) {
        let a = s.split(' ')[0];
        let b = s.split(' ')[1];
        return a.substr(0, 1) + '. ' + b;
    } else {
        return s;
    }
}
process.on('uncaughtException', err => {
    console.error(err && err.stack)
});

