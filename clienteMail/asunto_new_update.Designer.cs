namespace clienteMail
{
    partial class asunto_new_update
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(asunto_new_update));
            this.btnGuardar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTexto = new System.Windows.Forms.TextBox();
            this.clear1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAsuntoVacio = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.White;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(204)))), ((int)(((byte)(245)))));
            this.btnGuardar.Location = new System.Drawing.Point(245, 94);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(213, 47);
            this.btnGuardar.TabIndex = 17;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(35, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 25);
            this.label1.TabIndex = 16;
            this.label1.Text = "Texto del asunto";
            // 
            // txtTexto
            // 
            this.txtTexto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(204)))), ((int)(((byte)(245)))));
            this.txtTexto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTexto.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.txtTexto.ForeColor = System.Drawing.Color.White;
            this.txtTexto.Location = new System.Drawing.Point(200, 29);
            this.txtTexto.Name = "txtTexto";
            this.txtTexto.Size = new System.Drawing.Size(429, 26);
            this.txtTexto.TabIndex = 15;
            this.txtTexto.TextChanged += new System.EventHandler(this.txtTexto_TextChanged);
            // 
            // clear1
            // 
            this.clear1.AutoSize = true;
            this.clear1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clear1.ForeColor = System.Drawing.Color.White;
            this.clear1.Location = new System.Drawing.Point(635, 30);
            this.clear1.Name = "clear1";
            this.clear1.Size = new System.Drawing.Size(25, 24);
            this.clear1.TabIndex = 19;
            this.clear1.Text = "X";
            this.clear1.Visible = false;
            this.clear1.Click += new System.EventHandler(this.clear1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(40, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(620, 1);
            this.panel1.TabIndex = 21;
            // 
            // lblAsuntoVacio
            // 
            this.lblAsuntoVacio.AutoSize = true;
            this.lblAsuntoVacio.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsuntoVacio.ForeColor = System.Drawing.Color.Red;
            this.lblAsuntoVacio.Location = new System.Drawing.Point(416, 61);
            this.lblAsuntoVacio.Name = "lblAsuntoVacio";
            this.lblAsuntoVacio.Size = new System.Drawing.Size(242, 21);
            this.lblAsuntoVacio.TabIndex = 24;
            this.lblAsuntoVacio.Text = "Debe ingresar un asunto no vacío";
            this.lblAsuntoVacio.Visible = false;
            // 
            // asunto_new_update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(204)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(701, 153);
            this.Controls.Add(this.lblAsuntoVacio);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.clear1);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTexto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "asunto_new_update";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nuevo asunto";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTexto;
        private System.Windows.Forms.Label clear1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAsuntoVacio;
    }
}