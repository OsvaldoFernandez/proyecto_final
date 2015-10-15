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
using System.Reflection;
using System.Globalization;
using System.Threading;
using clienteMail.inciar_sesion;

namespace clienteMail
{
    public partial class Form1 : RichForm
    {
        Pop3Client client;
        bool authenticated = false;

        Dictionary<int, Rfc822Message[]> messagesRecibidos = new Dictionary<int, Rfc822Message[]>();
        mail_enviado[] messagesEnviados = new mail_enviado[8];
        Dictionary<int, uint> serialNumbers = new Dictionary<int, uint>();
        int pagActual;
        int mailSelected;
        bool recibidos; //true: recibidos. false: enviados.
        uint ultimoRender; //que mails ya mostré o "renderice"
        Color varcolor = Color.FromArgb(174, 225, 242);

        public Form1()
        {
            InitializeComponent();
        }

        public void SplashScreen()
        {
            Application.Run(new splashScreen());
        }

        public override void manejar_comando(string comando)
        {

            switch (comando)
            {
                case "uno":
                    seleccionarMail1(null,null);
                    mailSelected = 1;
                    leerMail1(null, null);
                    break;
                case "dos":
                    seleccionarMail2(null, null);
                    mailSelected = 2;
                    leerMail2(null, null);
                    break;
                case "tres":
                    seleccionarMail3(null,null);
                    mailSelected = 3;
                    leerMail3(null, null);
                    break;
                case "cuatro":
                    seleccionarMail4(null,null);
                    mailSelected = 4;
                    leerMail4(null, null);
                    break;
                case "cinco":
                    seleccionarMail5(null,null);
                    mailSelected = 5;
                    leerMail5(null, null);
                    break;
                case "seis":
                    seleccionarMail6(null,null);
                    mailSelected = 6;
                    leerMail6(null, null);
                    break;
                case "siete":
                    seleccionarMail7(null,null);
                    mailSelected = 7;
                    leerMail7(null, null);
                    break;
                case "ocho":
                    seleccionarMail8(null,null);
                    mailSelected = 8;
                    leerMail8(null, null);
                    break;
                case "contactos":
                    btnContactos_Click(null, null);
                    break;
                case "asuntos":
                    btnAsuntos_Click(null, null);
                    break;
                case "mensajes":
                    btnMensajes_Click(null, null);
                    break;
                case "recibidos":
                    btnRecibidos_Click(null, null);
                    break;
                case "enviados":
                    btnEnviados_Click(null, null);
                    break;
                case "redactar":
                    redactar_Click(null, null);
                    break; 
                case "anterior":
                    if (btnAnterior.Enabled)
                    {
                        btnAnterior_Click(null, null);
                    }
                    break;
                case "siguiente":
                    if (btnSiguiente.Enabled)
                    {
                        btnSiguiente_Click(null, null);
                    }
                    break; 
                case "actualizar":
                    btnActualizar_Click(null, null);
                    break;
                default:
                    break;
            }
        }

        private void btnRecibidos_Click(object sender, EventArgs e)
        {           
            recibidos = true;
            pagActual = 1;
            lblTitle.Text = "Recibidos";

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
                authenticated = true;
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
                this.Opacity = 100;
                // create client and connect 

                Thread t = new Thread(new ThreadStart(SplashScreen));
                t.Start();

                try{
                    client = new Pop3Client(G.user.POP3server, G.user.POP3port, G.user.Mail, G.user.Password);
                }
                catch
                {
                    t.Abort();
                    this.Opacity = 0;

                    var form2 = new frmAlert(this, "Error", "Hubo un inconveniente técnico.\n Vuelva a intentarlo más tarde.", "close");
                    form2.Show();
                    return;
                }
                
                
                client.Connected += new Pop3ClientEventHandler(client_Connected);
                client.Authenticated += new Pop3ClientEventHandler(client_Authenticated);
                client.MessageReceived += new Pop3MessageEventHandler(client_MessageReceived);
                client.Completed += new Pop3ClientEventHandler(client_Completed);
                client.Quit += new Pop3ClientEventHandler(client_Quit);
                client.BrokenMessage += new Pop3MessageInfoEventHandler(client_BrokenMessage);
                client.MessageDeleted += new Pop3MessageIDHandler(client_MessageDeleted);

                client.SSLInteractionType = EInteractionType.SSLPort;

                recibidos = true;
                try
                {
                    this.actualizar();
                    btnRecibidos_Click(null, e);
                    t.Abort();
                }
                catch
                {
                    t.Abort();
                    this.Opacity = 0;

                    var iniciarSesion = new iniciar_sesion();
                    iniciarSesion.Show();
                    var form2 = new frmAlert(this, "Error", "Hubo un inconveniente técnico.\n Vuelva a intentarlo más tarde.", "close");
                    form2.Show();
                    
                    return;
                }
                
            }

            private void actualizar()
            {
                messagesRecibidos = new Dictionary<int, Rfc822Message[]>();
                client.Login();
            }

