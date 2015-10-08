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
    public partial class leer_mail : RichForm
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

        private void leer_mail_Load(object sender, EventArgs e)
        {

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
            string asunto = "Fwk: " + message_actual.Asunto;
            string mensaje = message_actual.Mensaje;
            redactar_email.redactar form = new redactar_email.redactar(this, asunto, "", mensaje);
            form.Show();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

    }
}
