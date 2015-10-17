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
        Dictionary<int, string> messageUIDLs = new Dictionary<int,string>();
        int pagActual;
        int mailSelected;
        bool recibidos; //true: recibidos. false: enviados.
        uint ultimoRender; //que mails ya mostré o "renderice"
        Color varcolor = Color.FromArgb(174, 225, 242);

        public Form1()
        {
            InitializeComponent();
            agregar_eventos();
        }

        public void SplashScreen()
        {
            Application.Run(new splashScreen());
        }

        public override void manejar_comando(string comando)
        {

            switch (comando)
            {
                case "uno": case "dos": case "tres": case "cuatro": case "cinco": case "seis": case "siete": case "ocho":
                    mailSelected = 1 + Array.IndexOf<string>(
                        (new string[] {"uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho"}),
                        comando);
                    seleccionarMail(mailSelected);
                    leerMail(mailSelected);
                    break;
                case "contactos":
                    btnContactos_Click(null, EventArgs.Empty);
                    break;
                case "asuntos":
                    btnAsuntos_Click(null, EventArgs.Empty);
                    break;
                case "mensajes":
                    btnMensajes_Click(null, EventArgs.Empty);
                    break;
                case "recibidos":
                    btnRecibidos_Click(null, EventArgs.Empty);
                    break;
                case "enviados":
                    btnEnviados_Click(null, EventArgs.Empty);
                    break;
                case "redactar":
                    redactar_Click(null, EventArgs.Empty);
                    break; 
                case "anterior":
                    if (btnAnterior.Enabled) btnAnterior_Click(null, EventArgs.Empty);
                    break;
                case "siguiente":
                    if (btnSiguiente.Enabled) btnSiguiente_Click(null, EventArgs.Empty);
                    break; 
                case "actualizar":
                    btnActualizar_Click(null, EventArgs.Empty);
                    break;
            }
        }

        private void btnRecibidos_Click(object sender, EventArgs e)
        {           
            recibidos = true;
            pagActual = 1;
            lblTitle.Text = "Recibidos";

            if (!messagesRecibidos.ContainsKey(1)) ultimoRender = Convert.ToUInt32(client.GetStatistic().CountMessages);
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
                    from = message.From.DisplayName.ToString();
                else
                    from = message.From.Address.ToString();

                this.dataMails.Rows.Add((index + 1).ToString(), index, from, message.Subject.ToString(),
                                        message.Date.ToLocalTime().ToString());
                index++;
            }
            renderView();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Opacity = 100;
            // create client and connect 

            Thread t = new Thread(new ThreadStart(SplashScreen));
            t.Start();

            try 
            {
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
                
            client.Authenticated += ((Pop3Client c) => this.authenticated = true);

            #if DEBUG
                client.Connected += ((Pop3Client c) => Console.WriteLine("Cliente conectado"));
                client.Authenticated += ((Pop3Client c) => Console.WriteLine("Cliente autenticado"));
                client.MessageReceived += ((Pop3Client c, Rfc822Message m) =>
                    Console.WriteLine("Mensaje recibido: {0}", m.Subject));
                client.Completed += ((Pop3Client c) => Console.WriteLine("Operacion completada"));
                client.Quit += ((Pop3Client c) => Console.WriteLine("Cliente cerrado"));
                client.BrokenMessage += ((Pop3Client c, Pop3MessageInfo i, string err, Rfc822Message m) =>
                    Console.WriteLine("Mensaje {0} no valido: {1}", i.Number, err));
                client.MessageDeleted += ((Pop3Client c, uint n) => Console.WriteLine("Mensaje {0} borrado", n));
            #endif

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
            if (recibidos && messagesRecibidos.ContainsKey(pagActual)) return;
            if (recibidos)
            {
                while (!authenticated) System.Threading.Thread.Sleep(50);

                List<Rfc822Message> list = new List<Rfc822Message>();
                messageUIDLs = new Dictionary<int,string>();

                int index = 0;
                Rfc822Message message;
                uint mailsRenderizados = 0;
                for (uint i = ultimoRender; i > 0; i--)
                {
                    message = client.GetMessage(i);
                    if (message.From.Address != client.Username)
                    {
                        list.Add(message);
                        messageUIDLs.Add(index, client.GetUIDMessage(i).UniqueNumber);
                        index++;
                    }
                    mailsRenderizados++;
                    if (index == 8) break;
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
                    message.__fecha_creacion = message_rfc.Date.ToLocalTime();
                }
                else
                    message = messagesEnviados[index];

                (new leer_mail(message)).Show();
            }
        }

        private void redactar_Click(object sender, EventArgs e)
        {
            (new redactar_email.redactar(this)).Show();
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
            else
                btnEnviados_Click(null, e);
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            pagActual++;
            if (recibidos)
                this.showRecibidos();
            else
                this.showEnviados();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            pagActual--;
            if (recibidos)
                this.showRecibidos();
            else
                this.showEnviados();
        }

        private void handlePaginacion()
        { //se llama siempre que cambia la variable pagActual
            if (recibidos)
                handlePaginacionRecibidos();
            else
                handlePaginacionEnviados();
        }

        private void handlePaginacionRecibidos()
        {
            lblPagina.Text = "Página " + pagActual.ToString();
            btnAnterior.Enabled = pagActual != 1;
            int lastPage = Convert.ToInt32(messagesRecibidos.Keys.Last());
            btnSiguiente.Enabled = !(ultimoRender == 0 && (pagActual == lastPage));
        }

        private void handlePaginacionEnviados() 
        {
            int cant = (int) G.user.mailsEnviados().Length;
            int cantPaginas = (cant + 7) / 8;
            lblPagina.Text = "Página " + pagActual.ToString();
            btnAnterior.Enabled = pagActual != 1;
            btnSiguiente.Enabled = !(pagActual == cantPaginas || cant == 0);
        }       

        private void btnContactos_Click(object sender, EventArgs e)
        {
            (new contactos("home", this)).Show();
        }

        private void btnAsuntos_Click(object sender, EventArgs e)
        {
            (new asuntos("home", this)).Show();
        }

        private void btnMensajes_Click(object sender, EventArgs e)
        {
            (new mensajes("home", this)).Show();
        }

        // METODOS PARA LA VISTA

        private void renderView()
        {
            //clear labels 
            int i = 1;
            for (i = 1; i <= 8; i++)
            {
                string n = i.ToString();
                Control container = this.Controls["panel" + n];
                container.Controls["mailRte" + n].Text = "";
                container.Controls["mailSub" + n].Text = "";
                container.Controls["pictureBox" + n].Hide();
                container.Controls["index" + n].Hide();
                container.Controls["mailDate" + n].Text = "";
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
                        from = message.From.DisplayName.ToString();
                    else
                        from = message.From.Address.ToString();

                    string n = index.ToString();
                    Control container = this.Controls["panel" + n];
                    container.Controls["mailRte" + n].Text = from;
                    container.Controls["mailSub" + n].Text = message.Subject.ToString();
                    container.Controls["pictureBox" + n].Show();
                    container.Controls["index" + n].Show();
                    container.Controls["mailDate" + n].Text = message.Date.ToLocalTime().ToString("dd/MM/yyyy, HH:mm");
                    index++;
                }
            }
            else
            {
                mail_enviado[] viewSource = messagesEnviados;
                foreach (mail_enviado message in viewSource)
                {
                    string to = message.Para;

                    string n = index.ToString();
                    Control container = this.Controls["panel" + n];
                    container.Controls["mailRte" + n].Text = to;
                    container.Controls["mailSub" + n].Text = message.Asunto.ToString();
                    container.Controls["pictureBox" + n].Show();
                    container.Controls["index" + n].Show();
                    container.Controls["mailDate" + n].Text = message.Fecha_creacion.ToString();
                    index++;
                }
            }
        }

        private void resetPanels()
        {
            bool oscuro = true;
            foreach (Panel p in (new Panel[] {panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8})) {
              p.BackColor = oscuro ? Color.FromArgb(241, 255, 255) : Color.White;
              oscuro = !oscuro;
            }
            for (int i = 0; i < dataMails.RowCount; i++) dataMails.Rows[i].Selected = false;
        }

        private void seleccionarMail(int numero)
        {
            resetPanels();
            if ((new Label[] {index1, index2, index3, index4, index5, index6, index7, index8})[numero - 1].Visible)
            {
                this.Controls["panel"+numero].BackColor = varcolor;
                dataMails.Rows[numero - 1].Selected = true;
            }
        }

        public void leerMail(int numero) {
            if ((new Label[] {index1, index2, index3, index4, index5, index6, index7, index8})[numero - 1].Visible)
            {
                mail_enviado message = new mail_enviado();
                if (recibidos)
                {
                    Rfc822Message message_rfc = messagesRecibidos[pagActual][numero - 1];
                    message.__mensaje = message_rfc.Text.ToString();
                    message.__para = message_rfc.From.GetEmailString().Replace("<", "").Replace(">", "");
                    message.__asunto = message_rfc.Subject.ToString();
                    message.__fecha_creacion = message_rfc.Date.ToLocalTime();
                }
                else
                    message = messagesEnviados[numero - 1];

                leer_mail form = new leer_mail(message);
                form.Show();
            }
        }

        private void btnAnterior_EnabledChanged(object sender, System.EventArgs e)
        {
            btnAnterior.ForeColor = sender.Equals(false) ? Color.Blue : Color.Red;
        }


        // manejadores de eventos que no se pueden agregar directamente en el designer
        public void agregar_eventos ()
        {
            Control[][] controles_mails = new Control[][] {
              new Control[] {panel1, index1, mailDate1, mailSub1, mailRte1, pictureBox1},
              new Control[] {panel2, index2, mailDate2, mailSub2, mailRte2, pictureBox2},
              new Control[] {panel3, index3, mailDate3, mailSub3, mailRte3, pictureBox3},
              new Control[] {panel4, index4, mailDate4, mailSub4, mailRte4, pictureBox4},
              new Control[] {panel5, index5, mailDate5, mailSub5, mailRte5, pictureBox5},
              new Control[] {panel6, index6, mailDate6, mailSub6, mailRte6, pictureBox6},
              new Control[] {panel7, index7, mailDate7, mailSub7, mailRte7, pictureBox7},
              new Control[] {panel8, index8, mailDate8, mailSub8, mailRte8, pictureBox8}
            };
            int num;
            for (num = 1; num <= controles_mails.Length; num ++)
              foreach (Control control in controles_mails[num - 1]) {
                int k = num; // usar directamente num almacena una referencia a num, y num = 9 al salir del ciclo
                control.Click += (object sender, EventArgs e) => seleccionarMail(k);
                control.DoubleClick += (object sender, EventArgs e) => leerMail(k);
              }
        }
    }
}
