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
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(25, 13);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(288, 438);
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
            // comando
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 522);
            this.Controls.Add(this.disableBtn);
            this.Controls.Add(this.enableBtn);
            this.Controls.Add(this.richTextBox1);
            this.Name = "comando";
            this.Text = "Reconocmiento";
            this.Load += new System.EventHandler(this.comando_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button enableBtn;
        private System.Windows.Forms.Button disableBtn;
    }
}