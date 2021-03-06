﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.Threading;
using Email.Net;
using Email.Net.Common;
using Email.Net.Pop3;
using Email.Net.Common.Configurations;
using Email.Net.Common.Collections;
using Email.Net.Pop3.Exceptions;


namespace clienteMail
{
    public partial class Form1 : FormPaginado
    {
        Pop3Client client;

        mail_recibido[] messagesRecibidos = new mail_recibido[8];
        mail_enviado[] messagesEnviados = new mail_enviado[8];
        int mailSelected;
        bool recibidos; //true: recibidos. false: enviados.

        public Form1()
        {
            InitializeComponent();
            string[] controles = {"panel", "index", "mailSub", "mailRte", "mailDate", "pictureBox"};
            agregar_eventos(seleccionarMail, false, controles);
            agregar_eventos(leerMail, true, controles);
            lblPer.Text = "(" + G.sensibilidad_autenticacion.ToString() + "%)";
            lblComando.Text = "(" + G.sensibilidad.ToString() + "%)";
        }

        public override void manejar_comando(string comando)
        {
            actualizar_banderas(autenticacion_ok, autenticacion_mal);

            manejar_comando_basico(comando,
              (int numero) => {
                mailSelected = numero;
                seleccionarMail(numero);
              },
              Comando.Evento("contactos", btnContactos_Click),
              Comando.Evento("asuntos", btnAsuntos_Click),
              Comando.Evento("mensajes", btnMensajes_Click),
              Comando.Evento("recibidos", btnRecibidos_Click),
              Comando.Evento("enviados", btnEnviados_Click),
              Comando.Evento("enviar", btnEnviados_Click),
              Comando.Evento("redactar", redactar_Click),
              new Comando("anterior", () => {if (btnAnterior.Enabled) btnAnterior_Click(null, EventArgs.Empty);}),
              new Comando("siguiente", () => {if (btnSiguiente.Enabled) btnSiguiente_Click(null, EventArgs.Empty);}),
              Comando.Evento("actualizar", btnActualizar_Click),
              Comando.Evento("cerrar sesión", btnCerrar_Click)
            );
            if (comando == "leer" & mailSelected >= 1) leerMail(mailSelected);
        }

        private void btnRecibidos_Click(object sender, EventArgs e)
        {           
            recibidos = true;
            pagActual = 1;
            lblTitle.Text = "Recibidos";

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

            messagesRecibidos = G.user.mailRecibidoPag(pagActual);

            foreach (mail_recibido mail in messagesRecibidos)
            {
                if (mail.Remitente_nombre.Length > 0)
                    from = mail.Remitente_nombre;
                else
                    from = mail.Remitente_mail;

                this.dataMails.Rows.Add((index + 1).ToString(), index, from, mail.Asunto,
                                        mail.Fecha.ToString());
                index++;
            }
            renderView();
        }

        void Form1_FormClosed (object sender, System.Windows.Forms.FormClosedEventArgs e) {
          Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Opacity = 100;

            Cargando carg = new Cargando();
            carg.Ejecutar();

            recibidos = true;
            try {
                pagActual = 1;
                btnRecibidos_Click(null, e);
                carg.Detener();
            } catch {
                carg.Detener();
                this.Opacity = 0;

                new iniciar_sesion.iniciar_sesion().Show();
                var form2 = new frmAlert(this, "Error", "Hubo un inconveniente técnico.\n Vuelva a intentarlo más tarde.", "close");
                form2.Show();
                    
                return;
            }
                
        }

        private void Form1_Deactivate(Object sender, EventArgs e)
        {
            autenticacion_mal.Visible = false;
            autenticacion_ok.Visible = false;
        }

        private void getMails()
        {
            Rfc822Message message;
            mail_recibido mailRecibido = new mail_recibido();

            string uidl;
            if (recibidos)
            {
                Cargando carg = new Cargando();
                carg.Ejecutar();
                //sincronización mails.

                client = G.crear_cliente();

                int ciclos = 0;
                while (client.ConnectionState != EPop3ConnectionState.Authenticated) {
                  Thread.Sleep(100);
                  ciclos ++;
                  if (ciclos > 120) break;
                }

                Pop3MessageUIDInfoCollection messageUIDs = client.GetAllUIDMessages();
                uint i = 1;
                foreach (Pop3MessageUIDInfo uidInfo in messageUIDs)
                {
                    uidl = uidInfo.UniqueNumber;
                    if (!G.user.exists_mailRecibido(uidl))
                    {
                        message = client.GetMessage(i);

                        //guardo mail
                        mailRecibido.__uidl = uidl;
                        mailRecibido.__remitente_nombre = message.From.DisplayName.ToString();
                        mailRecibido.__remitente_mail = message.From.Address.ToString();
                        if (message.From.Address == client.Username)
                            mailRecibido.__asunto = null;
                        else
                            mailRecibido.__asunto = message.Subject.ToString();
                        mailRecibido.__mensaje = message.Text.ToString();
                        mailRecibido.__fecha = message.Date.ToLocalTime();
                        G.user.guardarMailRecibido(mailRecibido);
                    }
                    i++;
                }
                client.Logout();
                carg.Detener();
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
                leerMail(index);
            }
        }

