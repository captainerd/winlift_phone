# winlift_phone
A bridge for BPX's and phones for datasolution's winlift ERP (Client C# 6 (core) NET  - Server: Node - express)

This implementation is using a simple ping-pong tcp client/server model, server-client wasn't nessesary
just in order to subscribe more than 1 clients for the same lookups/Trunk phone line
also for not having to deal what is the IP of each terminal if it is dynamic 
and if it has a firewall blocking ) 

All BPX's and phone devices have a feature  to lookup a CID (in simple terms, run an HTTP 
request to a url of your choice where a variable represents the phone number of the caller) 

For example: 

A. In Asterisk this can be done either individually for each different extension via AGI in dial plan or globally via cid lookup  
B. In GrandStream phones this can be done by setting in their web interface "Action URL" eg. http://192.168.7.6:3535/?phone={caller_id}

Installation:

1. Compile the C# client and create a setup.
2. Install "CIDlookup" just paste it on the same server that holds the 
   MSQL database and edit config.json, 
   then run in cmd. "npm install" to install node modules.
   and node ./server.js, or setup as "windows startup service" the RunServer.bat
   
 Code is small thous is easily readable, i left variables un-named for speed.
