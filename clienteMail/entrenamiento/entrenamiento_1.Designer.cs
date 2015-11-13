namespace clienteMail.entrenamiento
{
    partial class entrenamiento_1
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(entrenamiento_1));
      this.lblTitle = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.btnRecibidos = new System.Windows.Forms.Button();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.comando = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.estado = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cafe = new System.Windows.Forms.PictureBox();
      this.errorpanel = new System.Windows.Forms.Panel();
      this.errorlabel = new System.Windows.Forms.Label();
      this.pausaBtn = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cafe)).BeginInit();
      this.errorpanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblTitle
      // 
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
      this.lblTitle.ForeColor = System.Drawing.Color.White;
      this.lblTitle.Location = new System.Drawing.Point(51, 31);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(125, 30);
      this.lblTitle.TabIndex = 10;
      this.lblTitle.Text = "Bienvenido";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
      this.label1.ForeColor = System.Drawing.Color.White;
      this.label1.Location = new System.Drawing.Point(51, 71);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(268, 25);
      this.label1.TabIndex = 11;
      this.label1.Text = "Es hora de entrenar el sistema";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
      this.label2.ForeColor = System.Drawing.Color.White;
      this.label2.Location = new System.Drawing.Point(51, 106);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(554, 25);
      this.label2.TabIndex = 12;
      this.label2.Text = "Mencione en voz alta los comandos que aparecerán en pantalla";
      // 
      // btnRecibidos
      // 
      this.btnRecibidos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(204)))), ((int)(((byte)(245)))));
      this.btnRecibidos.FlatAppearance.BorderSize = 2;
      this.btnRecibidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnRecibidos.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnRecibidos.ForeColor = System.Drawing.Color.White;
      this.btnRecibidos.Location = new System.Drawing.Point(56, 148);
      this.btnRecibidos.Name = "btnRecibidos";
      this.btnRecibidos.Size = new System.Drawing.Size(193, 30);
      this.btnRecibidos.TabIndex = 13;
      this.btnRecibidos.Text = "Estoy listo!";
      this.btnRecibidos.UseVisualStyleBackColor = false;
      this.btnRecibidos.Click += new System.EventHandler(this.btnRecibidos_Click);
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(225)))), ((int)(((byte)(242)))));
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.comando,
            this.estado,
            this.codigo});
      this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(225)))), ((int)(((byte)(242)))));
      this.dataGridView1.Location = new System.Drawing.Point(506, 31);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.Size = new System.Drawing.Size(287, 506);
      this.dataGridView1.TabIndex = 15;
      this.dataGridView1.Visible = false;
      // 
      // comando
      // 
      this.comando.HeaderText = "Comando";
      this.comando.Name = "comando";
      this.comando.ReadOnly = true;
      this.comando.Width = 140;
      // 
      // estado
      // 
      this.estado.HeaderText = "Estado";
      this.estado.Name = "estado";
      this.estado.ReadOnly = true;
      // 
      // codigo
      // 
      this.codigo.HeaderText = "Codigo";
      this.codigo.Name = "codigo";
      this.codigo.ReadOnly = true;
      this.codigo.Visible = false;
      this.codigo.Width = 5;
      // 
      // cafe
      // 
      this.cafe.Image = ((System.Drawing.Image)(resources.GetObject("cafe.Image")));
      this.cafe.Location = new System.Drawing.Point(56, 210);
      this.cafe.Name = "cafe";
      this.cafe.Size = new System.Drawing.Size(380, 327);
      this.cafe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.cafe.TabIndex = 16;
      this.cafe.TabStop = false;
      this.cafe.Visible = false;
      // 
      // errorpanel
      // 
      this.errorpanel.BackColor = System.Drawing.Color.FloralWhite;
      this.errorpanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.errorpanel.Controls.Add(this.errorlabel);
      this.errorpanel.ForeColor = System.Drawing.Color.Maroon;
      this.errorpanel.Location = new System.Drawing.Point(12, 454);
      this.errorpanel.Name = "errorpanel";
      this.errorpanel.Size = new System.Drawing.Size(480, 82);
      this.errorpanel.TabIndex = 17;
      this.errorpanel.Visible = false;
      // 
      // errorlabel
      // 
      this.errorlabel.AutoSize = true;
      this.errorlabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.errorlabel.Location = new System.Drawing.Point(4, 4);
      this.errorlabel.Name = "errorlabel";
      this.errorlabel.Size = new System.Drawing.Size(52, 21);
      this.errorlabel.TabIndex = 0;
      this.errorlabel.Text = "label3";
      this.errorlabel.Visible = false;
      // 
      // pausaBtn
      // 
      this.pausaBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(204)))), ((int)(((byte)(245)))));
      this.pausaBtn.FlatAppearance.BorderSize = 2;
      this.pausaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.pausaBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.pausaBtn.ForeColor = System.Drawing.Color.White;
      this.pausaBtn.Location = new System.Drawing.Point(56, 148);
      this.pausaBtn.Name = "pausaBtn";
      this.pausaBtn.Size = new System.Drawing.Size(193, 30);
      this.pausaBtn.TabIndex = 14;
      this.pausaBtn.Text = "Pausar";
      this.pausaBtn.UseVisualStyleBackColor = false;
      this.pausaBtn.Visible = false;
      this.pausaBtn.Click += new System.EventHandler(this.pausaBtn_Click);
      // 
      // entrenamiento_1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(183)))), ((int)(((byte)(248)))));
      this.ClientSize = new System.Drawing.Size(805, 576);
      this.Controls.Add(this.errorpanel);
      this.Controls.Add(this.cafe);
      this.Controls.Add(this.dataGridView1);
      this.Controls.Add(this.btnRecibidos);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lblTitle);
      this.Controls.Add(this.pausaBtn);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "entrenamiento_1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Entrenamiento";
      this.Load += new System.EventHandler(this.entrenamiento_1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cafe)).EndInit();
      this.errorpanel.ResumeLayout(false);
      this.errorpanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRecibidos;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.PictureBox cafe;
        private System.Windows.Forms.DataGridViewTextBoxColumn comando;
        private System.Windows.Forms.DataGridViewTextBoxColumn estado;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.Panel errorpanel;
        private System.Windows.Forms.Label errorlabel;
        private System.Windows.Forms.Button pausaBtn;

    }
}