            private void getMails()
            {
                if (recibidos && messagesRecibidos.ContainsKey(pagActual))
                {
                    return;
                }
                if (recibidos)
                {
                    while (!authenticated)
                    {
                        System.Threading.Thread.Sleep(50);
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
                        if (index == 8)
                        {
                            break;
                        }
                    }

                    ultimoRender = ultimoRender - mailsRenderizados;

                
                    messagesRecibidos.Add(pagActual, list.ToArray());
                }
                
            }

            private void btnEnviados_Click(object sender, EventArgs e)
            {
                recibidos = false;
                lblTitle.Text = "Enviados";

                pagActual = 1;
                dataMails.Columns[2].HeaderText = "Para";        

                this.showEnviados();
            }

            private void showEnviados()
            {
                this.dataMails.Rows.Clear();
                int index = 0;
                this.handlePaginacion();

                messagesEnviados = G.user.mailEnviadoPag(pagActual);

                foreach (mail_enviado message in messagesEnviados)
                {
                    this.dataMails.Rows.Add((index + 1).ToString(), index, message.Para, message.Asunto, message.Fecha_creacion);
                    index++;

                };
                renderView();
            }

            private void dataMails_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
                if (this.dataMails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    int index = Convert.ToInt32(this.dataMails.Rows[e.RowIndex].Cells["index"].Value);
                    mail_enviado message = new mail_enviado();
                    if (recibidos)
                    {
                        Rfc822Message message_rfc = messagesRecibidos[pagActual][index];
                        message.__mensaje = message_rfc.Text.ToString();
                        message.__from = message_rfc.From.GetEmailString().Replace("<", "").Replace(">", "");
                        message.__asunto = message_rfc.Subject.ToString();
                        message.__fecha_creacion = message_rfc.Date.AddHours(-3);
                    }
                    else
                    {
                        message = messagesEnviados[index];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            private void redactar_Click(object sender, EventArgs e)
            {
                redactar_email.redactar form = new redactar_email.redactar(this);
                form.Show();
            }

            private void btnActualizar_Click(object sender, EventArgs e)
            {
                this.actualizar();
                if (recibidos)
                {
                    Thread t = new Thread(new ThreadStart(SplashScreen));
                    t.Start();
                    btnRecibidos_Click(null, e);
                    t.Abort();
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

            private void handlePaginacion()
            { //se llama siempre que cambia la variable pagActual
                if (recibidos)
                {
                    handlePaginacionRecibidos();
                }
                else
                {
                    handlePaginacionEnviados();
                }
            }
            private void handlePaginacionRecibidos()
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
                bool lastPageRecibidos = pagActual == lastPage;
                if (ultimoRender == 0 && lastPageRecibidos)
                {
                    btnSiguiente.Enabled = false;
                }
                else
                {
                    btnSiguiente.Enabled = true;
                }
            }

            private void handlePaginacionEnviados() 
            {
                int cant = (int)G.user.mailsEnviados().Length;
                int cantPaginas = cant / 8;

                if (G.user.mailsEnviados().Length % 8 > 0)
                {
                    cantPaginas++;
                }

                lblPagina.Text = "Página " + pagActual.ToString();
                if (pagActual == 1)
                {
                    btnAnterior.Enabled = false;
                }
                else
                {
                    btnAnterior.Enabled = true;
                }

                if (pagActual == cantPaginas || cant == 0)
                {
                    btnSiguiente.Enabled = false;
                }
                else
                {
                    btnSiguiente.Enabled = true;
                }
            }       

            private void btnContactos_Click(object sender, EventArgs e)
            {
                var form = new contactos("home", this);
                form.Show();
            }

            private void btnAsuntos_Click(object sender, EventArgs e)
            {
                var form = new asuntos("home", this);
                form.Show();
            }

            private void btnMensajes_Click(object sender, EventArgs e)
            {
                var form = new mensajes("home", this);
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
                
                if (recibidos)
                {
                    Dictionary<int, Rfc822Message[]> viewSource = messagesRecibidos;
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
                        ctn5.Text = message.Date.AddHours(-3).ToString("dd/MM/yyyy, hh:mm");
                        index++;
                    }
                }
                else
                {
                    mail_enviado[] viewSource = messagesEnviados;
                    foreach (mail_enviado message in viewSource)
                    {
                        
                        string to = message.Para;

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
                        ctn.Text = to;
                        ctn2.Text = message.Asunto.ToString();
                        ctn3.Show();
                        ctn4.Show();
                        ctn5.Text = message.Fecha_creacion.ToString();
                        index++;
                    }
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


            public void leerMail1(object sender, EventArgs e)
            {
                if (index1.Visible)
                {
                    mail_enviado message = new mail_enviado();
                    if (recibidos)
                    {
                        Rfc822Message message_rfc = messagesRecibidos[pagActual][0];
                        message.__mensaje = message_rfc.Text.ToString();
                        message.__para = message_rfc.From.GetEmailString().Replace("<", "").Replace(">", "");
                        message.__asunto = message_rfc.Subject.ToString();
                        message.__fecha_creacion = message_rfc.Date.AddHours(-3);
                    }
                    else
                    {
                        message = messagesEnviados[0];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            public void leerMail2(object sender, EventArgs e)
            {
                if (index3.Visible)
                {
                    mail_enviado message = new mail_enviado();
                    if (recibidos)
                    {
                        Rfc822Message message_rfc = messagesRecibidos[pagActual][1];
                        message.__mensaje = message_rfc.Text.ToString();
                        message.__para = message_rfc.From.GetEmailString().Replace("<", "").Replace(">", "");
                        message.__asunto = message_rfc.Subject.ToString();
                        message.__fecha_creacion = message_rfc.Date.AddHours(-3);
                    }
                    else
                    {
                        message = messagesEnviados[1];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }


            public void leerMail3(object sender, EventArgs e)
            {
                if (index3.Visible)
                {
                    mail_enviado message = new mail_enviado();
                    if (recibidos)
                    {
                        Rfc822Message message_rfc = messagesRecibidos[pagActual][2];
                        message.__mensaje = message_rfc.Text.ToString();
                        message.__para = message_rfc.From.GetEmailString().Replace("<", "").Replace(">", "");
                        message.__asunto = message_rfc.Subject.ToString();
                        message.__fecha_creacion = message_rfc.Date.AddHours(-3);
                    }
                    else
                    {
                        message = messagesEnviados[2];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            public void leerMail4(object sender, EventArgs e)
            {
                if (index4.Visible)
                {
                    mail_enviado message = new mail_enviado();
                    if (recibidos)
                    {
                        Rfc822Message message_rfc = messagesRecibidos[pagActual][3];
                        message.__mensaje = message_rfc.Text.ToString();
                        message.__para = message_rfc.From.GetEmailString().Replace("<", "").Replace(">", "");
                        message.__asunto = message_rfc.Subject.ToString();
                        message.__fecha_creacion = message_rfc.Date.AddHours(-3);
                    }
                    else
                    {
                        message = messagesEnviados[3];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            public void leerMail5(object sender, EventArgs e)
            {
                if (index5.Visible)
                {
                    mail_enviado message = new mail_enviado();
                    if (recibidos)
                    {
                        Rfc822Message message_rfc = messagesRecibidos[pagActual][4];
                        message.__mensaje = message_rfc.Text.ToString();
                        message.__para = message_rfc.From.GetEmailString().Replace("<","").Replace(">","");
                        message.__asunto = message_rfc.Subject.ToString();
                        message.__fecha_creacion = message_rfc.Date.AddHours(-3);
                    }
                    else
                    {
                        message = messagesEnviados[4];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            public void leerMail6(object sender, EventArgs e)
            {
                if (index6.Visible)
                {
                    mail_enviado message = new mail_enviado();
                    if (recibidos)
                    {
                        Rfc822Message message_rfc = messagesRecibidos[pagActual][5];
                        message.__mensaje = message_rfc.Text.ToString();
                        message.__para = message_rfc.From.GetEmailString().Replace("<", "").Replace(">", "");
                        message.__asunto = message_rfc.Subject.ToString();
                        message.__fecha_creacion = message_rfc.Date.AddHours(-3);
                    }
                    else
                    {
                        message = messagesEnviados[5];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            public void leerMail7(object sender, EventArgs e)
            {
                if (index7.Visible)
                {
                    mail_enviado message = new mail_enviado();
                    if (recibidos)
                    {
                        Rfc822Message message_rfc = messagesRecibidos[pagActual][6];
                        message.__mensaje = message_rfc.Text.ToString();
                        message.__para = message_rfc.From.GetEmailString().Replace("<", "").Replace(">", "");
                        message.__asunto = message_rfc.Subject.ToString();
                        message.__fecha_creacion = message_rfc.Date.AddHours(-3);
                    }
                    else
                    {
                        message = messagesEnviados[6];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            public void leerMail8(object sender, EventArgs e)
            {
                if (index8.Visible)
                {
                    mail_enviado message = new mail_enviado();
                    if (recibidos)
                    {
                        Rfc822Message message_rfc = messagesRecibidos[pagActual][7];
                        message.__mensaje = message_rfc.Text.ToString();
                        message.__para = message_rfc.From.GetEmailString().Replace("<", "").Replace(">", "");
                        message.__asunto = message_rfc.Subject.ToString();
                        message.__fecha_creacion = message_rfc.Date.AddHours(-3);
                    }
                    else
                    {
                        message = messagesEnviados[7];
                    }

                    leer_mail form = new leer_mail(message);
                    form.Show();
                }
            }

            private void btnAnterior_EnabledChanged(object sender, System.EventArgs e)
            {
                btnAnterior.ForeColor = sender.Equals(false) ? Color.Blue : Color.Red;
            }
    }
}
