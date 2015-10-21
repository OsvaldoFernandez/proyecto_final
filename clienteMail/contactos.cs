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
            agregar_eventos();
            formAnterior = llamadoDesde;
            btnAceptar.Visible = llamadoDesde != "home";
            form_padre = formulario_padre;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.handlePaginacion();
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataContactos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount < 1) this.Close();
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var form = new frmContacto(0, this);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK) this.actualizarContactos();
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
            if(selectedRowCount != 1) {
                var form2 = new frmAlert(this, "Seleccionar contacto", "Debe seleccionar un contacto para modificar", "close");
                form2.Show();
                return;
            }
            int id = Convert.ToInt32(this.dataContactos.SelectedRows[0].Cells[3].Value);
            var form = new frmContacto(id, this);
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK) this.actualizarContactos();
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
            var form3 = new frmAlert(this, "Eliminar", "¿Está seguro que desea eliminar el contacto?", "yesno");
            form3.Show();
        }

        private void handlePaginacion() //se llama siempre que cambia la variable pagActual
        {
            int cantPaginas = ((int) G.user.contactos().Length + 7) / 8;
            lblPagina.Text = "Página " + pagActual.ToString() + " de " + cantPaginas.ToString();
            btnAnterior.Enabled = pagActual != 1;
            btnSiguiente.Enabled = (pagActual != cantPaginas) && (cantPaginas != 0);
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
            if (formAnterior != "home") this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        // METODOS PARA LA VISTA. OSVALDO

        private void renderView()
        {
            //clear labels 
            int i;
            for (i = 1; i <= 8; i++)
            {
                string n = i.ToString();
                Control container = this.Controls["panel" + n];
                container.Controls["contacto" + n].Text = "";
                container.Controls["contactoNombre" + n].Text = "";
                container.Controls["pictureBox" + n].Hide();
                container.Controls["index" + n].Hide();
            }
            resetPanels();
            //rewrite labels
            i = 1;
            foreach (Contacto con in G.user.contactosPag(pagActual))
            {
                string n = (i ++).ToString();
                Control container = this.Controls["panel" + n];
                container.Controls["contacto" + n].Text = con.Mail;
                container.Controls["contactoNombre" + n].Text = con.Nombre + " " + con.Apellido;
                container.Controls["pictureBox" + n].Show();
                container.Controls["index" + n].Show();
            }
        }

        private void resetPanels()
        {
            bool oscuro = true;
            foreach (Panel panel in new Panel[] {panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8}) {
              panel.BackColor = oscuro ? Color.FromArgb(241, 255, 255) : Color.White;
              oscuro = !oscuro;
            }
            for (int i = 0; i <= (dataContactos.RowCount - 2); i++) dataContactos.Rows[i].Selected = false;
        }

        private void seleccionar_contacto (int contacto) {
          resetPanels();
          if (Controls["index" + contacto.ToString()].Visible) {
            Controls["panel" + contacto.ToString()].BackColor = varcolor;
            dataContactos.Rows[contacto - 1].Selected = true;
          }
        }

        public override void manejar_comando(string comando)
        {
            switch (comando)
            {
                case "uno": case "dos": case "tres": case "cuatro": case "cinco": case "seis": case "siete": case "ocho":
                    seleccionar_contacto(1 + Array.IndexOf<string>(
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
          string[] nombres_controles = {"panel", "contactoNombre", "index", "contacto", "pictureBox"};
          for (int i = 1; i <= 8; i ++) {
            int k = i;
            foreach (string nombre in nombres_controles)
              Controls[nombre + i.ToString()].Click += (object sender, EventArgs e) => seleccionar_contacto(k);
          }
        }
    }
}
