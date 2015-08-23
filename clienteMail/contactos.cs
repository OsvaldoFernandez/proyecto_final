using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace clienteMail
{
    public partial class contactos : Form
    {
        public contactos()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //se agregan contactos para la presentacion comercial.
            this.dataContactos.Rows.Add(1, "Federico Dopino", "fededopi@gmail.com");
            this.dataContactos.Rows.Add(2, "Osvaldo Fernandez", "osvaldo.fernandez@gmail.com");
            this.dataContactos.Rows.Add(3, "Melanie Guterman", "melanie.guterman@gmail.com");
            this.dataContactos.Rows.Add(4, "Julián Raspanti", "juli_raspanti@hotmail.com");
            this.dataContactos.Rows.Add(5, "Ale Taboada", "alejandro@taboada.com");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
