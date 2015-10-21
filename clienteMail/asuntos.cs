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
    public partial class asuntos : RichForm
    {
        int pagActual = 1;
        string formAnterior;
        public int idSelected { get; set; }
        Color varcolor = Color.FromArgb(174, 225, 242);

        public asuntos(string llamadoDesde, RichForm formulario_padre)
        {
            InitializeComponent();
            agregar_eventos();
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

            Int32 selectedRowCount = dataAsuntos.Rows.GetRowCount(DataGridViewElementStates.Selected);
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
            var form3 = new frmAlert(this, "Eliminar", "Está seguro que desea eliminar el asunto?", "yesno");
            form3.Show();
        }

        private void handlePaginacion() //se llama siempre que cambia la variable pagActual
        {
            int cantPaginas = ((int) G.user.asuntos().Length + 7) / 8;
            lblPagina.Text = "Página " + pagActual.ToString() + " de " + cantPaginas.ToString();
            btnAnterior.Enabled = pagActual != 1;
            btnSiguiente.Enabled = (pagActual != cantPaginas) && (cantPaginas != 0);
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

        private void resetPanels()
        {
            bool oscuro = true;
            foreach (Panel panel in (new Panel[] {panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8})) {
                panel.BackColor = oscuro ? Color.FromArgb(241, 255, 255) : Color.White;
                oscuro = !oscuro;
            }
            for (int i = 0; i <= (dataAsuntos.RowCount - 2); i++)
                dataAsuntos.Rows[i].Selected = false;
        }

        private void seleccionar_asunto (int asunto) {
          resetPanels();
          if (Controls["index" + asunto.ToString()].Visible) {
            Controls["panel" + asunto.ToString()].BackColor = varcolor;
            dataAsuntos.Rows[asunto - 1].Selected = true;
          }
        }

        public override void manejar_comando(string comando)
        {
            switch (comando)
            {
                case "uno": case "dos": case "tres": case "cuatro":
                case "cinco": case "seis": case "siete": case "ocho":
                    seleccionar_asunto(1 + Array.IndexOf<string> (
                      new string[] {"uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho"},
                      comando
                    ));
                    break;
                case "cerrar":
                    btnVolver_Click(null, EventArgs.Empty);
                    break;
                case "aceptar":
                    btnAceptar_Click(null, EventArgs.Empty);
                    break;
                case "eliminar":
                    btnEliminar_Click(null, EventArgs.Empty);
                    break;
                case "anterior":
                    if (btnAnterior.Enabled) btnAnterior_Click(null, EventArgs.Empty);
                    break;
                case "siguiente":
                    if (btnSiguiente.Enabled) btnSiguiente_Click(null, EventArgs.Empty);
                    break;
            }
        }
        
        private void agregar_eventos () {
          string[] nombres = {"panel", "index", "asunto", "pictureBox"};
          for (int n = 1; n <= 8; n ++) {
            int k = n;
            foreach (string nombre in nombres)
              Controls[nombre + n.ToString()].Click += (object sender, EventArgs e) => seleccionar_asunto(k);
          }
        }
    }
}
