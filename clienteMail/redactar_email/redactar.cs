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

namespace clienteMail.redactar_email
{
    public partial class redactar : Form
    {
        private string To;
        private string Subject;
        private string Body;
        private int asuntoID = 0, mensajeID = 0, contactoID = 0;

        private MailMessage mail;

        public redactar()
        {
            InitializeComponent();
        }
       

        private void enviarBtn_Click_1(object sender, EventArgs e)
        {
            if (!(toTxt.Text.Trim() == ""))
            {
                To = toTxt.Text;
                Subject = asuntoTxt.Text;
                Body = cuerpoTxt.Text;

                mail = new MailMessage();
                mail.To.Add(new MailAddress(this.To));
                mail.From = new MailAddress("proyectofinal512@gmail.com", "Grupo 512 - Proyecto final");
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = false;

                SmtpClient client = new SmtpClient(G.user.SMTPserver, G.user.SMTPport);
                
                client.Credentials = new System.Net.NetworkCredential(G.user.Mail, G.user.Password);
                client.EnableSsl = true;
                client.Send(mail);
                MessageBox.Show("Mail enviado exitosamente");

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

                this.Close();
            }
        }

        private void adjBtn_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            //adjTxt.Text = openFileDialog1.FileName;
            
        }

        private void toTxt_TextChanged(object sender, EventArgs e)
        {
            //contactos form = new contactos();
            //form.Show();
        }

        private void redactar_Load(object sender, EventArgs e)
        {

        }

        private void btnPara_Click(object sender, EventArgs e)
        {
            var form = new contactos("redactar");
            DialogResult res = form.ShowDialog(this);
            if (res != System.Windows.Forms.DialogResult.Cancel)
            {
                contactoID = form.idSelected;
                toTxt.Text = G.user.getContacto(contactoID).Mail;
            }
        }

        private void btnAsunto_Click(object sender, EventArgs e)
        {
            var form = new asuntos("redactar");
            DialogResult res = form.ShowDialog(this);
            if (res != System.Windows.Forms.DialogResult.Cancel)
            {
                asuntoID = form.idSelected;
                asuntoTxt.Text = G.user.getAsunto(asuntoID).Texto;
            }
        }

        private void btnMensaje_Click(object sender, EventArgs e)
        {
            var form = new mensajes("redactar");
            DialogResult res = form.ShowDialog(this);
            if (res != System.Windows.Forms.DialogResult.Cancel)
            {
                mensajeID = form.idSelected;
                cuerpoTxt.Text = G.user.getMensaje(mensajeID).Texto;
            }
        }

    }
}

