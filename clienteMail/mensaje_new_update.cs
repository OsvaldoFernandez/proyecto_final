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
    public partial class mensaje_new_update : Form
    {
        private int ID = 0;
        public mensaje_new_update(int id)
        {
            InitializeComponent();

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
            //validar.
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

    }
}
