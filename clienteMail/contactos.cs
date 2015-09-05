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
        int pagActual = 1;
        string formAnterior;
        public string mailSelected { get; set; } 

        public contactos(string llamadoDesde)
        {
            InitializeComponent();
            formAnterior = llamadoDesde;
            if (llamadoDesde == "home")
            {
                btnAceptar.Visible = false;
            }
            else
            {
                btnAceptar.Visible = true;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.handlePaginacion();
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //ACA PREGUNTO SI HAY ALGUN CONTACTO SELECCIONADO. SINO MUESTRO ERROR
            //HARDCODEO MAIL QUE DEVUELVE

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.mailSelected = "meliguter@gmail.com";
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var form = new frmContacto(0);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK)
            {
                this.actualizarContactos();
            }
        }

        private void actualizarContactos()
        {
            this.dataContactos.Rows.Clear();
            int i = 1;
            foreach (Contacto con in G.user.contactosPag(pagActual))
            {
                this.dataContactos.Rows.Add(i, con.Mail, con.Nombre + " " + con.Apellido, con.ID);
                i++;
            }
            renderView();
        }

        private void renderView()
        {
            int i = 1;
            foreach (Contacto con in G.user.contactosPag(pagActual))
            {
                string labelName = "contacto" + i.ToString();
                string containerName = "panel" + i.ToString();
                Control container = this.Controls[containerName];
                Control ctn = container.Controls[labelName];
                ctn.Text = con.Mail;
                i++;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dataContactos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if(selectedRowCount != 1){
                MessageBox.Show("Debe seleccionar UN contacto a modificar.");   
                return;
            }
            int id = Convert.ToInt32(this.dataContactos.SelectedRows[0].Cells[3].Value);
            var form = new frmContacto(id);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK)
            {
                this.actualizarContactos();
            }
        }

        private void handlePaginacion() //se llama siempre que cambia la variable pagActual
        {
            int cantPaginas = (int) G.user.contactos().Length/8;

            if(G.user.contactos().Length % 8 > 0){
                cantPaginas++;
            }

            lblPagina.Text = "Página " + pagActual.ToString() + " de " + cantPaginas.ToString();
            if (pagActual == 1)
            {
                btnAnterior.Enabled = false;
            }
            else
            {
                btnAnterior.Enabled = true;
            }
            
            if (pagActual == cantPaginas)
            {
                btnSiguiente.Enabled = false;
            }
            else
            {
                btnSiguiente.Enabled = true;
            }
            this.actualizarContactos();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            pagActual--;
            this.handlePaginacion();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            pagActual++;
            this.handlePaginacion();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            if (formAnterior != "home")
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            this.Close();
        }


    }
}
