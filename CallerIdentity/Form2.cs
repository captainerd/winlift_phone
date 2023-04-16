using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CallerIdentity
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private string server_key = Properties.Settings.Default.server_secret;
        phonebookentry[] pentries = new phonebookentry[0];
        public class phonebookentry
        {
            public string name { get; set; }
            public string address { get; set;  }
            public string phone { get; set; }

        }
        private async void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://"  + Properties.Settings.Default.API_address + "/phonebook");

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
                            Array.Resize(ref pentries, pentries.Length + 1);
                            pentries[i] = la;
                            i = i + 1;
                        }
                    }
                }
                //Got phonebook from sotlift server?
                foreach (var item in pentries)
                {

                    //MessageBox.Show(item.name); 
                    listBox1.Items.Add(item.name);

                }
            
                    client.Dispose();
            } catch { }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtphone.Enabled = false;
            if (listBox1.SelectedItem != null)
            {
                txtphone.Text = pentries[listBox1.SelectedIndex].phone;
                txtname.Text = pentries[listBox1.SelectedIndex].name;
                txtadd.Text = pentries[listBox1.SelectedIndex].address;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var mainForm = Application.OpenForms.OfType<Form1>().Single();
            mainForm.loadPhonebook();
            this.Hide();
        }

        private async void button1_Click(object sender, EventArgs e)
        {


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://"  + Properties.Settings.Default.API_address + "/phonebook");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            
            // List data response.
            HttpResponseMessage response = await Task.Run(() =>  client.GetAsync("?change=" + txtphone.Text + "&name=" + txtname.Text + "&address=" + txtadd.Text + "&key=" + server_key).Result);   
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.

                var res =  response.Content.ReadAsStringAsync().Result;
                if (res.CompareTo("1") == 0)
                {
                    MessageBox.Show("Saved!");
                    var mainForm = Application.OpenForms.OfType<Form1>().Single();
                    mainForm.loadPhonebook();
                    this.Hide();
                }
            }
            }

        private void buttonnew_Click(object sender, EventArgs e)
        {
            txtphone.Enabled = true;
            txtphone.Text = "";
            txtadd.Text = "";
            txtname.Text = "";

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://" + Properties.Settings.Default.API_address + "/phonebook");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = await Task.Run(() => client.GetAsync("?delete=" + txtphone.Text + "&name=" + txtname.Text + "&address=" + txtadd.Text + "&key=" + server_key).Result);  
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.

                var res = response.Content.ReadAsStringAsync().Result;
                if (res.CompareTo("1") == 0)
                {
                    MessageBox.Show("Deleted!");
                    this.Hide();
                }
            }
        }
    }
}