        private void redactar_Click(object sender, EventArgs e)
        {
            (new redactar_email.redactar(this)).Show();
        }

        private void btnActualizar_Click(object sender, EventArgs e) {
            pagActual = 1;
            if (recibidos)
                btnRecibidos_Click(null, e);
            else
                btnEnviados_Click(null, e);
        }

        private void actualizar_vista () {
          if (recibidos)
            showRecibidos();
          else
            showEnviados();
        }

        private void btnSiguiente_Click(object sender, EventArgs e) {
            pagActual++;
            actualizar_vista();
        }

        private void btnAnterior_Click(object sender, EventArgs e) {
            pagActual--;
            actualizar_vista();
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
            btnAnterior.Visible = pagActual != 1;
            int cantPaginas = (G.user.cantidad_mails_recibidos() + 7) / 8;
            btnSiguiente.Enabled = !(pagActual == cantPaginas || cantPaginas == 0);
            btnSiguiente.Visible = !(pagActual == cantPaginas || cantPaginas == 0);
        }

        private void handlePaginacionEnviados() 
        {
            int cantPaginas = (G.user.cantidad_mails_enviados() + 7) / 8;
            lblPagina.Text = "Página " + pagActual.ToString();
            btnAnterior.Enabled = pagActual != 1;
            btnAnterior.Visible = pagActual != 1;
            btnSiguiente.Enabled = !(pagActual == cantPaginas || cantPaginas == 0);
            btnSiguiente.Visible = !(pagActual == cantPaginas || cantPaginas == 0);
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
                mail_recibido[] viewSource = messagesRecibidos;
                foreach (mail_recibido mail in viewSource)
                {
                    if (mail.Remitente_nombre.Length > 0)
                        from = mail.Remitente_nombre;
                    else
                        from = mail.Remitente_mail;

                    string n = index.ToString();
                    Control container = this.Controls["panel" + n];
                    container.Controls["mailRte" + n].Text = from;
                    container.Controls["mailSub" + n].Text = mail.Asunto;
                    container.Controls["pictureBox" + n].Show();
                    container.Controls["index" + n].Show();
                    container.Controls["mailDate" + n].Text = mail.Fecha.ToString("dd/MM/yyyy, HH:mm");
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
                    container.Controls["mailDate" + n].Text = message.Fecha_creacion.ToString("dd/MM/yyyy, HH:mm");
                    index++;
                }
            }
        }

        protected override void resetPanels()
        {
            base.resetPanels();
            for (int i = 0; i < dataMails.RowCount; i++) dataMails.Rows[i].Selected = false;
        }

        private void seleccionarMail(int numero)
        {
            seleccionar_elemento(numero, "index", "panel", dataMails);
        }

        public void leerMail(int numero) {
            if ((new Label[] {index1, index2, index3, index4, index5, index6, index7, index8})[numero - 1].Visible)
            {
                mail_enviado message;
                if (recibidos)
                {
                    mail_recibido messageRecibido = messagesRecibidos[numero - 1];
                    message = new mail_enviado();
                    message.__mensaje = messageRecibido.Mensaje;
                    message.__para = messageRecibido.Remitente_mail;
                    message.__asunto = messageRecibido.Asunto;
                    message.__fecha_creacion = messageRecibido.Fecha;
                    message.__uidl = messageRecibido.UIDL;
                }
                else
                    message = messagesEnviados[numero - 1];

                leer_mail form = new leer_mail(message, this);
                form.Show();
            }
        }

        private void btnAnterior_EnabledChanged(object sender, System.EventArgs e)
        {
            btnAnterior.ForeColor = sender.Equals(false) ? Color.Blue : Color.Red;
        }

