using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Win32;

namespace CallerIdentity
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private void Form3_Load(object sender, EventArgs e)
        {
        
            if (rkApp.GetValue("CallerID Oracle") == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                checkBox1.Checked = false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                checkBox1.Checked = true;
            }

            txtserver_ip.Text = Properties.Settings.Default.server_ip;
            txtserver_port.Text = Properties.Settings.Default.server_port.ToString();
            txt_api_url.Text = Properties.Settings.Default.API_address;
            txt_grand_pass.Text = Properties.Settings.Default.grandstream_pass;
            txt_grand_user.Text = Properties.Settings.Default.grandstream_user;
            txt_server_secret.Text = Properties.Settings.Default.server_secret;
            txt_grandstream_ip.Text = Properties.Settings.Default.grandstream_ip;
            chk_sound.Checked = Properties.Settings.Default.play_sound;
            checkBox2.Checked = Properties.Settings.Default.scrolling;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked) rkApp.SetValue("CallerID Oracle", Application.ExecutablePath); 
            if (!checkBox1.Checked) rkApp.DeleteValue("CallerID Oracle", false);

            Properties.Settings.Default.server_ip = txtserver_ip.Text;
             Properties.Settings.Default.server_port = int.Parse(txtserver_port.Text);
            Properties.Settings.Default.API_address = txt_api_url.Text;
            Properties.Settings.Default.grandstream_pass = txt_grand_pass.Text;
  
      Properties.Settings.Default.grandstream_user = txt_grand_user.Text;
            Properties.Settings.Default.server_secret = txt_server_secret.Text;
            Properties.Settings.Default.grandstream_ip = txt_grandstream_ip.Text;
            Properties.Settings.Default.play_sound = chk_sound.Checked;
            Properties.Settings.Default.scrolling = checkBox2.Checked;


            Properties.Settings.Default.Save();
 
            
            this.Hide();
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
