using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Email.Net;
using Email.Net.Common;
using Email.Net.Pop3;
using Email.Net.Common.Configurations;
using Email.Net.Common.Collections;

namespace clienteMail
{
    public partial class Form1 : Form
    {
        Pop3Client client;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRecibidos_Click(object sender, EventArgs e)
        {
            this.dataMails.Rows.Clear();
            Rfc822Message[] messages = client.GetAllMessages().ToArray();
            
            int i = 1;

            foreach (Rfc822Message message in messages)
            {
                if(message.From.Address != client.Username){
                    this.dataMails.Rows.Add(i.ToString(), message.From.DisplayName.ToString(), message.Subject.ToString(), message.Date.AddHours(-3).ToString());
                    i++;
                }
                
            }
        }

            void client_Connected(Pop3Client sender)
            {
                Console.WriteLine("Client connected");
            }
            void client_Authenticated(Pop3Client sender)
            {
                Console.WriteLine("Client authentificated");
            }
            void client_MessageReceived(Pop3Client sender, Rfc822Message message)
            {
                Console.WriteLine(string.Format("Message received: {0}", message.Subject));
            }
            void client_Completed(Pop3Client sender)
            {
                Console.WriteLine("Job completed");
            }
            void client_Quit(Pop3Client sender)
            {
                Console.WriteLine("Client left");
            }
            void client_BrokenMessage(Pop3Client sender, Pop3MessageInfo messageInfo, string errorMessage, Rfc822Message message)
            {
                Console.WriteLine(string.Format("message is broken: {0}", messageInfo.Number));
            }
            void client_MessageDeleted(Pop3Client sender, uint serialNumber)
            {
                Console.WriteLine(string.Format("message deleted: {0}", serialNumber));
            }

            private void Form1_Load(object sender, EventArgs e)
            {
                // create client and connect 
                client = new Pop3Client("pop.gmail.com", 995, "proyectofinal512@gmail.com", "proyecto123");

                client.Connected += new Pop3ClientEventHandler(client_Connected);
                client.Authenticated += new Pop3ClientEventHandler(client_Authenticated);
                client.MessageReceived += new Pop3MessageEventHandler(client_MessageReceived);
                client.Completed += new Pop3ClientEventHandler(client_Completed);
                client.Quit += new Pop3ClientEventHandler(client_Quit);
                client.BrokenMessage += new Pop3MessageInfoEventHandler(client_BrokenMessage);
                client.MessageDeleted += new Pop3MessageIDHandler(client_MessageDeleted);

                client.SSLInteractionType = EInteractionType.SSLPort;
                // authenticate 
                client.Login();
                btnRecibidos_Click(null, EventArgs.Empty);

            }

            private void btnEnviados_Click(object sender, EventArgs e)
            {
                this.dataMails.Rows.Clear();

                dataMails.Columns[1].HeaderText = "Para";
                Rfc822MessageCollection messages = client.GetAllMessages();

                int i = 1;
                string para;
                
                foreach (Rfc822Message message in messages)
                {
                    if (message.From.Address == client.Username)
                    {
                        if (message.To.Count() > 1)
                        {
                            //hay mas de un destinatario
                            para = message.To.Count().ToString() + " destinatarios";
                        } else
                        if(string.IsNullOrEmpty(message.To[0].DisplayName)){
                            para = message.To[0].ToString();
                        } else 
                        {
                            para = message.To[0].DisplayName;
                        }
                        
                        this.dataMails.Rows.Add(i.ToString(), para, message.Subject.ToString(), message.Date.AddHours(-3).ToString());
                        i++;
                    }
                    
                }
            }
        
    }
}
