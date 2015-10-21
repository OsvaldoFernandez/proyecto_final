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
    public partial class mensajes : RichForm
    {
        int pagActual = 1;
        string formAnterior;
        public int idSelected { get; set; }
        Color varcolor = Color.FromArgb(174, 225, 242);

        public mensajes(string llamadoDesde, RichForm formulario_padre)
        {
            InitializeComponent();
            formAnterior = llamadoDesde;
            btnAceptar.Visible = llamadoDesde != "home";
            form_padre = formulario_padre;
        }

        private void mensajes_Load(object sender, EventArgs e)
        {
           this.handlePaginacion();
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataMensajes.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
                //no seleccionó nada.
                this.Close();

            this.idSelected = Convert.ToInt32(this.dataMensajes.SelectedRows[0].Cells[2].Value);
            form_padre.agregar_mensaje(this.idSelected);

            this.Close();
        }

        public override void manejar_aceptar(string contexto)
        {
            if (contexto == "Eliminar")
            {
                int id = Convert.ToInt32(this.dataMensajes.SelectedRows[0].Cells[2].Value);
                G.user.eliminar_mensaje(id);
                this.actualizarMensajes();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var form = new mensaje_new_update(0, this);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK) this.actualizarMensajes();
        }

        private void actualizarMensajes()
        {
            this.dataMensajes.Rows.Clear();
            int i = 1;
            foreach (Mensaje mensaje in G.user.mensajesPag(pagActual))
            {
                this.dataMensajes.Rows.Add(i, mensaje.Texto, mensaje.ID);
                i++;
            }
            renderView();
        }



        private void btnModificar_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dataMensajes.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount < 1)
            {
                var form2 = new frmAlert(this, "Seleccionar mensaje", "Debe seleccionar un mensaje para eliminar", "close");
                form2.Show();
                return;
            }
            int id = Convert.ToInt32(this.dataMensajes.SelectedRows[0].Cells[2].Value);
            var form = new mensaje_new_update(id, this);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK) this.actualizarMensajes();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dataMensajes.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount < 1)
            {
                var form = new frmAlert(this, "Seleccionar mensaje", "Debe seleccionar un mensaje para eliminar", "close");
                form.Show(this);
                return;
            }
            int id = Convert.ToInt32(this.dataMensajes.SelectedRows[0].Cells[2].Value);
            var form3 = new frmAlert(this, "Eliminar", "¿Está seguro que desea eliminar el contacto?", "yesno");
            form3.Show();
        }

        private void handlePaginacion() //se llama siempre que cambia la variable pagActual
        {
            int cant = (int)G.user.mensajes().Length;
            int cantPaginas = cant/ 8;

            if (G.user.mensajes().Length % 8 > 0 || cantPaginas == 0)
            {
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
            this.actualizarMensajes();
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
                string labelName = "mensaje" + i.ToString();
                string containerName = "panel" + i.ToString();
                string indexName = "pictureBox" + i.ToString();
                string labelIndexName = "index" + i.ToString();
                Control container = this.Controls[containerName];
                Control ctn = container.Controls[labelName];
                Control ctn3 = container.Controls[indexName];
                Control ctn4 = container.Controls[labelIndexName];
                ctn.Text = "";
                ctn3.Hide();
                ctn4.Hide();
            }
            resetPanels();
            //rewrite labels
            i = 1;
            foreach (Mensaje mensaje in G.user.mensajesPag(pagActual))
            {
                string labelName = "mensaje" + i.ToString();
                string containerName = "panel" + i.ToString();
                string indexName = "pictureBox" + i.ToString();
                string labelIndexName = "index" + i.ToString();
                Control container = this.Controls[containerName];
                Control ctn = container.Controls[labelName];
                Control ctn3 = container.Controls[indexName];
                Control ctn4 = container.Controls[labelIndexName];
                ctn.Text = mensaje.Texto;
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
            for (i = 0; i <= (dataMensajes.RowCount - 2); i++)
            {
                dataMensajes.Rows[i].Selected = false;
            }
        }

        private void seleccionarMensaje1(object sender, EventArgs e)
        {
            resetPanels();
            if (index1.Visible)
            {
                panel1.BackColor = varcolor;
                dataMensajes.Rows[0].Selected = true;
            }
        }

        private void seleccionarMensaje2(object sender, EventArgs e)
        {
            resetPanels();
            if (index2.Visible)
            {
                panel2.BackColor = varcolor;
                dataMensajes.Rows[1].Selected = true;
            }
        }

        private void seleccionarMensaje3(object sender, EventArgs e)
        {
            resetPanels();
            if (index3.Visible)
            {
                panel3.BackColor = varcolor;
                dataMensajes.Rows[2].Selected = true;
            }
        }

        private void seleccionarMensaje4(object sender, EventArgs e)
        {
            resetPanels();
            if (index4.Visible)
            {
                panel4.BackColor = varcolor;
                dataMensajes.Rows[3].Selected = true;
            }
        }

        private void seleccionarMensaje5(object sender, EventArgs e)
        {
            resetPanels();
            if (index5.Visible)
            {
                panel5.BackColor = varcolor;
                dataMensajes.Rows[4].Selected = true;
            }
        }

        private void seleccionarMensaje6(object sender, EventArgs e)
        {
            resetPanels();
            if (index6.Visible)
            {
                panel6.BackColor = varcolor;
                dataMensajes.Rows[5].Selected = true;
            }
        }

        private void seleccionarMensaje7(object sender, EventArgs e)
        {
            resetPanels();
            if (index7.Visible)
            {
                panel7.BackColor = varcolor;
                dataMensajes.Rows[6].Selected = true;
            }
        }

        private void seleccionarMensaje8(object sender, EventArgs e)
        {
            resetPanels();
            if (index8.Visible)
            {
                panel8.BackColor = varcolor;
                dataMensajes.Rows[7].Selected = true;
            }
        }

        public override void manejar_comando(string comando)
        {

            switch (comando)
            {
                case "uno":
                    seleccionarMensaje1(null, null);
                    break;
                case "dos":
                    seleccionarMensaje2(null, null);
                    break;
                case "tres":
                    seleccionarMensaje3(null, null);
                    break;
                case "cuatro":
                    seleccionarMensaje4(null, null);
                    break;
                case "cinco":
                    seleccionarMensaje5(null, null);
                    break;
                case "seis":
                    seleccionarMensaje6(null, null);
                    break;
                case "siete":
                    seleccionarMensaje7(null, null);
                    break;
                case "ocho":
                    seleccionarMensaje8(null, null);
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
