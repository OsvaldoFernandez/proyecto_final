namespace clienteMail
{
    partial class Form1
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
            this.dataMails = new System.Windows.Forms.DataGridView();
            this.Nro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.De = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Asunto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRecibidos = new System.Windows.Forms.Button();
            this.btnEnviados = new System.Windows.Forms.Button();
            this.redactar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataMails)).BeginInit();
            this.SuspendLayout();
            // 
            // dataMails
            // 
            this.dataMails.AllowUserToAddRows = false;
            this.dataMails.AllowUserToDeleteRows = false;
            this.dataMails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataMails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nro,
            this.index,
            this.De,
            this.Asunto,
            this.Fecha});
            this.dataMails.Location = new System.Drawing.Point(14, 58);
            this.dataMails.Name = "dataMails";
            this.dataMails.ReadOnly = true;
            this.dataMails.Size = new System.Drawing.Size(622, 225);
            this.dataMails.TabIndex = 1;
            this.dataMails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataMails_CellContentClick);
            // 
            // Nro
            // 
            this.Nro.Frozen = true;
            this.Nro.HeaderText = "Nº";
            this.Nro.Name = "Nro";
            this.Nro.ReadOnly = true;
            this.Nro.Width = 30;
            // 
            // index
            // 
            this.index.Frozen = true;
            this.index.HeaderText = "index";
            this.index.Name = "index";
            this.index.ReadOnly = true;
            this.index.Visible = false;
            // 
            // De
            // 
            this.De.Frozen = true;
            this.De.HeaderText = "De";
            this.De.Name = "De";
            this.De.ReadOnly = true;
            this.De.Width = 120;
            // 
            // Asunto
            // 
            this.Asunto.Frozen = true;
            this.Asunto.HeaderText = "Asunto";
            this.Asunto.Name = "Asunto";
            this.Asunto.ReadOnly = true;
            this.Asunto.Width = 300;
            // 
            // Fecha
            // 
            this.Fecha.Frozen = true;
            this.Fecha.HeaderText = "Fecha";
            this.Fecha.Name = "Fecha";
            this.Fecha.ReadOnly = true;
            this.Fecha.Width = 150;
            // 
            // btnRecibidos
            // 
            this.btnRecibidos.Location = new System.Drawing.Point(14, 12);
            this.btnRecibidos.Name = "btnRecibidos";
            this.btnRecibidos.Size = new System.Drawing.Size(75, 23);
            this.btnRecibidos.TabIndex = 2;
            this.btnRecibidos.Text = "Recibidos";
            this.btnRecibidos.UseVisualStyleBackColor = true;
            this.btnRecibidos.Click += new System.EventHandler(this.btnRecibidos_Click);
            // 
            // btnEnviados
            // 
            this.btnEnviados.Location = new System.Drawing.Point(104, 12);
            this.btnEnviados.Name = "btnEnviados";
            this.btnEnviados.Size = new System.Drawing.Size(75, 23);
            this.btnEnviados.TabIndex = 3;
            this.btnEnviados.Text = "Enviados";
            this.btnEnviados.UseVisualStyleBackColor = true;
            this.btnEnviados.Click += new System.EventHandler(this.btnEnviados_Click);
            // 
            // redactar
            // 
            this.redactar.Location = new System.Drawing.Point(196, 12);
            this.redactar.Name = "redactar";
            this.redactar.Size = new System.Drawing.Size(75, 23);
            this.redactar.TabIndex = 4;
            this.redactar.Text = "Redactar";
            this.redactar.UseVisualStyleBackColor = true;
            this.redactar.Click += new System.EventHandler(this.redactar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 358);
            this.Controls.Add(this.redactar);
            this.Controls.Add(this.btnEnviados);
            this.Controls.Add(this.btnRecibidos);
            this.Controls.Add(this.dataMails);
            this.Name = "Form1";
            this.Text = "Cliente mail";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataMails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataMails;
        private System.Windows.Forms.Button btnRecibidos;
        private System.Windows.Forms.Button btnEnviados;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nro;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewTextBoxColumn De;
        private System.Windows.Forms.DataGridViewTextBoxColumn Asunto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fecha;
        private System.Windows.Forms.Button redactar;
    }
}

