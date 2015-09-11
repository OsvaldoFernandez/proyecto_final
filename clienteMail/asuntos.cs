﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace clienteMail
{
    public partial class asuntos : Form
    {
        int pagActual = 1;
        string formAnterior;
        public string textoSelected { get; set; }
        Color varcolor = Color.FromArgb(255, 255, 224);

        public asuntos(string llamadoDesde)
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

        private void asuntos_Load(object sender, EventArgs e)
        {
           this.handlePaginacion();
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {

            Int32 selectedRowCount = dataAsuntos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                //no seleccionó a nadie.
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.textoSelected = this.dataAsuntos.SelectedRows[0].Cells[1].Value.ToString();

            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("AA");
            var form = new asunto_new_update(0);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK)
            {
                this.actualizarAsuntos();
            }
        }

        private void actualizarAsuntos()
        {
            this.dataAsuntos.Rows.Clear();
            int i = 1;
            foreach (Asunto asunto in G.user.asuntosPag(pagActual))
            {
                this.dataAsuntos.Rows.Add(i, asunto.Texto, asunto.ID);
                i++;
            }
            renderView();
        }



        private void btnModificar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BB");
            Int32 selectedRowCount = dataAsuntos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                MessageBox.Show("Debe seleccionar un asunto a modificar.");
                return;
            }
            int id = Convert.ToInt32(this.dataAsuntos.SelectedRows[0].Cells[2].Value);
            var form = new asunto_new_update(id);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK)
            {
                this.actualizarAsuntos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dataAsuntos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                MessageBox.Show("Debe seleccionar un asunto para eliminar.");
                return;
            }
            int id = Convert.ToInt32(this.dataAsuntos.SelectedRows[0].Cells[2].Value);
            DialogResult rta = MessageBox.Show("Está seguro que desea eliminar el asunto?", "", MessageBoxButtons.YesNo);
            if (rta == DialogResult.Yes)
            {
                G.user.eliminar_asunto(id);
                this.actualizarAsuntos();
            }
        }

        private void handlePaginacion() //se llama siempre que cambia la variable pagActual
        {
            int cantPaginas = (int)G.user.asuntos().Length / 8;

            if (G.user.asuntos().Length % 8 > 0)
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

            if (pagActual == cantPaginas)
            {
                btnSiguiente.Enabled = false;
            }
            else
            {
                btnSiguiente.Enabled = true;
            }
            this.actualizarAsuntos();
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
                string labelName = "asunto" + i.ToString();
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
            foreach (Asunto asunto in G.user.asuntosPag(pagActual))
            {
                string labelName = "asunto" + i.ToString();
                string containerName = "panel" + i.ToString();
                string indexName = "pictureBox" + i.ToString();
                string labelIndexName = "index" + i.ToString();
                Control container = this.Controls[containerName];
                Control ctn = container.Controls[labelName];
                Control ctn3 = container.Controls[indexName];
                Control ctn4 = container.Controls[labelIndexName];
                ctn.Text = asunto.Texto;
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
            for (i = 0; i <= (dataAsuntos.RowCount - 2); i++)
            {
                dataAsuntos.Rows[i].Selected = false;
            }
        }

        private void seleccionarAsunto1(object sender, EventArgs e)
        {
            resetPanels();
            if (index1.Visible)
            {
                panel1.BackColor = varcolor;
                dataAsuntos.Rows[0].Selected = true;
            }
        }

        private void seleccionarAsunto2(object sender, EventArgs e)
        {
            resetPanels();
            if (index2.Visible)
            {
                panel2.BackColor = varcolor;
                dataAsuntos.Rows[1].Selected = true;
            }
        }

        private void seleccionarAsunto3(object sender, EventArgs e)
        {
            resetPanels();
            if (index3.Visible)
            {
                panel3.BackColor = varcolor;
                dataAsuntos.Rows[2].Selected = true;
            }
        }

        private void seleccionarAsunto4(object sender, EventArgs e)
        {
            resetPanels();
            if (index4.Visible)
            {
                panel4.BackColor = varcolor;
                dataAsuntos.Rows[3].Selected = true;
            }
        }

        private void seleccionarAsunto5(object sender, EventArgs e)
        {
            resetPanels();
            if (index5.Visible)
            {
                panel5.BackColor = varcolor;
                dataAsuntos.Rows[4].Selected = true;
            }
        }

        private void seleccionarAsunto6(object sender, EventArgs e)
        {
            resetPanels();
            if (index6.Visible)
            {
                panel6.BackColor = varcolor;
                dataAsuntos.Rows[5].Selected = true;
            }
        }

        private void seleccionarAsunto7(object sender, EventArgs e)
        {
            resetPanels();
            if (index7.Visible)
            {
                panel7.BackColor = varcolor;
                dataAsuntos.Rows[6].Selected = true;
            }
        }

        private void seleccionarAsunto8(object sender, EventArgs e)
        {
            resetPanels();
            if (index8.Visible)
            {
                panel8.BackColor = varcolor;
                dataAsuntos.Rows[7].Selected = true;
            }
        }
        

    }
}