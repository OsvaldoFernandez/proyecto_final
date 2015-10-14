namespace clienteMail.comando
{
    partial class comando
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.enableBtn = new System.Windows.Forms.Button();
            this.disableBtn = new System.Windows.Forms.Button();
            this.txtFormActivo = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSensibilidad = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnActualizarSensibilidad = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(25, 21);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(234, 414);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "- Log - ";
            // 
            // enableBtn
            // 
            this.enableBtn.Location = new System.Drawing.Point(25, 469);
            this.enableBtn.Name = "enableBtn";
            this.enableBtn.Size = new System.Drawing.Size(140, 23);
            this.enableBtn.TabIndex = 1;
            this.enableBtn.Text = "Habilitar voz";
            this.enableBtn.UseVisualStyleBackColor = true;
            this.enableBtn.Click += new System.EventHandler(this.enableBtn_Click);
            // 
            // disableBtn
            // 
            this.disableBtn.Enabled = false;
            this.disableBtn.Location = new System.Drawing.Point(173, 469);
            this.disableBtn.Name = "disableBtn";
            this.disableBtn.Size = new System.Drawing.Size(140, 23);
            this.disableBtn.TabIndex = 2;
            this.disableBtn.Text = "Deshabilitar voz";
            this.disableBtn.UseVisualStyleBackColor = true;
            this.disableBtn.Click += new System.EventHandler(this.disableBtn_Click);
            // 
            // txtFormActivo
            // 
            this.txtFormActivo.Location = new System.Drawing.Point(282, 22);
            this.txtFormActivo.Name = "txtFormActivo";
            this.txtFormActivo.Size = new System.Drawing.Size(177, 413);
            this.txtFormActivo.TabIndex = 3;
            this.txtFormActivo.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(339, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Form activo";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtSensibilidad
            // 
            this.txtSensibilidad.Location = new System.Drawing.Point(342, 472);
            this.txtSensibilidad.Name = "txtSensibilidad";
            this.txtSensibilidad.Size = new System.Drawing.Size(100, 20);
            this.txtSensibilidad.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(339, 456);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Sensibilidad:";
            // 
            // btnActualizarSensibilidad
            // 
            this.btnActualizarSensibilidad.Location = new System.Drawing.Point(367, 498);
            this.btnActualizarSensibilidad.Name = "btnActualizarSensibilidad";
            this.btnActualizarSensibilidad.Size = new System.Drawing.Size(75, 23);
            this.btnActualizarSensibilidad.TabIndex = 7;
            this.btnActualizarSensibilidad.Text = "Actualizar";
            this.btnActualizarSensibilidad.UseVisualStyleBackColor = true;
            this.btnActualizarSensibilidad.Click += new System.EventHandler(this.btnActualizarSensibilidad_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 498);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(288, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Entrenar el sistema";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comando
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 522);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnActualizarSensibilidad);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSensibilidad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFormActivo);
            this.Controls.Add(this.disableBtn);
            this.Controls.Add(this.enableBtn);
            this.Controls.Add(this.richTextBox1);
            this.Name = "comando";
            this.Text = "Reconocmiento";
            this.Load += new System.EventHandler(this.comando_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button enableBtn;
        private System.Windows.Forms.Button disableBtn;
        private System.Windows.Forms.RichTextBox txtFormActivo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSensibilidad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnActualizarSensibilidad;
        private System.Windows.Forms.Button button1;
    }
}