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
    public partial class frmAlert : RichForm
    {
        RichForm form_padre;
        public frmAlert(RichForm form_padre_actual, string titulo, string mensaje, string type)
        {
            InitializeComponent();
            form_padre = form_padre_actual;
            G.formulario_activo = this;

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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            G.formulario_activo = form_padre;
        }

        public override void manejar_comando(string comando)
        {

            switch (comando)
            {
                case "aceptar":
                    btnAceptar_Click(null, null);
                    break;
                case "cerrar":
                    btnCerrar_Click(null, null);
                    break;
                default:
                    break;
            }
        }
    }
}
