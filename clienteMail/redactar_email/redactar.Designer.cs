namespace clienteMail.redactar_email
{
    partial class redactar
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
            this.toTxt = new System.Windows.Forms.TextBox();
            this.asuntoTxt = new System.Windows.Forms.TextBox();
            this.adjTxt = new System.Windows.Forms.TextBox();
            this.adjBtn = new System.Windows.Forms.Button();
            this.cuerpoTxt = new System.Windows.Forms.TextBox();
            this.enviarBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // toTxt
            // 
            this.toTxt.Location = new System.Drawing.Point(81, 59);
            this.toTxt.Name = "toTxt";
            this.toTxt.Size = new System.Drawing.Size(257, 20);
            this.toTxt.TabIndex = 0;
            // 
            // asuntoTxt
            // 
            this.asuntoTxt.Location = new System.Drawing.Point(81, 97);
            this.asuntoTxt.Name = "asuntoTxt";
            this.asuntoTxt.Size = new System.Drawing.Size(257, 20);
            this.asuntoTxt.TabIndex = 1;
            // 
            // adjTxt
            // 
            this.adjTxt.Location = new System.Drawing.Point(81, 137);
            this.adjTxt.Name = "adjTxt";
            this.adjTxt.Size = new System.Drawing.Size(164, 20);
            this.adjTxt.TabIndex = 2;
            // 
            // adjBtn
            // 
            this.adjBtn.Location = new System.Drawing.Point(263, 137);
            this.adjBtn.Name = "adjBtn";
            this.adjBtn.Size = new System.Drawing.Size(75, 23);
            this.adjBtn.TabIndex = 3;
            this.adjBtn.Text = "Examinar";
            this.adjBtn.UseVisualStyleBackColor = true;
            this.adjBtn.Click += new System.EventHandler(this.adjBtn_Click_1);
            // 
            // cuerpoTxt
            // 
            this.cuerpoTxt.Location = new System.Drawing.Point(81, 174);
            this.cuerpoTxt.Multiline = true;
            this.cuerpoTxt.Name = "cuerpoTxt";
            this.cuerpoTxt.Size = new System.Drawing.Size(257, 130);
            this.cuerpoTxt.TabIndex = 4;
            // 
            // enviarBtn
            // 
            this.enviarBtn.Location = new System.Drawing.Point(81, 319);
            this.enviarBtn.Name = "enviarBtn";
            this.enviarBtn.Size = new System.Drawing.Size(257, 23);
            this.enviarBtn.TabIndex = 5;
            this.enviarBtn.Text = "Enviar";
            this.enviarBtn.UseVisualStyleBackColor = true;
            this.enviarBtn.Click += new System.EventHandler(this.enviarBtn_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Para";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Asunto";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Adjuntar";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // redactar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 409);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.enviarBtn);
            this.Controls.Add(this.cuerpoTxt);
            this.Controls.Add(this.adjBtn);
            this.Controls.Add(this.adjTxt);
            this.Controls.Add(this.asuntoTxt);
            this.Controls.Add(this.toTxt);
            this.Name = "redactar";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox toTxt;
        private System.Windows.Forms.TextBox asuntoTxt;
        private System.Windows.Forms.TextBox adjTxt;
        private System.Windows.Forms.Button adjBtn;
        private System.Windows.Forms.TextBox cuerpoTxt;
        private System.Windows.Forms.Button enviarBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}