namespace CallerIdentity
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtserver_ip = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtserver_port = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_api_url = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_server_secret = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_grandstream_ip = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_grand_user = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_grand_pass = new System.Windows.Forms.TextBox();
            this.chk_sound = new System.Windows.Forms.CheckBox();
            this.Save = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server IP:";
            // 
            // txtserver_ip
            // 
            this.txtserver_ip.Location = new System.Drawing.Point(264, 33);
            this.txtserver_ip.Name = "txtserver_ip";
            this.txtserver_ip.Size = new System.Drawing.Size(252, 39);
            this.txtserver_ip.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(522, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 32);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // txtserver_port
            // 
            this.txtserver_port.Location = new System.Drawing.Point(599, 33);
            this.txtserver_port.Name = "txtserver_port";
            this.txtserver_port.Size = new System.Drawing.Size(115, 39);
            this.txtserver_port.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 32);
            this.label3.TabIndex = 4;
            this.label3.Text = "API ip/domain:";
            // 
            // txt_api_url
            // 
            this.txt_api_url.Location = new System.Drawing.Point(264, 98);
            this.txt_api_url.Name = "txt_api_url";
            this.txt_api_url.Size = new System.Drawing.Size(450, 39);
            this.txt_api_url.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 32);
            this.label4.TabIndex = 6;
            this.label4.Text = "Server secret:";
            // 
            // txt_server_secret
            // 
            this.txt_server_secret.Location = new System.Drawing.Point(264, 168);
            this.txt_server_secret.Name = "txt_server_secret";
            this.txt_server_secret.Size = new System.Drawing.Size(450, 39);
            this.txt_server_secret.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 254);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 32);
            this.label5.TabIndex = 8;
            this.label5.Text = "Grandstream IP:";
            // 
            // txt_grandstream_ip
            // 
            this.txt_grandstream_ip.Location = new System.Drawing.Point(264, 247);
            this.txt_grandstream_ip.Name = "txt_grandstream_ip";
            this.txt_grandstream_ip.Size = new System.Drawing.Size(450, 39);
            this.txt_grandstream_ip.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 317);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(207, 32);
            this.label6.TabIndex = 10;
            this.label6.Text = "Grandstream user:";
            // 
            // txt_grand_user
            // 
            this.txt_grand_user.Location = new System.Drawing.Point(264, 314);
            this.txt_grand_user.Name = "txt_grand_user";
            this.txt_grand_user.Size = new System.Drawing.Size(200, 39);
            this.txt_grand_user.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 376);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(208, 32);
            this.label7.TabIndex = 12;
            this.label7.Text = "Grandstream pass:";
            // 
            // txt_grand_pass
            // 
            this.txt_grand_pass.Location = new System.Drawing.Point(264, 385);
            this.txt_grand_pass.Name = "txt_grand_pass";
            this.txt_grand_pass.PasswordChar = '*';
            this.txt_grand_pass.Size = new System.Drawing.Size(200, 39);
            this.txt_grand_pass.TabIndex = 13;
            // 
            // chk_sound
            // 
            this.chk_sound.AutoSize = true;
            this.chk_sound.Location = new System.Drawing.Point(57, 456);
            this.chk_sound.Name = "chk_sound";
            this.chk_sound.Size = new System.Drawing.Size(318, 36);
            this.chk_sound.TabIndex = 15;
            this.chk_sound.Text = "Enable sound notification";
            this.chk_sound.UseVisualStyleBackColor = true;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(103, 578);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(214, 46);
            this.Save.TabIndex = 16;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(355, 578);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(186, 46);
            this.button1.TabIndex = 17;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(419, 456);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(303, 36);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "Run on windows startup";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(57, 512);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(301, 36);
            this.checkBox2.TabIndex = 19;
            this.checkBox2.Text = "Display scrolling banner";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 652);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.chk_sound);
            this.Controls.Add(this.txt_grand_pass);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_grand_user);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_grandstream_ip);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_server_secret);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_api_url);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtserver_port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtserver_ip);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form3";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox txtserver_ip;
        private Label label2;
        private TextBox txtserver_port;
        private Label label3;
        private TextBox txt_api_url;
        private Label label4;
        private TextBox txt_server_secret;
        private Label label5;
        private TextBox txt_grandstream_ip;
        private Label label6;
        private TextBox txt_grand_user;
        private Label label7;
        private TextBox txt_grand_pass;
        private CheckBox chk_sound;
        private Button Save;
        private Button button1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
    }
}