using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Text.RegularExpressions;

namespace clienteMail.redactar_email
{
    public partial class redactar : FormComandos
    {
        private string To;
        private string Subject;
        private string Body;
        private int asuntoID = 0, mensajeID = 0, contactoID = 0;

        private MailMessage mail;

        public redactar(RichForm formulario_padre, string asunto = "", string para = "", string mensaje = "")
        {
            InitializeComponent();
            form_padre = formulario_padre;
            asuntoTxt.Text = asunto;
            toTxt.Text = para;
            webBrowser.DocumentText = mensaje;
            webBrowser.Visible = mensaje != "";
        }

        public void SplashScreen()
        {
            Application.Run(new splashScreen());
        }

        private void enviarBtn_Click(object sender, EventArgs e)
        {
            // ver comentario en contacto_new_update.cs respecto de la expresion regular
            Regex reg = new Regex(@"^[^ /?@\x00-\x1f()<>]+@([^. /?@\x00-\x1f()<>]+\.)*[a-zA-Z]{2,}\.?$");

            if (!reg.IsMatch(toTxt.Text))
            {
                var form = new frmAlert(this, "Ingresar destinatario", "Debe agregar un destinatario correcto en el campo Para", "close");
                DialogResult vr = form.ShowDialog(this);
                return;
            }

            Cargando carg = new Cargando();
            carg.Ejecutar();

            To = toTxt.Text;
            Subject = asuntoTxt.Text;
            Body = cuerpoTxt.Text + "<br><br>" + webBrowser.DocumentText;

            mail = new MailMessage();
            mail.To.Add(new MailAddress(this.To));
            mail.From = new MailAddress(G.user.Mail, G.user.Mail);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(G.user.SMTPserver, G.user.SMTPport);
                
            client.Credentials = new System.Net.NetworkCredential(G.user.Mail, G.user.Password);
            client.EnableSsl = true;

            try
            {
                client.Send(mail);

                if (contactoID > 0) G.user.contacto_enviado(contactoID);
                if (asuntoID > 0) G.user.asunto_usado(asuntoID);
                if (mensajeID > 0) G.user.mensaje_usado(mensajeID);

                mail_enviado mailEnviado = new mail_enviado();
                mailEnviado.__para = To;
                mailEnviado.__asunto = Subject;
                mailEnviado.__mensaje = Body;

                G.user.guardarMailEnviado(mailEnviado);

                carg.Detener();

                var form2 = new frmAlert(this, "Mail enviado", "El mail ha sido enviado exitosamente", "close");
                form2.Show();
            }
            catch
            {
                carg.Detener();
                var form2 = new frmAlert(this, "Error", "Hubo un inconveniente técnico. \nVuelva a intentarlo más tarde.", "close");
                form2.Show();
            }
        }

        private void btnPara_Click(object sender, EventArgs e)
        {
            new contactos("redactar", this).Show();
        }

        private void btnAsunto_Click(object sender, EventArgs e)
        {
            new asuntos("redactar", this).Show();
        }

        private void btnMensaje_Click(object sender, EventArgs e)
        {
            new mensajes("redactar", this).Show();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            new frmAlert(this, "Descartar", "¿Está seguro que desea descartar los cambios?", "yesno").Show(this);
        }

        public override void manejar_comando(string comando)
        {
            manejar_comando_basico(comando,
              Comando.Evento("para", btnPara_Click),
              Comando.Evento("asuntos", btnAsunto_Click),
              Comando.Evento("mensajes", btnMensaje_Click),
              Comando.Evento("enviar", enviarBtn_Click),
              Comando.Evento("cancelar", btnCancelar_Click)
            );
        }

        public override void agregar_contacto(int id_contacto)
        {
            contactoID = id_contacto;
            toTxt.Text = G.user.getContacto(id_contacto).Mail;
        }

        public override void agregar_asunto(int id_asunto)
        {
            asuntoID = id_asunto;
            asuntoTxt.Text = G.user.getAsunto(id_asunto).Texto;
        }

        public override void agregar_mensaje(int id_mensaje)
        {
            mensajeID = id_mensaje;
            cuerpoTxt.Text = G.user.getMensaje(id_mensaje).Texto;
        }

        public override void manejar_aceptar(string contexto)
        {
            if (contexto == "Descartar") this.Close();
        }

        public override void manejar_cerrar(string contexto)
        {
            if (contexto == "Mail enviado") this.Close();
        }
    }
}

