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
    public partial class asuntos : FormPaginado
    {
        string formAnterior;
        public int idSelected { get; set; }

        public asuntos(string llamadoDesde, RichForm formulario_padre)
        {
            InitializeComponent();
            agregar_eventos(seleccionar_asunto, false, "panel", "index", "pictureBox", "asunto");
            formAnterior = llamadoDesde;
            btnAceptar.Visible = llamadoDesde != "home";
            form_padre = formulario_padre;
        }

        private void asuntos_Load(object sender, EventArgs e)
        {
           this.handlePaginacion();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataAsuntos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
                //no seleccionó a nadie.
                this.Close();

            this.idSelected = Convert.ToInt32(this.dataAsuntos.SelectedRows[0].Cells[2].Value);
            form_padre.agregar_asunto(this.idSelected);

            this.Close();
        }

        public override void manejar_aceptar(string contexto)
        {
            if (contexto == "Eliminar")
            {
                int id = Convert.ToInt32(this.dataAsuntos.SelectedRows[0].Cells[2].Value); ;
                G.user.eliminar_asunto(id);
                this.actualizarAsuntos();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var form = new asunto_new_update(0, this);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK) this.actualizarAsuntos();
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
            Int32 selectedRowCount = dataAsuntos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                var form2 = new frmAlert(this, "Seleccionar asunto", "Debe seleccionar un asunto para modificar", "close");
                form2.Show();
                return;
            }
            int id = Convert.ToInt32(this.dataAsuntos.SelectedRows[0].Cells[2].Value);
            var form = new asunto_new_update(id, this);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK) this.actualizarAsuntos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dataAsuntos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                var form = new frmAlert(this, "Seleccionar asunto", "Debe seleccionar un asunto para eliminar", "close");
                form.Show();
                return;
            }
            int id = Convert.ToInt32(this.dataAsuntos.SelectedRows[0].Cells[2].Value);
            var form3 = new frmAlert(this, "Eliminar", "¿Está seguro que desea eliminar el asunto?", "yesno");
            form3.Show();
        }

        private void handlePaginacion() //se llama siempre que cambia la variable pagActual
        {
            actualizar_pagina(G.user.asuntos().Length, btnAnterior, btnSiguiente, lblPagina);
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
            if (formAnterior != "home") this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        // METODOS PARA LA VISTA. OSVALDO

        private void renderView()
        {
            //clear labels 
            int i = 1;
            for (i = 1; i <= 8; i++)
            {
                string n = i.ToString();
                Control container = this.Controls["panel" + n];
                container.Controls["asunto" + n].Text = "";
                container.Controls["pictureBox" + n].Hide();
                container.Controls["index" + n].Hide();
            }
            resetPanels();
            //rewrite labels
            i = 1;
            foreach (Asunto asunto in G.user.asuntosPag(pagActual))
            {
                string n = (i ++).ToString();
                Control container = this.Controls["panel" + n];
                container.Controls["asunto" + n].Text = asunto.Texto;
                container.Controls["pictureBox" + n].Show();
                container.Controls["index" + n].Show();
            }
        }

        protected override void resetPanels()
        {
            base.resetPanels();
            for (int i = 0; i <= (dataAsuntos.RowCount - 2); i ++) dataAsuntos.Rows[i].Selected = false;
        }

        private void seleccionar_asunto (int asunto) {
          seleccionar_elemento(asunto, "index", "panel", dataAsuntos);
        }

        public override void manejar_comando(string comando)
        {
            manejar_comando_basico(comando, seleccionar_asunto,
              Comando.Evento("cerrar", btnVolver_Click),
              Comando.Evento("aceptar", btnAceptar_Click),
              Comando.Evento("eliminar", btnEliminar_Click),
              new Comando("anterior", () => {if (btnAnterior.Enabled) btnAnterior_Click(null, EventArgs.Empty);}),
              new Comando("siguiente", () => {if (btnSiguiente.Enabled) btnSiguiente_Click(null, EventArgs.Empty);})
            );
        }
    }
}
