using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CallerIdentity
{

    public partial class Form1 : Form
    {

        Form4 f4 = new Form4();
        private string server_key = Properties.Settings.Default.server_secret;
        private bool _ForceClose = false;
        private bool color_flash_state = false;
        TcpClient tcpclnt = new TcpClient();
        //Declare and Initialize the IP Adress
        System.Media.SoundPlayer player = new System.Media.SoundPlayer((@"c:\Windows\Media\Ring04.wav"));
        IPAddress ipAd = IPAddress.Parse(Properties.Settings.Default.server_ip);
        private bool authenticated_to_server = false;
        private long lastpong = 0;
        //Declare and Initilize the Port Number;
        int PortNumber = Properties.Settings.Default.server_port;
      
        phonebookentry[] pentries_phonebook = new phonebookentry[0];
        phonebookentry[] pentries = new phonebookentry[0];
        private String selected_tocall  = "";
        public class phonebookentry
        {
            public string name { get; set; }
            public string address { get; set; }
            public string phone { get; set; }
            public string winlift_id { get; set; }
            public string source { get; set; }
            public string datetime { get; set; }

        }

        public Form1()
        {
            InitializeComponent();
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.notifyIcon1.MouseDoubleClick += notifyIcon1_MouseClick;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //label1.Text = "test";
           
        }
        public void notifyIcon1_MouseClick(object sender, MouseEventArgs e)


        {

            ShowForm();
            notifyIcon1.Visible = false;
        }

 
        private async void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("http://" + Properties.Settings.Default.grandstream_ip + "/cgi-bin/api-make_call?phonenumber=" + selected_tocall + "&account=" + Properties.Settings.Default.grandstream_user + "&password=" + Properties.Settings.Default.grandstream_pass);

            HttpClient client = new HttpClient();
                //http://192.168.x.xx/cgi-bin/api-make_call?phonenumber=xxxxxxxx&account=admin&password=teleAdmin
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
           
                try
                {
                    HttpResponseMessage response =   await Task.Run(() => client.GetAsync("http://" + Properties.Settings.Default.grandstream_ip + "/cgi-bin/api-make_call?phonenumber=" + selected_tocall + "&account=" + Properties.Settings.Default.grandstream_user + "&password=" + Properties.Settings.Default.grandstream_pass).Result);

                } catch { }
          

        }
        public async void loadPhonebook()
        {
            try
            {

                comboBox1.Items.Clear();
                 pentries_phonebook = new phonebookentry[0];

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://" + Properties.Settings.Default.API_address + "/phonebook");

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.

                HttpResponseMessage response = await Task.Run(() => client.GetAsync("?load=1&key=" + server_key).Result);
                {
                    // Parse the response body.

                    dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    var i = 0;
                    foreach (var item in json)
                    {

                        foreach (var itema in item)
                        {

                            phonebookentry la = itema.ToObject<phonebookentry>();
                            Array.Resize(ref pentries_phonebook, pentries_phonebook.Length + 1);
                            pentries_phonebook[i] = la;
                            i = i + 1;
                        }
                    }
                }
                //Got phonebook from sotlift server?
                foreach (var item in pentries_phonebook)
                {

                    //MessageBox.Show(item.name); 
                   
                    comboBox1.Items.Add(item.name);
           

                }

                client.Dispose();
            }
            catch { }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           

            

            // this.loadPhonebook();
            try
            {
                tcpclnt = new TcpClient();
                tcpclnt.ConnectAsync(ipAd, PortNumber);
                this.loadPhonebook();
            }
            catch { }

            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    Hide();
                }));
            }

            seen_close();
          
            label3.BackColor = Form1.DefaultBackColor;

            label3.ReadOnly = true;
            label2.Text = "No new calls yet";
            label3.Text = "...";
           // seen_close();



        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_ForceClose)
            {

                // Cancel the closure
                e.Cancel = true;
                seen_close();
            }
        }
        private void seen_close()
        {
            player.Stop();
         
            label3.ForeColor = Color.Black;
            timer2.Enabled = false;
    
            label2.Text = "No new calls yet";
            label3.Text = "Waiting...";
            label3.ForeColor = Color.Gray;
        
            label7.Text = "";
            this.Hide();
            notifyIcon1.Visible = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            seen_close();
        }
         
        private void tryAuthenticate()
        {
            try
            {
                int byteCount = Encoding.ASCII.GetByteCount("KEY:" + server_key + "\r\n");
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes("KEY:" + server_key + "\r\n");



                NetworkStream stream = tcpclnt.GetStream();
                stream.Write(sendData, 0, sendData.Length);
            }
            catch { }
        }
        private void ShowForm()
        {


            if (Properties.Settings.Default.scrolling)
            {
                f4.Show(); // Shows Form2
                return;
            }

            this.Show();
            this.Activate();
            this.Focus();
            this.WindowState = FormWindowState.Minimized;

            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.Focus();
            this.ShowInTaskbar = true;
            this.TopMost = true;

        }
        private void read_socket()
        {
 
new Thread(() => 
{
    Thread.CurrentThread.IsBackground = true; 

  

    while (tcpclnt.Connected)
    {
        try
        {
          

            //  MessageBox.Show(tcpclnt.GetStream().DataAvailable.ToString());


            byte[] bb = new byte[300];
            int k = tcpclnt.GetStream().Read(bb, 0, 300);
            string Response = Encoding.UTF8.GetString(bb);

            for (int i = 0; i <= Response.Split("\r\n").Length; i++) {
                String switching = Response.Split("\r\n")[i];
                //psefto-mini custom RFC 
                switch (switching.Split(":")[0])
                {

                    case string x when x.CompareTo("PONG") == 0:

                        lastpong = long.Parse(switching.Split(":")[1]);




                        break;


                    case string x when x.CompareTo("AUTH") == 0:


                    
                        this.tryAuthenticate();


                        break;
                    //authed?
                    case string x when x.CompareTo("AUTHENTICATED-OK") == 0:
                        authenticated_to_server = true;

                        break;

                    //bring window on top
                    case string x when x.CompareTo("SHOW-WINDOW") == 0:

                        Invoke(new Action(() =>
                        {
                            this.ShowForm();

                        }));

                        if (Properties.Settings.Default.play_sound) player.Play();



                        //  if (Properties.Settings.Default.play_sound)  player.Play();



                        break;
                    //ktl
                    case string x when x.CompareTo("CLIENT-DATA") == 0:
                        //Response.Split(":")[1] = ONOMA [2] = DIEFTHINISI [3] = TILF [4] = source (winlift/1188/phonebook) [5] = CLIENT_ID (Ama htan winlift or 0 )

                        Invoke(new Action(() =>
                        {
                            f4.say("Incoming: " + Response.Split(":")[1] + " - " + Response.Split(":")[2]);
                            label2.Text = "New call comming:";
                            label3.Text = "Caller: " + Response.Split(":")[1] + " \r\nAddress: " + Response.Split(":")[2] + " \r\nNo: " + Response.Split(":")[3];
                            label7.Text = Response.Split(":")[4];
                            timer2.Enabled = true;
                            phonebookentry eggrafi = new phonebookentry();
                            eggrafi.name = Response.Split(":")[1];
                            eggrafi.address = Response.Split(":")[2];
                            eggrafi.phone = Response.Split(":")[3];
                            eggrafi.winlift_id = Response.Split(":")[5];

                            eggrafi.datetime = DateTime.Now.ToString("h:mm:ss d-M-y");
                            eggrafi.source = Response.Split(":")[4];


                            int synolo = pentries.Length + 1;
                            listBox1.Items.Insert(0, "[" + eggrafi.datetime + "] " + eggrafi.name + ", " + eggrafi.phone);

                            Array.Resize(ref pentries, synolo);
                            pentries[synolo - 1] = eggrafi;


                        }));


                        break;

                    default:

                        break;
                }

            }

        }
        catch { }
    }
}).Start();

        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _ForceClose = true;
            this.Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
            if (!this.color_flash_state)
            {

                label3.ForeColor = Color.Red;
                this.color_flash_state = true;
            } else
            {
                this.color_flash_state = false;
                label3.ForeColor = Color.Black;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
        
                    if (!tcpclnt.Connected)
                    {
                        // Client disconnected
                        label5.Text = "Disconnected";
                        label5.ForeColor = Color.Red;


                        try
                        {
               
                     
                            tcpclnt.Close();
                            tcpclnt = new TcpClient();
                            tcpclnt.ConnectAsync(ipAd, PortNumber);
                     
                        }
                        catch { }

                    }
                    else
                    {
                        label5.Text = "Connected";

                       
                        this.read_socket();
                        label5.ForeColor = Color.Green;
                    }
                
            }
            catch {

              

            }

          
        }
        private void editPhonebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog(); // Shows Form2
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                //egine sorted den yparxia antoistixia index k object
          //      comboBox1.Text = pentries[listBox1.SelectedIndex].phone;
          //      selected_tocall = pentries[listBox1.SelectedIndex].phone;
              button2.Enabled = true;

                selected_tocall = listBox1.SelectedItem.ToString().Split(',')[listBox1.SelectedItem.ToString().Split(',').Length-1].Trim();
                comboBox1.Text = selected_tocall;

            } else
            {
                button2.Enabled = false;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {


            try
            { 
                long unixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                if (unixTime - lastpong > 40)
                {
              
                    //parapanw apo 20sec epese.
                    label5.Text = "Ping timeout";
                    label5.ForeColor = Color.Red;



                        tcpclnt.Close();
                        tcpclnt = new TcpClient();
                        tcpclnt.ConnectAsync(ipAd, PortNumber);
 

                }


                String Ping = "PING:" + unixTime.ToString() + "\r\n";
                int byteCount = Encoding.ASCII.GetByteCount(Ping);
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(Ping);



                NetworkStream stream = tcpclnt.GetStream();
                stream.Write(sendData, 0, sendData.Length);
            }
            catch { }
            
            //anagastika ping/pong 
                     // ;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f2 = new Form3();
            f2.ShowDialog(); // Shows Form2
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;

            MessageBox.Show("CallerID Oracle Version: " + version + "\n\nDesigned for Sot/Omikron Lift \n by captainerd (@github)");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                comboBox1.Text = pentries_phonebook[comboBox1.SelectedIndex].name;
                selected_tocall = pentries_phonebook[comboBox1.SelectedIndex].phone;
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }
    }


}

