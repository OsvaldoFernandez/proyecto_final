﻿using System;
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

namespace clienteMail.redactar_email
{
    public partial class redactar : RichForm
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
            if (mensaje != "")
            {
                webBrowser.Visible = true;
            }
            else
            {
                webBrowser.Visible = false;
            }
        }

        public void SplashScreen()
        {
            Application.Run(new splashScreen());
        }

        private void enviarBtn_Click(object sender, EventArgs e)
        {
            if (toTxt.Text.Trim() == "")
            {
                var form = new frmAlert(this, "Ingresar destinatario", "Debe agregar un destinatario en el campo Para", "close");
                DialogResult vr = form.ShowDialog(this);
                return;
            }

            Thread t = new Thread(new ThreadStart(SplashScreen));
            t.Start();
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
            client.Send(mail);

            if (contactoID > 0)
            {
                G.user.contacto_enviado(contactoID);
            }
            if (asuntoID > 0)
            {
                G.user.asunto_usado(asuntoID);
            }
            if (mensajeID > 0)
            {
                G.user.mensaje_usado(mensajeID);
            }

            mail_enviado mailEnviado = new mail_enviado();
            mailEnviado.__para = To;
            mailEnviado.__asunto = Subject;
            mailEnviado.__mensaje = Body;

            G.user.guardarMail(mailEnviado);
            
            t.Abort();

            var form2 = new frmAlert(this, "Mail enviado", "El mail ha sido enviado exitosamente", "close");
            form2.Show();
        }

        private void adjBtn_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            
        }

        private void toTxt_TextChanged(object sender, EventArgs e)
        {
        }

        private void redactar_Load(object sender, EventArgs e)
        {

        }

        private void btnPara_Click(object sender, EventArgs e)
        {
            var form = new contactos("redactar", this);
            form.Show();
        }

        private void btnAsunto_Click(object sender, EventArgs e)
        {
            var form = new asuntos("redactar", this);
            form.Show();

        }

        private void btnMensaje_Click(object sender, EventArgs e)
        {
            var form = new mensajes("redactar", this);
            form.Show();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var form = new frmAlert(this, "Descartar", "Está seguro que desea descartar los cambios?", "yesno");
            form.Show(this);
        }

        public override void manejar_comando(string comando)
        {

            switch (comando)
            {
                case "para":
                    btnPara_Click(null, null);
                    break;
                case "asuntos":
                    btnAsunto_Click(null, null);
                    break;
                case "mensajes":
                    btnMensaje_Click(null, null);
                    break;
                case "enviar":
                    enviarBtn_Click(null, null);
                    break;
                case "cancelar":
                    btnCancelar_Click(null, null);
                    break;
                default:
                    break;
            }
        }

        public override void agregar_contacto(int id_contacto)
        {
                contactoID = id_contacto;
                toTxt.Text = G.user.getContacto(id_contacto).Mail;
        }

        public override void agregar_asunto(int id_asunto)
        {
            contactoID = id_asunto;
            asuntoTxt.Text = G.user.getAsunto(id_asunto).Texto;
        }

        public override void agregar_mensaje(int id_mensaje)
        {
            contactoID = id_mensaje;
            cuerpoTxt.Text = G.user.getMensaje(id_mensaje).Texto;
        }

        public override void manejar_aceptar(string contexto)
        {
            if (contexto == "Descartar")
            {
                //Cuando se oprime aceptar en el alert de Descartar, esta ventana debería cerrarse
                this.Close();
            }
        }

        public override void manejar_cerrar(string contexto)
        {
            if (contexto == "Mail enviado")
            {
                //Cuando se oprime cerrar en el alert de mail enviado, esta ventana debería cerrarse
                this.Close();
            }
            if (contexto == "Descartar")
            {
                //Cuando se oprime cerrar en el alert de Descartar no debería pasar nada
            }
           
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
        }

    }
}

