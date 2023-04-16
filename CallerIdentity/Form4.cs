using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CallerIdentity
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        public void say(String s)
        {
            var rec = Screen.PrimaryScreen.WorkingArea;
            int margain = 10;

            this.Location = new Point(rec.Width - (this.Width + margain), rec.Height - (this.Height + margain));

            label1.Text = s;
            label1.Left = 2500;
        }
        private void Form4_Click(object sender, PaintEventArgs e)
        {
            var mainForm = Application.OpenForms.OfType<Form1>().Single();
            mainForm.Show();
            mainForm.Activate();
            mainForm.Focus();
            mainForm.WindowState = FormWindowState.Normal;
            this.Hide();
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            var rec = Screen.PrimaryScreen.WorkingArea;
            int margain = 10;
            this.Location = new Point(rec.Width - (this.Width + margain), rec.Height - (this.Height + margain));

            timer2.Enabled = true;
            label1.Left = 2500;


        }
        private void label1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label1.DisplayRectangle, Color.Black, ButtonBorderStyle.Outset);
        }
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            this.TopMost = true;
            var mainForm = Application.OpenForms.OfType<Form1>().Single();
            mainForm.Show();
            mainForm.Activate();
            mainForm.Focus();
            mainForm.WindowState = FormWindowState.Normal;
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Left = label1.Left - 3;
      

            if (label1.Left < -1000)
            {
                label1.Left = 2500;
            }




        }

        private void timer2_Tick(object sender, EventArgs e)
        {
     
            timer2.Enabled = false;
            this.Hide();
    
        }
    }
}
