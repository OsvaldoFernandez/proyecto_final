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
    public partial class frmContacto : Form
    {
        private int ID = 0;
        public frmContacto(int id)
        {
            InitializeComponent();

            if (id > 0)
            {
                Contacto contacto = G.user.getContacto(id);
                txtNombre.Text = contacto.Nombre;
                txtApellido.Text = contacto.Apellido;
                txtMail.Text = contacto.Mail;
            }
            ID = id;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //validar.
            Contacto contacto = new Contacto();
            contacto.__nombre = txtNombre.Text;
            contacto.__apellido = txtApellido.Text;
            contacto.__mail = txtMail.Text;

            if (ID == 0) //agregue
            {
                G.user.agregar_contacto(contacto);
            }
            else //modifique
            {
                contacto.__id = ID;
                G.user.modificar_contacto(contacto);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
