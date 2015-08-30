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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.mail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.contrasena = new System.Windows.Forms.TextBox();
            this.proveedor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.servidorpop3 = new System.Windows.Forms.TextBox();
            this.puertopop3 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.puertosmtp = new System.Windows.Forms.TextBox();
            this.servidorsmtp = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(165, 229);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Crear cuenta";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mail";
            // 
            // mail
            // 
            this.mail.Location = new System.Drawing.Point(144, 81);
            this.mail.Name = "mail";
            this.mail.Size = new System.Drawing.Size(188, 20);
            this.mail.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Contraseña";
            // 
            // contrasena
            // 
            this.contrasena.Location = new System.Drawing.Point(144, 122);
            this.contrasena.Name = "contrasena";
            this.contrasena.Size = new System.Drawing.Size(188, 20);
            this.contrasena.TabIndex = 4;
            // 
            // proveedor
            // 
            this.proveedor.FormattingEnabled = true;
            this.proveedor.Location = new System.Drawing.Point(144, 161);
            this.proveedor.Name = "proveedor";
            this.proveedor.Size = new System.Drawing.Size(188, 21);
            this.proveedor.TabIndex = 5;
            this.proveedor.SelectedIndexChanged += new System.EventHandler(this.proveedor_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Proveedor de mail";
            // 
            // servidorpop3
            // 
            this.servidorpop3.Location = new System.Drawing.Point(175, 34);
            this.servidorpop3.Name = "servidorpop3";
            this.servidorpop3.Size = new System.Drawing.Size(152, 20);
            this.servidorpop3.TabIndex = 7;
            // 
            // puertopop3
            // 
            this.puertopop3.Location = new System.Drawing.Point(175, 73);
            this.puertopop3.Name = "puertopop3";
            this.puertopop3.Size = new System.Drawing.Size(152, 20);
            this.puertopop3.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.puertosmtp);
            this.groupBox1.Controls.Add(this.servidorsmtp);
            this.groupBox1.Controls.Add(this.servidorpop3);
            this.groupBox1.Controls.Add(this.puertopop3);
            this.groupBox1.Location = new System.Drawing.Point(416, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 225);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Servidor";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(81, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Puerto SMTP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(76, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Servidor SMTP";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(84, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Puerto POP3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(76, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Servidor POP3";
            // 
            // puertosmtp
            // 
            this.puertosmtp.Location = new System.Drawing.Point(175, 149);
            this.puertosmtp.Name = "puertosmtp";
            this.puertosmtp.Size = new System.Drawing.Size(152, 20);
            this.puertosmtp.TabIndex = 10;
            // 
            // servidorsmtp
            // 
            this.servidorsmtp.Location = new System.Drawing.Point(175, 110);
            this.servidorsmtp.Name = "servidorsmtp";
            this.servidorsmtp.Size = new System.Drawing.Size(152, 20);
            this.servidorsmtp.TabIndex = 9;
            // 
            // crear_cuenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 298);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.proveedor);
            this.Controls.Add(this.contrasena);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "crear_cuenta";
            this.Text = "crear_cuenta";
            this.Load += new System.EventHandler(this.crear_cuenta_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox puertosmtp;
        private System.Windows.Forms.TextBox servidorsmtp;
    }
}