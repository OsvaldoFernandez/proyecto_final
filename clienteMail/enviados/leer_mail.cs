using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Email.Net;
using Email.Net.Common;
using Email.Net.Pop3;
using Email.Net.Common.Configurations;
using Email.Net.Common.Collections;


namespace clienteMail
{
    public partial class leer_mail : FormComandos
    {
        mail_enviado message_actual;
        public leer_mail(mail_enviado message)
        {
            InitializeComponent();
            webBrowser.DocumentText = message.Mensaje;
            message_actual = message;

            lblFrom.Text = "De: " + message.__para;
            lblAsunto.Text = "Asunto: " + message.__asunto;
            lblFecha.Text = "Fecha: " + message.__fecha_creacion.ToString();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnResponder_Click(object sender, EventArgs e)
        {
            string asunto = "Re: " + message_actual.Asunto;
            string para = message_actual.Para;
            string mensaje = message_actual.Mensaje;
            redactar_email.redactar form = new redactar_email.redactar(this, asunto, para, mensaje);
            form.Show();
        }

        private void btnReenviar_Click(object sender, EventArgs e)
        {
            string asunto = "Fwd: " + message_actual.Asunto;
            string mensaje = message_actual.Mensaje;
            redactar_email.redactar form = new redactar_email.redactar(this, asunto, "", mensaje);
            form.Show();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var form = new frmAlert(this, "Eliminar", "Está seguro que desea eliminar el mail?", "yesno");
            DialogResult vr = form.ShowDialog(this);
            if (vr == System.Windows.Forms.DialogResult.OK)
            {
                //BORRA MAIL
            } 
        }

        public override void manejar_comando(string comando)
        {
            manejar_comando_basico(comando,
              Comando.Evento("responder", btnResponder_Click),
              Comando.Evento("reenviar", btnReenviar_Click),
              Comando.Evento("eliminar", btnEliminar_Click),
              Comando.Evento("cerrar", btnCerrar_Click)
            );
        }

        public override void manejar_aceptar(string contexto)
        {
            if (contexto == "Eliminar") btnEliminar_Click(null, EventArgs.Empty);
        }
    }
}
