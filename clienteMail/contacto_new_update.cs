using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace clienteMail
{

    public partial class frmContacto : RichForm
    {
        public string contexto;
        private int ID = 0;
        public frmContacto(int id, RichForm form_padre_actual)
        {
            InitializeComponent();
            form_padre = form_padre_actual;
            contexto = "Agregar contacto";
            if (id > 0)
            {
                Contacto contacto = G.user.getContacto(id);
                txtNombre.Text = contacto.Nombre;
                txtApellido.Text = contacto.Apellido;
                txtMail.Text = contacto.Mail;

                contexto = "Modificar contacto";
                this.Text = contexto;
            }
            ID = id;
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            clear1.Visible = txtNombre.Text != "";
        }

        private void clear1_Click(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            clear1.Visible = false;
        }

        private void txtApellido_TextChanged(object sender, EventArgs e)
        {
            clear2.Visible = txtApellido.Text != "";
        }

        private void clear2_Click(object sender, EventArgs e)
        {
            txtApellido.Text = "";
            clear2.Visible = false;
        }

        private void txtMail_TextChanged(object sender, EventArgs e)
        {
            clear3.Visible = txtMail.Text != "";
        }

        private void clear3_Click(object sender, EventArgs e)
        {
            txtMail.Text = "";
            clear3.Visible = false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // correcciones hechas sobre expresion regular, no permitia todos los casos
            // cualquier direccion con caracteres por encima de U+007F no es aceptada (usar Punycode (RFC 3492))
            Regex reg = new Regex(@"^[^ /?@\x00-\x1f()<>]+@([^. /?@\x00-\x1f()<>]+\.)*[a-zA-Z]{2,}\.?$");
            if (!reg.IsMatch(txtMail.Text))
            {
                panel3.BackColor = Color.Red;
                lblMailIncorrecto.Visible = true;
                return;
            }
            panel3.BackColor = Color.White;
            lblMailIncorrecto.Visible = false;

            Contacto contacto = new Contacto();
            contacto.__nombre = txtNombre.Text;
            contacto.__apellido = txtApellido.Text;
            contacto.__mail = txtMail.Text;

            if (ID == 0) //agregue
                G.user.agregar_contacto(contacto);
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