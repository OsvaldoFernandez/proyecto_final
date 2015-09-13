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
            this.enviarBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnPara = new System.Windows.Forms.Button();
            this.btnAsunto = new System.Windows.Forms.Button();
            this.btnMensaje = new System.Windows.Forms.Button();
            this.cuerpoTxt = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // toTxt
            // 
            this.toTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toTxt.Location = new System.Drawing.Point(156, 57);
            this.toTxt.Name = "toTxt";
            this.toTxt.Size = new System.Drawing.Size(462, 29);
            this.toTxt.TabIndex = 0;
            this.toTxt.TextChanged += new System.EventHandler(this.toTxt_TextChanged);
            // 
            // asuntoTxt
            // 
            this.asuntoTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asuntoTxt.Location = new System.Drawing.Point(156, 95);
            this.asuntoTxt.Margin = new System.Windows.Forms.Padding(10);
            this.asuntoTxt.Name = "asuntoTxt";
            this.asuntoTxt.Size = new System.Drawing.Size(462, 29);
            this.asuntoTxt.TabIndex = 1;
            // 
            // enviarBtn
            // 
            this.enviarBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enviarBtn.Location = new System.Drawing.Point(244, 401);
            this.enviarBtn.Name = "enviarBtn";
            this.enviarBtn.Size = new System.Drawing.Size(124, 49);
            this.enviarBtn.TabIndex = 5;
            this.enviarBtn.Text = "Enviar";
            this.enviarBtn.UseVisualStyleBackColor = true;
            this.enviarBtn.Click += new System.EventHandler(this.enviarBtn_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(87, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 24);
            this.label1.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(240, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Redactar Mail";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(383, 401);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(124, 49);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnPara
            // 
            this.btnPara.Location = new System.Drawing.Point(70, 57);
            this.btnPara.Name = "btnPara";
            this.btnPara.Size = new System.Drawing.Size(75, 23);
            this.btnPara.TabIndex = 11;
            this.btnPara.Text = "Para";
            this.btnPara.UseVisualStyleBackColor = true;
            this.btnPara.Click += new System.EventHandler(this.btnPara_Click);
            // 
            // btnAsunto
            // 
            this.btnAsunto.Location = new System.Drawing.Point(70, 101);
            this.btnAsunto.Name = "btnAsunto";
            this.btnAsunto.Size = new System.Drawing.Size(75, 23);
            this.btnAsunto.TabIndex = 12;
            this.btnAsunto.Text = "Asunto";
            this.btnAsunto.UseVisualStyleBackColor = true;
            this.btnAsunto.Click += new System.EventHandler(this.btnAsunto_Click);
            // 
            // btnMensaje
            // 
            this.btnMensaje.Location = new System.Drawing.Point(70, 152);
            this.btnMensaje.Name = "btnMensaje";
            this.btnMensaje.Size = new System.Drawing.Size(75, 23);
            this.btnMensaje.TabIndex = 13;
            this.btnMensaje.Text = "Mensaje";
            this.btnMensaje.UseVisualStyleBackColor = true;
            this.btnMensaje.Click += new System.EventHandler(this.btnMensaje_Click);
            // 
            // cuerpoTxt
            // 
            this.cuerpoTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cuerpoTxt.Location = new System.Drawing.Point(156, 152);
            this.cuerpoTxt.Name = "cuerpoTxt";
            this.cuerpoTxt.Size = new System.Drawing.Size(462, 227);
            this.cuerpoTxt.TabIndex = 14;
            this.cuerpoTxt.Text = "";
            // 
            // redactar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 482);
            this.Controls.Add(this.cuerpoTxt);
            this.Controls.Add(this.btnMensaje);
            this.Controls.Add(this.btnAsunto);
            this.Controls.Add(this.btnPara);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.enviarBtn);
            this.Controls.Add(this.asuntoTxt);
            this.Controls.Add(this.toTxt);
            this.Name = "redactar";
            this.Text = "Redactar";
            this.Load += new System.EventHandler(this.redactar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox toTxt;
        private System.Windows.Forms.TextBox asuntoTxt;
        private System.Windows.Forms.Button enviarBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnPara;
        private System.Windows.Forms.Button btnAsunto;
        private System.Windows.Forms.Button btnMensaje;
        private System.Windows.Forms.RichTextBox cuerpoTxt;
    }
}