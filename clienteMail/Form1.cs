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
using Email.Net.Pop3.Exceptions;

namespace clienteMail
{
    public partial class Form1 : Form
    {
        Pop3Client client;

        Dictionary<int, Rfc822Message[]> messagesRecibidos = new Dictionary<int, Rfc822Message[]>();
        Dictionary<int, Rfc822Message[]> messagesEnviados = new Dictionary<int, Rfc822Message[]>();
        Dictionary<int, uint> serialNumbers = new Dictionary<int, uint>();
        int pagActual;
        bool recibidos; //true: recibidos. false: enviados.
        uint ultimoRender; //que mails ya mostré o "renderice"

        public Form1()
        {
            InitializeComponent();
        }

        private void btnRecibidos_Click(object sender, EventArgs e)
        {
            recibidos = true;
            pagActual = 1;

            if (!messagesRecibidos.ContainsKey(1))
            {
                ultimoRender = Convert.ToUInt32(client.GetStatistic().CountMessages);
            }
            dataMails.Columns[2].HeaderText = "De";

            this.showRecibidos();
        }

        private void showRecibidos()
        {
            this.dataMails.Rows.Clear();

            int index = 0;
            this.getMails();
            this.handlePaginacion();
            string from;

            foreach (Rfc822Message message in messagesRecibidos[pagActual])
            {
                if (message.From.DisplayName.ToString().Length > 0)
                {
                    from = message.From.DisplayName.ToString();
                }
                else
                {
                    from = message.From.Address.ToString();
                }

                this.dataMails.Rows.Add((index + 1).ToString(), index, from, message.Subject.ToString(), message.Date.AddHours(-3).ToString());
                index++;
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

                client = new Pop3Client(G.user.POP3server, G.user.POP3port, G.user.Mail, G.user.Password);
                
                client.Connected += new Pop3ClientEventHandler(client_Connected);
                client.Authenticated += new Pop3ClientEventHandler(client_Authenticated);
                client.MessageReceived += new Pop3MessageEventHandler(client_MessageReceived);
                client.Completed += new Pop3ClientEventHandler(client_Completed);
                client.Quit += new Pop3ClientEventHandler(client_Quit);
                client.BrokenMessage += new Pop3MessageInfoEventHandler(client_BrokenMessage);
                client.MessageDeleted += new Pop3MessageIDHandler(client_MessageDeleted);

                client.SSLInteractionType = EInteractionType.SSLPort;

                recibidos = true;
                this.actualizar();
                btnRecibidos_Click(null, e);
            }

            private void actualizar()
            {
                messagesRecibidos = new Dictionary<int, Rfc822Message[]>();
                messagesEnviados = new Dictionary<int, Rfc822Message[]>();
                try
                {
                    client.Login();
                }
                catch (Exception)
                {
                    MessageBox.Show("No hay conexión a Internet.");
                    System.Environment.Exit(1);
                }
            }

            private void getMails()
            {
                if (recibidos && messagesRecibidos.ContainsKey(pagActual))
                {
                    return;
                }
                if (!recibidos && messagesEnviados.ContainsKey(pagActual))
                {
                    return;
                }

                List<Rfc822Message> list = new List<Rfc822Message>();
                serialNumbers = new Dictionary<int, uint>();
                

                int index = 0;
                Rfc822Message message;
                uint mailsRenderizados = 0;
                for (uint i = ultimoRender; i > 0; i--)
                {
                    message = client.GetMessage(i);

                    if ((message.From.Address == client.Username && !recibidos) || (message.From.Address != client.Username && recibidos))
                    {
                        list.Add(message);
                        serialNumbers.Add(index, client.GetMessageInfo(i).Number);
                        index++;
                    }
                    mailsRenderizados++;
                    if (index == 10) //Cambiar por 10
                    {
                        break;
                    }
                }

                ultimoRender = ultimoRender - mailsRenderizados;

                if (recibidos)
                {
                    messagesRecibidos.Add(pagActual, list.ToArray());
                }
                else
                {
                    messagesEnviados.Add(pagActual, list.ToArray());   
                }
                
            }

            private void btnEnviados_Click(object sender, EventArgs e)
            {
                recibidos = false;

                pagActual = 1;

                if (!messagesEnviados.ContainsKey(1))
                {
                    ultimoRender = Convert.ToUInt32(client.GetStatistic().CountMessages);
                }
                dataMails.Columns[2].HeaderText = "Para";        

                this.showEnviados();
            }

            private void showEnviados()
            {
                this.dataMails.Rows.Clear();
                int index = 0;
                string para;
                this.getMails();
                this.handlePaginacion();

                foreach (Rfc822Message message in messagesEnviados[pagActual])
                {

                    if (message.To.Count() > 1)
                    {
                        //hay mas de un destinatario
                        para = message.To.Count().ToString() + " destinatarios";
                    }
                    else if (string.IsNullOrEmpty(message.To[0].DisplayName))
                    {
                        para = message.To[0].ToString();
                    }
                    else
                    {
                        para = message.To[0].DisplayName;
                    }
                    
                    this.dataMails.Rows.Add((index + 1).ToString(), index, para, message.Subject.ToString(), message.Date.AddHours(-3).ToString());
                    index++;

                }
            }

            private void dataMails_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
                if (this.dataMails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    int index = Convert.ToInt32(this.dataMails.Rows[e.RowIndex].Cells["index"].Value);
                    Rfc822Message message;
                    if (recibidos)
                    {
                        message = messagesRecibidos[pagActual][index];
                    }
                    else
                    {
                        message = messagesEnviados[pagActual][index];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            private void redactar_Click(object sender, EventArgs e)
            {
                redactar_email.redactar form = new redactar_email.redactar();
                form.Show();
            }

            private void btnActualizar_Click(object sender, EventArgs e)
            {
                this.actualizar();
                if (recibidos)
                {
                    btnRecibidos_Click(null, e);
                }
                else {
                    btnEnviados_Click(null, e);
                }

            }

            private void btnSiguiente_Click(object sender, EventArgs e)
            {
                pagActual++;
                if (recibidos)
                {
                    this.showRecibidos();
                }
                else
                {
                    this.showEnviados();
                }
            }

            private void btnAnterior_Click(object sender, EventArgs e)
            {
                pagActual--;
                if (recibidos)
                {
                    this.showRecibidos();
                }
                else
                {
                    this.showEnviados();
                }
            }

            private void handlePaginacion() //se llama siempre que cambia la variable pagActual
            {
                lblPagina.Text = "Página: " + pagActual.ToString();
                if (pagActual == 1)
                {
                    btnAnterior.Enabled = false;
                }
                else {
                    btnAnterior.Enabled = true;
                }
                int lastPage = Convert.ToInt32(messagesRecibidos.Keys.Last());
                bool lastPageRecibidos = recibidos && pagActual == lastPage;
                bool lastPageEnviados = false;
                if (messagesEnviados.ContainsKey(1))
                {
                    lastPage = Convert.ToInt32(messagesEnviados.Keys.Last());
                    lastPageEnviados = !recibidos && pagActual == lastPage;
                }
                if (ultimoRender == 0 && (lastPageEnviados || lastPageRecibidos))
                {
                    btnSiguiente.Enabled = false;
                }
                else
                {
                    btnSiguiente.Enabled = true;
                }
            }

            private void btnEliminar_Click(object sender, EventArgs e)
            {

                Int32 selectedRowCount = dataMails.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Está seguro que desea eliminar el mail?", "Eliminar", MessageBoxButtons.YesNo);
                    if(dialogResult == DialogResult.Yes)
                    {
                        int index = Convert.ToInt32(this.dataMails.SelectedRows[0].Cells["index"].Value);
                        client.DeleteMessage(serialNumbers[index]);
                    }
                }
                
            }

            private void btnContactos_Click(object sender, EventArgs e)
            {
                var form = new contactos("home");
                form.Show();
            }

            private void btnAsuntos_Click(object sender, EventArgs e)
            {
                var form = new asuntos("home");
                form.Show();
            }

            private void btnMensajes_Click(object sender, EventArgs e)
            {
                var form = new mensajes("home");
                form.Show();
            }
    }
}
