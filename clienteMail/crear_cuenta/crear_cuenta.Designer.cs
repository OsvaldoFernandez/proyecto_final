namespace clienteMail.crear_cuenta
{
    partial class crear_cuenta
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(crear_cuenta));
      this.button1 = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.mail = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.contrasena = new System.Windows.Forms.TextBox();
      this.proveedor = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.servidorpop3 = new System.Windows.Forms.TextBox();
      this.puertopop3 = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.puertosmtp = new System.Windows.Forms.TextBox();
      this.servidorsmtp = new System.Windows.Forms.TextBox();
      this.logoPic = new System.Windows.Forms.PictureBox();
      this.sslpop3 = new System.Windows.Forms.CheckBox();
      this.sslsmtp = new System.Windows.Forms.CheckBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.lblError = new System.Windows.Forms.Label();
      this.panel2 = new System.Windows.Forms.Panel();
      this.panel3 = new System.Windows.Forms.Panel();
      this.panel4 = new System.Windows.Forms.Panel();
      this.panel5 = new System.Windows.Forms.Panel();
      this.panel6 = new System.Windows.Forms.Panel();
      this.panel7 = new System.Windows.Forms.Panel();
      this.btnVolver = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.logoPic)).BeginInit();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel5.SuspendLayout();
      this.panel6.SuspendLayout();
      this.panel7.SuspendLayout();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.FlatAppearance.BorderSize = 3;
      this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.button1.ForeColor = System.Drawing.Color.White;
      this.button1.Location = new System.Drawing.Point(101, 489);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(184, 50);
      this.button1.TabIndex = 0;
      this.button1.Text = "Crear cuenta";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.label1.ForeColor = System.Drawing.Color.White;
      this.label1.Location = new System.Drawing.Point(106, 171);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(179, 30);
      this.label1.TabIndex = 1;
      this.label1.Text = "Cuenta de correo";
      // 
      // mail
      // 
      this.mail.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.mail.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.mail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(135)))), ((int)(((byte)(138)))));
      this.mail.Location = new System.Drawing.Point(7, 7);
      this.mail.Name = "mail";
      this.mail.Size = new System.Drawing.Size(375, 28);
      this.mail.TabIndex = 0;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.label2.ForeColor = System.Drawing.Color.White;
      this.label2.Location = new System.Drawing.Point(133, 256);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(121, 30);
      this.label2.TabIndex = 3;
      this.label2.Text = "Contraseña";
      // 
      // contrasena
      // 
      this.contrasena.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.contrasena.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.contrasena.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(135)))), ((int)(((byte)(138)))));
      this.contrasena.Location = new System.Drawing.Point(6, 6);
      this.contrasena.Name = "contrasena";
      this.contrasena.PasswordChar = '*';
      this.contrasena.Size = new System.Drawing.Size(375, 28);
      this.contrasena.TabIndex = 1;
      // 
      // proveedor
      // 
      this.proveedor.BackColor = System.Drawing.Color.White;
      this.proveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.proveedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.proveedor.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.proveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(135)))), ((int)(((byte)(138)))));
      this.proveedor.FormattingEnabled = true;
      this.proveedor.Location = new System.Drawing.Point(6, 5);
      this.proveedor.Name = "proveedor";
      this.proveedor.Size = new System.Drawing.Size(375, 38);
      this.proveedor.TabIndex = 2;
      this.proveedor.SelectedIndexChanged += new System.EventHandler(this.proveedor_SelectedIndexChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.label3.ForeColor = System.Drawing.Color.White;
      this.label3.Location = new System.Drawing.Point(96, 333);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(189, 30);
      this.label3.TabIndex = 6;
      this.label3.Text = "Proveedor de mail";
      this.label3.Click += new System.EventHandler(this.label3_Click);
      // 
      // servidorpop3
      // 
      this.servidorpop3.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.servidorpop3.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.servidorpop3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(135)))), ((int)(((byte)(138)))));
      this.servidorpop3.Location = new System.Drawing.Point(4, 5);
      this.servidorpop3.Name = "servidorpop3";
      this.servidorpop3.Size = new System.Drawing.Size(363, 28);
      this.servidorpop3.TabIndex = 7;
      // 
      // puertopop3
      // 
      this.puertopop3.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.puertopop3.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.puertopop3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(135)))), ((int)(((byte)(138)))));
      this.puertopop3.Location = new System.Drawing.Point(6, 6);
      this.puertopop3.Name = "puertopop3";
      this.puertopop3.Size = new System.Drawing.Size(363, 28);
      this.puertopop3.TabIndex = 8;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.label7.ForeColor = System.Drawing.Color.White;
      this.label7.Location = new System.Drawing.Point(547, 432);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(138, 30);
      this.label7.TabIndex = 14;
      this.label7.Text = "Puerto SMTP";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.label6.ForeColor = System.Drawing.Color.White;
      this.label6.Location = new System.Drawing.Point(540, 360);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(154, 30);
      this.label6.TabIndex = 13;
      this.label6.Text = "Servidor SMTP";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.label5.ForeColor = System.Drawing.Color.White;
      this.label5.Location = new System.Drawing.Point(547, 253);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(136, 30);
      this.label5.TabIndex = 12;
      this.label5.Text = "Puerto POP3";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.label4.ForeColor = System.Drawing.Color.White;
      this.label4.Location = new System.Drawing.Point(540, 179);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(152, 30);
      this.label4.TabIndex = 11;
      this.label4.Text = "Servidor POP3";
      // 
      // puertosmtp
      // 
      this.puertosmtp.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.puertosmtp.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.puertosmtp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(135)))), ((int)(((byte)(138)))));
      this.puertosmtp.Location = new System.Drawing.Point(4, 6);
      this.puertosmtp.Name = "puertosmtp";
      this.puertosmtp.Size = new System.Drawing.Size(363, 28);
      this.puertosmtp.TabIndex = 10;
      // 
      // servidorsmtp
      // 
      this.servidorsmtp.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.servidorsmtp.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
      this.servidorsmtp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(135)))), ((int)(((byte)(138)))));
      this.servidorsmtp.Location = new System.Drawing.Point(5, 6);
      this.servidorsmtp.Name = "servidorsmtp";
      this.servidorsmtp.Size = new System.Drawing.Size(363, 28);
      this.servidorsmtp.TabIndex = 9;
      // 
      // logoPic
      // 
      this.logoPic.Image = ((System.Drawing.Image)(resources.GetObject("logoPic.Image")));
      this.logoPic.Location = new System.Drawing.Point(116, -35);
      this.logoPic.Name = "logoPic";
      this.logoPic.Size = new System.Drawing.Size(150, 150);
      this.logoPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.logoPic.TabIndex = 10;
      this.logoPic.TabStop = false;
      // 
      // sslpop3
      // 
      this.sslpop3.AutoSize = true;
      this.sslpop3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.sslpop3.ForeColor = System.Drawing.Color.White;
      this.sslpop3.Location = new System.Drawing.Point(429, 333);
      this.sslpop3.Name = "sslpop3";
      this.sslpop3.Size = new System.Drawing.Size(345, 21);
      this.sslpop3.TabIndex = 15;
      this.sslpop3.Text = "El servidor POP3 requiere una conexión segura (SSL)";
      this.sslpop3.UseVisualStyleBackColor = true;
      // 
      // sslsmtp
      // 
      this.sslsmtp.AutoSize = true;
      this.sslsmtp.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.sslsmtp.ForeColor = System.Drawing.Color.White;
      this.sslsmtp.Location = new System.Drawing.Point(429, 518);
      this.sslsmtp.Name = "sslsmtp";
      this.sslsmtp.Size = new System.Drawing.Size(346, 21);
      this.sslsmtp.TabIndex = 16;
      this.sslsmtp.Text = "El servidor SMTP requiere una conexión segura (SSL)";
      this.sslsmtp.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.Red;
      this.panel1.Controls.Add(this.proveedor);
      this.panel1.ForeColor = System.Drawing.Color.Red;
      this.panel1.Location = new System.Drawing.Point(12, 366);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(387, 49);
      this.panel1.TabIndex = 17;
      // 
      // lblError
      // 
      this.lblError.AutoSize = true;
      this.lblError.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
      this.lblError.ForeColor = System.Drawing.Color.Red;
      this.lblError.Location = new System.Drawing.Point(12, 435);
      this.lblError.Name = "lblError";
      this.lblError.Size = new System.Drawing.Size(365, 25);
      this.lblError.TabIndex = 18;
      this.lblError.Text = "Por favor, complete los datos incorrectos.";
      this.lblError.Visible = false;
      // 
      // panel2
      // 
      this.panel2.BackColor = System.Drawing.Color.Red;
      this.panel2.Controls.Add(this.contrasena);
      this.panel2.Location = new System.Drawing.Point(12, 290);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(387, 40);
      this.panel2.TabIndex = 19;
      this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
      // 
      // panel3
      // 
      this.panel3.BackColor = System.Drawing.Color.Red;
      this.panel3.Controls.Add(this.mail);
      this.panel3.Location = new System.Drawing.Point(12, 205);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(387, 42);
      this.panel3.TabIndex = 20;
      // 
      // panel4
      // 
      this.panel4.BackColor = System.Drawing.Color.Red;
      this.panel4.Controls.Add(this.servidorpop3);
      this.panel4.Location = new System.Drawing.Point(425, 208);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(375, 38);
      this.panel4.TabIndex = 21;
      // 
      // panel5
      // 
      this.panel5.BackColor = System.Drawing.Color.Red;
      this.panel5.Controls.Add(this.puertopop3);
      this.panel5.Location = new System.Drawing.Point(423, 282);
      this.panel5.Name = "panel5";
      this.panel5.Size = new System.Drawing.Size(376, 39);
      this.panel5.TabIndex = 22;
      // 
      // panel6
      // 
      this.panel6.BackColor = System.Drawing.Color.Red;
      this.panel6.Controls.Add(this.servidorsmtp);
      this.panel6.Location = new System.Drawing.Point(424, 392);
      this.panel6.Name = "panel6";
      this.panel6.Size = new System.Drawing.Size(374, 40);
      this.panel6.TabIndex = 23;
      // 
      // panel7
      // 
      this.panel7.BackColor = System.Drawing.Color.Red;
      this.panel7.Controls.Add(this.puertosmtp);
      this.panel7.Location = new System.Drawing.Point(425, 462);
      this.panel7.Name = "panel7";
      this.panel7.Size = new System.Drawing.Size(372, 40);
      this.panel7.TabIndex = 24;
      // 
      // btnVolver
      // 
      this.btnVolver.FlatAppearance.BorderSize = 3;
      this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnVolver.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
      this.btnVolver.ForeColor = System.Drawing.Color.White;
      this.btnVolver.Location = new System.Drawing.Point(12, 77);
      this.btnVolver.Name = "btnVolver";
      this.btnVolver.Size = new System.Drawing.Size(96, 38);
      this.btnVolver.TabIndex = 25;
      this.btnVolver.Text = "Volver";
      this.btnVolver.UseVisualStyleBackColor = true;
      this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
      // 
      // crear_cuenta
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(183)))), ((int)(((byte)(248)))));
      this.ClientSize = new System.Drawing.Size(416, 551);
      this.Controls.Add(this.btnVolver);
      this.Controls.Add(this.panel7);
      this.Controls.Add(this.panel6);
      this.Controls.Add(this.panel5);
      this.Controls.Add(this.panel4);
      this.Controls.Add(this.panel3);
      this.Controls.Add(this.lblError);
      this.Controls.Add(this.sslsmtp);
      this.Controls.Add(this.sslpop3);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.logoPic);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.panel2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "crear_cuenta";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Registrarme";
      this.Load += new System.EventHandler(this.crear_cuenta_Load);
      ((System.ComponentModel.ISupportInitialize)(this.logoPic)).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.panel7.ResumeLayout(false);
      this.panel7.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox contrasena;
        private System.Windows.Forms.ComboBox proveedor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox servidorpop3;
        private System.Windows.Forms.TextBox puertopop3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox puertosmtp;
        private System.Windows.Forms.TextBox servidorsmtp;
        private System.Windows.Forms.PictureBox logoPic;
        private System.Windows.Forms.CheckBox sslpop3;
        private System.Windows.Forms.CheckBox sslsmtp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnVolver;
    }
}