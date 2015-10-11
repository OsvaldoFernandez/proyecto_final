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

            switch (comando)
            {
                case "responder":
                    btnResponder_Click(null, null);
                    break;
                case "reenviar":
                    btnReenviar_Click(null, null);
                    break;
                case "eliminar":
                    btnEliminar_Click(null, null);
                    break;
                case "cerrar":
                    btnCerrar_Click(null, null);
                    break;
                default:
                    break;
            }
        }

        public override void manejar_aceptar(string contexto)
        {
            if (contexto == "Eliminar")
            {
                btnEliminar_Click(null, null);
            }
        }

        public override void manejar_cerrar(string contexto)
        {
            //Cuando se oprime cerrar en el alert de Eliminar no debería pasar nada
        }

    }
}
