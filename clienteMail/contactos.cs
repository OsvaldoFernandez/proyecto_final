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
    public partial class contactos : RichForm
    {
        int pagActual = 1;
        string formAnterior;
        public int idSelected { get; set; }
        Color varcolor = Color.FromArgb(174, 225, 242);

        public contactos(string llamadoDesde, RichForm formulario_padre)
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
            form_padre = formulario_padre;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.handlePaginacion();
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {

            Int32 selectedRowCount = dataContactos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                //no seleccionó a nadie.
                this.Close();
            }

            this.idSelected = Convert.ToInt32(this.dataContactos.SelectedRows[0].Cells[3].Value);

            form_padre.agregar_contacto(this.idSelected);

            this.Close();
        }

        public override void manejar_aceptar(string contexto)
        {
            if (contexto == "Eliminar")
            {
                int id = Convert.ToInt32(this.dataContactos.SelectedRows[0].Cells[3].Value);
                G.user.eliminar_contacto(id);
                this.actualizarContactos();
            }

        }

        public override void manejar_cerrar(string contexto)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var form = new frmContacto(0, this);
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



        private void btnModificar_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dataContactos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if(selectedRowCount != 1){
                var form2 = new frmAlert(this, "Seleccionar contacto", "Debe seleccionar un contacto para modificar", "close");
                form2.Show();
                return;
            }
            int id = Convert.ToInt32(this.dataContactos.SelectedRows[0].Cells[3].Value);
            var form = new frmContacto(id, this);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK)
            {
                this.actualizarContactos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dataContactos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                var form = new frmAlert(this, "Seleccionar contacto", "Debe seleccionar un contacto para eliminar", "close");
                form.Show();
                return;
            }
            int id = Convert.ToInt32(this.dataContactos.SelectedRows[0].Cells[3].Value);
            var form3 = new frmAlert(this, "Eliminar", "Está seguro que desea eliminar el contacto?", "yesno");
            form3.Show();
        }

        private void handlePaginacion() //se llama siempre que cambia la variable pagActual
        {
            int cant = (int) G.user.contactos().Length;
            int cantPaginas = cant/8;

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
            
            if (pagActual == cantPaginas || cant == 0)
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

        // METODOS PARA LA VISTA. OSVALDO

        private void renderView()
        {
            //clear labels 
            int i = 1;
            for (i = 1; i <= 8; i++)
            {
                string labelName = "contacto" + i.ToString();
                string label2Name = "contactoNombre" + i.ToString();
                string containerName = "panel" + i.ToString();
                string indexName = "pictureBox" + i.ToString();
                string labelIndexName = "index" + i.ToString();
                Control container = this.Controls[containerName];
                Control ctn = container.Controls[labelName];
                Control ctn2 = container.Controls[label2Name];
                Control ctn3 = container.Controls[indexName];
                Control ctn4 = container.Controls[labelIndexName];
                ctn.Text = "";
                ctn2.Text = "";
                ctn3.Hide();
                ctn4.Hide();
            }
            resetPanels();
            //rewrite labels
            i = 1;
            foreach (Contacto con in G.user.contactosPag(pagActual))
            {
                string labelName = "contacto" + i.ToString();
                string label2Name = "contactoNombre" + i.ToString();
                string containerName = "panel" + i.ToString();
                string indexName = "pictureBox" + i.ToString();
                string labelIndexName = "index" + i.ToString();
                Control container = this.Controls[containerName];
                Control ctn = container.Controls[labelName];
                Control ctn2 = container.Controls[label2Name];
                Control ctn3 = container.Controls[indexName];
                Control ctn4 = container.Controls[labelIndexName];
                ctn.Text = con.Mail;
                ctn2.Text = con.Nombre + " " + con.Apellido;
                ctn3.Show();
                ctn4.Show();
                i++;
            }
        }

        private void resetPanels()
        {
            panel1.BackColor = Color.FromArgb(241, 255, 255);
            panel2.BackColor = Color.White;
            panel3.BackColor = Color.FromArgb(241, 255, 255);
            panel4.BackColor = Color.White;
            panel5.BackColor = Color.FromArgb(241, 255, 255);
            panel6.BackColor = Color.White;
            panel7.BackColor = Color.FromArgb(241, 255, 255);
            panel8.BackColor = Color.White;
            int i = 0;
            for (i = 0; i <= (dataContactos.RowCount - 2); i++)
            {
                dataContactos.Rows[i].Selected = false;
            }
        }

        private void seleccionarContacto1(object sender, EventArgs e)
        {
            resetPanels();
            if (index1.Visible)
            {
                panel1.BackColor = varcolor;
                dataContactos.Rows[0].Selected = true;
            }
        }

        private void seleccionarContacto2(object sender, EventArgs e)
        {
            resetPanels();
            if (index2.Visible)
            {
                panel2.BackColor = varcolor;
                dataContactos.Rows[1].Selected = true;
            }
        }

        private void seleccionarContacto3(object sender, EventArgs e)
        {
            resetPanels();
            if (index3.Visible)
            {
                panel3.BackColor = varcolor;
                dataContactos.Rows[2].Selected = true;
            }
        }

        private void seleccionarContacto4(object sender, EventArgs e)
        {
            resetPanels();
            if (index4.Visible)
            {
                panel4.BackColor = varcolor;
                dataContactos.Rows[3].Selected = true;
            }
        }

        private void seleccionarContacto5(object sender, EventArgs e)
        {
            resetPanels();
            if (index5.Visible)
            {
                panel5.BackColor =  varcolor;
                dataContactos.Rows[4].Selected = true;
            }
        }

        private void seleccionarContacto6(object sender, EventArgs e)
        {
            resetPanels();
            if (index6.Visible)
            {
                panel6.BackColor = varcolor;
                dataContactos.Rows[5].Selected = true;
            }
        }

        private void seleccionarContacto7(object sender, EventArgs e)
        {
            resetPanels();
            if (index7.Visible)
            {
                panel7.BackColor = varcolor;
                dataContactos.Rows[6].Selected = true;
            }
        }

        private void seleccionarContacto8(object sender, EventArgs e)
        {
            resetPanels();
            if (index8.Visible)
            {
                panel8.BackColor = varcolor;
                dataContactos.Rows[7].Selected = true;
            }
        }

        public override void manejar_comando(string comando)
        {

            switch (comando)
            {
                case "uno":
                    seleccionarContacto1(null, null);
                    break;
                case "dos":
                    seleccionarContacto2(null, null);
                    break;
                case "tres":
                    seleccionarContacto3(null, null);
                    break;
                case "cuatro":
                    seleccionarContacto4(null, null);
                    break;
                case "cinco":
                    seleccionarContacto5(null, null);
                    break;
                case "seis":
                    seleccionarContacto6(null, null);
                    break;
                case "siete":
                    seleccionarContacto7(null, null);
                    break;
                case "ocho":
                    seleccionarContacto8(null, null);
                    break;
                case "cerrar":
                    btnVolver_Click(null, null);
                    break;
                case "aceptar":
                    btnAceptar_Click(null, null);
                    break;
                case "eliminar":
                    btnEliminar_Click(null, null);
                    break;
                case "anterior":
                    if (btnAnterior.Enabled)
                    {
                        btnAnterior_Click(null, null);
                    }
                    break;
                case "siguiente":
                    if (btnSiguiente.Enabled)
                    {
                        btnSiguiente_Click(null, null);
                    }
                    break;
                default:
                    break;
            }
        }


    }
}
