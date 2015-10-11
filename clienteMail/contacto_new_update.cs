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


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (txtNombre.Text == "")
            {
                clear1.Visible = false;
            }
            else
            {
                clear1.Visible = true;
            }
        }

        private void clear1_Click(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            clear1.Visible = false;
        }

        private void txtApellido_TextChanged(object sender, EventArgs e)
        {
            if (txtApellido.Text == "")
            {
                clear2.Visible = false;
            }
            else
            {
                clear2.Visible = true;
            }
        }

        private void clear2_Click(object sender, EventArgs e)
        {
            txtApellido.Text = "";
            clear2.Visible = false;
        }

        private void txtMail_TextChanged(object sender, EventArgs e)
        {
            if (txtMail.Text == "")
            {
                clear3.Visible = false;
            }
            else
            {
                clear3.Visible = true;
            }
        }

        private void clear3_Click(object sender, EventArgs e)
        {
            txtMail.Text = "";
            clear3.Visible = false;
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
        }

        private void frmContacto_Load(object sender, EventArgs e)
        {

        }


    }
}
