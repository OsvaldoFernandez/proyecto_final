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

        private MailMessage mail;
        private Attachment Data;

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
                /*
                if (!(adjTxt.Text.Trim() == ""))
                {
                    Data = new Attachment(adjTxt.Text, MediaTypeNames.Application.Octet);
                    mail.Attachments.Add(Data);
                }*/


                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                //using (client)
                //{
                client.Credentials = new System.Net.NetworkCredential("proyectofinal512@gmail.com", "proyecto123");
                client.EnableSsl = true;
                client.Send(mail);
                //}
                MessageBox.Show("Mensaje enviado");
                this.Close();
            }
        }

        private void adjBtn_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            //adjTxt.Text = openFileDialog1.FileName;
            
        }
    }
}

