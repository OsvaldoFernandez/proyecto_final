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
    public partial class frmAlert : Form
    {
        public frmAlert(string titulo, string mensaje, string type)
        {
            InitializeComponent();
            this.Text = titulo;
            lblMensaje.Text = mensaje;
            if (type == "yesno")
            {
                btnAceptar.Show();
                btnCancelar.Text = "Cancelar";
            }
            else
            {
                btnAceptar.Hide();
                btnCancelar.Text = "Cerrar";
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnAsunto_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }
    }
}
