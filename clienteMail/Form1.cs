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
    public partial class Form1 : RichForm
    {
        Pop3Client client;

        Dictionary<int, Rfc822Message[]> messagesRecibidos = new Dictionary<int, Rfc822Message[]>();
        Dictionary<int, Rfc822Message[]> messagesEnviados = new Dictionary<int, Rfc822Message[]>();
        Dictionary<int, uint> serialNumbers = new Dictionary<int, uint>();
        int pagActual;
        bool recibidos; //true: recibidos. false: enviados.
        uint ultimoRender; //que mails ya mostré o "renderice"
        Color varcolor = Color.FromArgb(255, 255, 224);

        public Form1()
        {
            InitializeComponent();
        }

        public override void manejar_comando(string comando)
        {
            switch (comando)
            {
                case "uno":
                    leerMail1(null,null);
                    break;
                case "dos":
                    leerMail2(null, null);
                    break;
                case "tres":
                    leerMail3(null,null);
                    break;
                case "cuatro":
                    leerMail4(null,null);;
                    break;
                case "cinco":
                    leerMail5(null,null);
                    break;
                case "seis":
                    leerMail6(null,null);;
                    break;
                case "siete":
                    leerMail7(null,null);
                    break;
                case "ocho":
                    leerMail8(null,null);
                    break;
                case "nueve":
                    // DO NOTHING
                    break;
            }
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
            renderView();
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
                    if (index == 8) //Cambiar por 8
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

                };
                renderView();
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
                lblPagina.Text = "Página " + pagActual.ToString();
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

        // METODOS PARA LA VISTA

            private void renderView()
            {
                //clear labels 
                int i = 1;
                for (i = 1; i <= 8; i++)
                {
                    string labelName = "mailRte" + i.ToString();
                    string label2Name = "mailSub" + i.ToString();
                    string containerName = "panel" + i.ToString();
                    string indexName = "pictureBox" + i.ToString();
                    string labelIndexName = "index" + i.ToString();
                    string labelDateName = "mailDate" + i.ToString();
                    Control container = this.Controls[containerName];
                    Control ctn = container.Controls[labelName];
                    Control ctn2 = container.Controls[label2Name];
                    Control ctn3 = container.Controls[indexName];
                    Control ctn4 = container.Controls[labelIndexName];
                    Control ctn5 = container.Controls[labelDateName];
                    ctn.Text = "";
                    ctn2.Text = "";
                    ctn3.Hide();
                    ctn4.Hide();
                    ctn5.Text = "";
                }

                resetPanels();
                //rewrite labels
                int index = 1;
                string from;
                Dictionary<int, Rfc822Message[]> viewSource = new Dictionary<int, Rfc822Message[]>();
                if (recibidos)
                {
                    viewSource = messagesRecibidos;
                }
                else
                {
                    viewSource = messagesEnviados;
                }


                foreach (Rfc822Message message in viewSource[pagActual])
                {
                    if (message.From.DisplayName.ToString().Length > 0)
                    {
                        from = message.From.DisplayName.ToString();
                    }
                    else
                    {
                        from = message.From.Address.ToString();
                    }

                    string labelName = "mailRte" + index.ToString();
                    string label2Name = "mailSub" + index.ToString();
                    string containerName = "panel" + index.ToString();
                    string indexName = "pictureBox" + index.ToString();
                    string labelIndexName = "index" + index.ToString();
                    string labelDateName = "mailDate" + index.ToString();
                    Control container = this.Controls[containerName];
                    Control ctn = container.Controls[labelName];
                    Control ctn2 = container.Controls[label2Name];
                    Control ctn3 = container.Controls[indexName];
                    Control ctn4 = container.Controls[labelIndexName];
                    Control ctn5 = container.Controls[labelDateName];
                    ctn.Text = from;
                    ctn2.Text = message.Subject.ToString();
                    ctn3.Show();
                    ctn4.Show();
                    ctn5.Text = message.Date.AddHours(-3).ToString();
                    index++;
                }
            }

            private void resetPanels()
            {
                panel1.BackColor = Color.FromArgb(241, 255, 255);
                panel2.BackColor = Color.White;
                panel3.BackColor = Color.FromArgb(241, 255, 255);
                panel4.BackColor = Color.White;
                panel5.BackColor = Color.FromArgb(241, 255, 255);
                panel6.BackColor = Color.White;
                panel7.BackColor = Color.FromArgb(241, 255, 255);
                panel8.BackColor = Color.White;
                int i = 0;
                for (i = 0; i <= (dataMails.RowCount - 1); i++)
                {
                    dataMails.Rows[i].Selected = false;
                }
            }

            private void seleccionarMail1(object sender, EventArgs e)
            {
                resetPanels();
                if (index1.Visible)
                {
                    panel1.BackColor = varcolor;
                    dataMails.Rows[0].Selected = true;
                }
            }

            private void seleccionarMail2(object sender, EventArgs e)
            {
                resetPanels();
                if (index2.Visible)
                {
                    panel2.BackColor = varcolor;
                    dataMails.Rows[1].Selected = true;
                }
            }

            private void seleccionarMail3(object sender, EventArgs e)
            {
                resetPanels();
                if (index3.Visible)
                {
                    panel3.BackColor = varcolor;
                    dataMails.Rows[2].Selected = true;
                }
            }

            private void seleccionarMail4(object sender, EventArgs e)
            {
                resetPanels();
                if (index4.Visible)
                {
                    panel4.BackColor = varcolor;
                    dataMails.Rows[3].Selected = true;
                }
            }

            private void seleccionarMail5(object sender, EventArgs e)
            {
                resetPanels();
                if (index5.Visible)
                {
                    panel5.BackColor = varcolor;
                    dataMails.Rows[4].Selected = true;
                }
            }

            private void seleccionarMail6(object sender, EventArgs e)
            {
                resetPanels();
                if (index6.Visible)
                {
                    panel6.BackColor = varcolor;
                    dataMails.Rows[5].Selected = true;
                }
            }

            private void seleccionarMail7(object sender, EventArgs e)
            {
                resetPanels();
                if (index7.Visible)
                {
                    panel7.BackColor = varcolor;
                    dataMails.Rows[6].Selected = true;
                }
            }

            private void seleccionarMail8(object sender, EventArgs e)
            {
                resetPanels();
                if (index8.Visible)
                {
                    panel8.BackColor = varcolor;
                    dataMails.Rows[7].Selected = true;
                }
            }




            private void leerMail1(object sender, EventArgs e)
            {
                if (index1.Visible)
                {
                    Rfc822Message message;
                    if (recibidos)
                    {
                        message = messagesRecibidos[pagActual][0];
                    }
                    else
                    {
                        message = messagesEnviados[pagActual][0];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            private void leerMail2(object sender, EventArgs e)
            {
                if (index3.Visible)
                {
                    Rfc822Message message;
                    if (recibidos)
                    {
                        message = messagesRecibidos[pagActual][1];
                    }
                    else
                    {
                        message = messagesEnviados[pagActual][1];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }


            private void leerMail3(object sender, EventArgs e)
            {
                if (index3.Visible)
                {
                    Rfc822Message message;
                    if (recibidos)
                    {
                        message = messagesRecibidos[pagActual][2];
                    }
                    else
                    {
                        message = messagesEnviados[pagActual][2];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            private void leerMail4(object sender, EventArgs e)
            {
                if (index4.Visible)
                {
                    Rfc822Message message;
                    if (recibidos)
                    {
                        message = messagesRecibidos[pagActual][3];
                    }
                    else
                    {
                        message = messagesEnviados[pagActual][3];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            private void leerMail5(object sender, EventArgs e)
            {
                if (index5.Visible)
                {
                    Rfc822Message message;
                    if (recibidos)
                    {
                        message = messagesRecibidos[pagActual][4];
                    }
                    else
                    {
                        message = messagesEnviados[pagActual][4];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            private void leerMail6(object sender, EventArgs e)
            {
                if (index6.Visible)
                {
                    Rfc822Message message;
                    if (recibidos)
                    {
                        message = messagesRecibidos[pagActual][5];
                    }
                    else
                    {
                        message = messagesEnviados[pagActual][5];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            private void leerMail7(object sender, EventArgs e)
            {
                if (index7.Visible)
                {
                    Rfc822Message message;
                    if (recibidos)
                    {
                        message = messagesRecibidos[pagActual][6];
                    }
                    else
                    {
                        message = messagesEnviados[pagActual][6];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            private void leerMail8(object sender, EventArgs e)
            {
                if (index8.Visible)
                {
                    Rfc822Message message;
                    if (recibidos)
                    {
                        message = messagesRecibidos[pagActual][7];
                    }
                    else
                    {
                        message = messagesEnviados[pagActual][7];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }
    }
}
