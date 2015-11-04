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
        public string res;
        public string contexto;
        public frmAlert(RichForm form_padre_actual, string titulo, string mensaje, string type)
        {
            InitializeComponent();
            form_padre = form_padre_actual;
            contexto = titulo;

            this.Text = titulo;
            lblMensaje.Text = mensaje;
            if (type == "yesno")
            {
                btnAceptar.Show();
                btnCerrar.Text = "Cancelar";
            }
            else
            {
                btnAceptar.Hide();
                btnCerrar.Location = new Point(150, 60);
                btnCerrar.Text = "Cerrar";
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            form_padre.manejar_aceptar(contexto);
            this.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            form_padre.manejar_cerrar(contexto);
            this.Close();
        }

        private void frmAlert_Deactivate(Object sender, EventArgs e)
        {
            autenticacion_mal.Visible = false;
            autenticacion_ok.Visible = false;
        }

        public override void manejar_comando(string comando)
        {

            if (G.confianza_autenticacion > G.sensibilidad_autenticacion)
            {
                autenticacion_ok.Visible = true;
                autenticacion_mal.Visible = false;
            }
            else
            {
                autenticacion_mal.Visible = true;
                autenticacion_ok.Visible = false;
            };

            switch (comando)
            {
                case "aceptar":
                    btnAceptar_Click(null, null);
                    break;
                case "cerrar": case "cancelar":
                    btnCerrar_Click(null, null);
                    break;
            }
        }

        private void frmAlert_Load(object sender, EventArgs e)
        {
            autenticacion_mal.Visible = false;
            autenticacion_ok.Visible = false;
        }
    }
}