        public bool eliminar_mail (string UIDL) {
            Cargando carg = new Cargando();
            carg.Ejecutar();
            
            client = G.crear_cliente(); 
            Pop3MessageUIDInfoCollection messageUIDs = client.GetAllUIDMessages();
            uint i = 1;
            foreach (Pop3MessageUIDInfo uidInfo in messageUIDs)
            {
                if (uidInfo.UniqueNumber == UIDL)
                {
                    client.DeleteMessage(i);
                    break;
                }
                i++;
            }
            client.Logout();
            
            G.user.eliminar_mail_recibido(UIDL);

            client.Logout();
            if (((G.user.cantidad_mails_recibidos() % 8) == 1) && btnAnterior.Enabled)
                btnAnterior_Click(null, EventArgs.Empty);
            else
                actualizar_vista();

            carg.Detener();
            return true;
        }

        public void eliminar_mail (int ID) {
            G.user.eliminar_mail_enviado(ID);
          if (((G.user.cantidad_mails_enviados() % 8) == 1) && btnAnterior.Enabled)
            btnAnterior_Click(null, EventArgs.Empty);
          else
              actualizar_vista();
        }

        private void trackAutenticacion_Scroll(object sender, EventArgs e)
        {
            G.sensibilidad_autenticacion = trackAutenticacion.Value;
            lblPer.Text = "(" + G.sensibilidad_autenticacion.ToString() + "%)";
        }

        private void trackComando_Scroll(object sender, EventArgs e)
        {
            G.sensibilidad = trackComando.Value;
            lblComando.Text = "(" + G.sensibilidad.ToString() + "%)";
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            pictureBox10.Visible = false;
            pictureBox11.Visible = true;
            G.comando_form.actualizar_estado_microfono(false);

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            pictureBox11.Visible = false;
            pictureBox10.Visible = true;
            G.comando_form.actualizar_estado_microfono(true);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            new iniciar_sesion.iniciar_sesion().Show();
        }

        private void btnHistorial_Click (object sender, EventArgs e) {
          int actual = 0;
          while (mostrar_historial(actual, 20)) actual += 20;
        }

        private bool mostrar_historial (int inicio, int cantidad) {
          int rc = 0;
          string historial = "";
          SQLiteCommand stmt = new SQLiteCommand(G.conexion_principal);
          stmt.CommandText = "SELECT hora, comando, conf_rec, conf_aut FROM Comando WHERE Usuario = ? ORDER BY hora DESC LIMIT ?, ?";
          SQLiteParameter param = new SQLiteParameter();
          stmt.Parameters.Add(param);
          param.Value = G.user.ID;
          param = new SQLiteParameter();
          stmt.Parameters.Add(param);
          param.Value = inicio;
          param = new SQLiteParameter();
          stmt.Parameters.Add(param);
          param.Value = cantidad;
          SQLiteDataReader dr = stmt.ExecuteReader();
          while (dr.Read()) {
            if (rc != 0) historial += Environment.NewLine;
            rc ++;
            historial += linea_historial(dr.GetInt64(0), dr.GetString(1), dr.GetDouble(2), dr.GetDouble(3));
          }
          dr.Close();
          dr.Dispose();
          stmt.Dispose();
          if (rc == 0) {
            MessageBox.Show((inicio == 0) ? "No hay comandos en el historial" : "No hay más entradas en el historial",
                            "Historial de comandos de voz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
          } else if (rc < cantidad) {
            MessageBox.Show(historial, "Historial de comandos de voz", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
          }
          return MessageBox.Show(
            historial + Environment.NewLine + Environment.NewLine + "¿Desea ver las siguientes " + cantidad.ToString() + " entradas?",
            "Historial de comandos de voz", MessageBoxButtons.YesNo, MessageBoxIcon.Information
          ) == DialogResult.Yes;
        }

        private string linea_historial (long hora, string comando, double confianza_reconocimiento, double confianza_autenticacion) {
          return string.Format(CultureInfo.InvariantCulture, "[{0:d/M/yyy H:mm:ss}] {1} ({2}), {3}",
                               DateTime.FromFileTimeUtc(hora).ToLocalTime(), comando,
                               convertir_numero_a_porcentaje(confianza_reconocimiento),
                               (confianza_autenticacion < -1) ? "error de autenticación" :
                               string.Format("{0} ({1})", (confianza_autenticacion > 0) ? "autenticado" : "denegado",
                                                           convertir_numero_a_porcentaje(Math.Abs(confianza_autenticacion))
                               )
          );
        }

        private string convertir_numero_a_porcentaje (double numero) {
          if (numero < 0) return "-" + convertir_numero_a_porcentaje(-numero);
          int p = (int) (numero * 10000 + 0.5);
          return string.Format("{0},{1:00}%", p / 100, p % 100);
        }
    }
}
