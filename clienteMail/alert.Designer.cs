namespace clienteMail
{
    partial class frmAlert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAlert));
            this.lblMensaje = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.autenticacion_ok = new System.Windows.Forms.PictureBox();
            this.autenticacion_mal = new System.Windows.Forms.PictureBox();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autenticacion_ok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.autenticacion_mal)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.ForeColor = System.Drawing.Color.White;
            this.lblMensaje.Location = new System.Drawing.Point(38, 43);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(85, 25);
            this.lblMensaje.TabIndex = 24;
            this.lblMensaje.Text = "Mensaje";
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(225)))), ((int)(((byte)(242)))));
            this.panel10.Controls.Add(this.btnAceptar);
            this.panel10.Controls.Add(this.label7);
            this.panel10.Controls.Add(this.btnCerrar);
            this.panel10.Location = new System.Drawing.Point(43, 117);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(488, 105);
            this.panel10.TabIndex = 29;
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(204)))), ((int)(((byte)(245)))));
            this.btnAceptar.FlatAppearance.BorderSize = 2;
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAceptar.ForeColor = System.Drawing.Color.White;
            this.btnAceptar.Location = new System.Drawing.Point(22, 60);
            this.btnAceptar.Margin = new System.Windows.Forms.Padding(0);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAceptar.Size = new System.Drawing.Size(193, 30);
            this.btnAceptar.TabIndex = 10;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 15.25F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(96)))), ((int)(((byte)(113)))));
            this.label7.Location = new System.Drawing.Point(17, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(187, 30);
            this.label7.TabIndex = 0;
            this.label7.Text = "Comandos de voz";
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(204)))), ((int)(((byte)(245)))));
            this.btnCerrar.FlatAppearance.BorderSize = 2;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(259, 60);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(193, 30);
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // autenticacion_ok
            // 
            this.autenticacion_ok.Image = ((System.Drawing.Image)(resources.GetObject("autenticacion_ok.Image")));
            this.autenticacion_ok.Location = new System.Drawing.Point(517, 12);
            this.autenticacion_ok.Name = "autenticacion_ok";
            this.autenticacion_ok.Size = new System.Drawing.Size(40, 40);
            this.autenticacion_ok.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.autenticacion_ok.TabIndex = 31;
            this.autenticacion_ok.TabStop = false;
            this.autenticacion_ok.Visible = false;
            // 
            // autenticacion_mal
            // 
            this.autenticacion_mal.Image = ((System.Drawing.Image)(resources.GetObject("autenticacion_mal.Image")));
            this.autenticacion_mal.Location = new System.Drawing.Point(517, 12);
            this.autenticacion_mal.Name = "autenticacion_mal";
            this.autenticacion_mal.Size = new System.Drawing.Size(40, 40);
            this.autenticacion_mal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.autenticacion_mal.TabIndex = 30;
            this.autenticacion_mal.TabStop = false;
            this.autenticacion_mal.Visible = false;
            // 
            // frmAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(204)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(569, 234);
            this.Controls.Add(this.autenticacion_ok);
            this.Controls.Add(this.autenticacion_mal);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.lblMensaje);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmAlert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "alert";
            this.Deactivate += new System.EventHandler(this.frmAlert_Deactivate);
            this.Load += new System.EventHandler(this.frmAlert_Load);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autenticacion_ok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.autenticacion_mal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.PictureBox autenticacion_ok;
        private System.Windows.Forms.PictureBox autenticacion_mal;
    }
}