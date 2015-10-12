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
    public partial class mensaje_new_update : RichForm
    {
        private int ID = 0;

        public mensaje_new_update(int id, RichForm form_padre_actual)
        {
            InitializeComponent();
            form_padre = form_padre_actual;

            if (id > 0)
            {
                Mensaje mensaje = G.user.getMensaje(id);
                txtTexto.Text = mensaje.Texto;

                this.Text = "Modificar mensaje";
            }
            ID = id;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtTexto.Text.Length == 0)
            {
                lblMensajeVacio.Visible = true;
                return;
            }
            lblMensajeVacio.Visible = false;
            Mensaje mensaje = new Mensaje();
            mensaje.__texto = txtTexto.Text;

            if (ID == 0) //agregue
            {
                G.user.agregar_mensaje(mensaje);
            }
            else //modifique
            {
                mensaje.__id = ID;
                G.user.modificar_mensaje(mensaje);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
        }

    }
}
