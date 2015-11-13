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
    public partial class contactos : FormPaginado
    {
        string formAnterior;
        public int idSelected { get; set; }

        public contactos(string llamadoDesde, RichForm formulario_padre)
        {
            InitializeComponent();
            agregar_eventos(seleccionar_contacto, false, "panel", "contactoNombre", "index", "contacto", "pictureBox");
            formAnterior = llamadoDesde;
            btnAceptar.Visible = llamadoDesde != "home";
            form_padre = formulario_padre;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.handlePaginacion();
            autenticacion_mal.Visible = false;
            autenticacion_ok.Visible = false;
        }

        private void contactos_Deactivate(Object sender, EventArgs e)
        {
            autenticacion_mal.Visible = false;
            autenticacion_ok.Visible = false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataContactos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1 || btnAceptar.Visible == false) return;
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
            handlePaginacion();
        }

        private void actualizarContactos()
        {
            this.dataContactos.Rows.Clear();
            int i = 1;
            foreach (Contacto con in G.user.contactosPag(pagActual)) {
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
            if (vr == System.Windows.Forms.DialogResult.OK) this.handlePaginacion();
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
            if (G.user.contactos().Length == 0)
            {
                cantPaginas = 1;
            }
            lblPagina.Text = "Página " + pagActual.ToString() + " de " + cantPaginas.ToString();
            btnAnterior.Enabled = pagActual != 1;
            btnAnterior.Visible = pagActual != 1;
            btnSiguiente.Enabled = (pagActual != cantPaginas) && (cantPaginas != 0);
            btnSiguiente.Visible = (pagActual != cantPaginas) && (cantPaginas != 0);
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

        protected override void resetPanels()
        {
            base.resetPanels();
            for (int i = 0; i <= (dataContactos.RowCount - 2); i ++) dataContactos.Rows[i].Selected = false;
        }

        private void seleccionar_contacto (int contacto) {
          seleccionar_elemento(contacto, "index", "panel", dataContactos);
        }

        public override void manejar_comando(string comando)
        {

            actualizar_banderas(autenticacion_ok, autenticacion_mal);

            manejar_comando_basico(comando, seleccionar_contacto,
              Comando.Evento("cerrar", btnVolver_Click),
              Comando.Evento("aceptar", btnAceptar_Click),
              Comando.Evento("eliminar", btnEliminar_Click),
              new Comando("anterior", () => {if (btnAnterior.Enabled) btnAnterior_Click(null, EventArgs.Empty);}),
              new Comando("siguiente", () => {if (btnSiguiente.Enabled) btnSiguiente_Click(null, EventArgs.Empty);})
            );
        }
    }
}
