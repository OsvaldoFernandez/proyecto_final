using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace clienteMail.crear_cuenta
{
    public partial class crear_cuenta : Form
    {
        Color colorFondo = Color.FromArgb(61, 183, 248);
        public crear_cuenta()
        {
            InitializeComponent();
        }

        private void crear_cuenta_Load(object sender, EventArgs e)
        {
            SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
            cmd.CommandText = "SELECT proveedor FROM Proveedor_mail";
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read()) proveedor.Items.Add(dr.GetString(0));
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            proveedor.Items.Add("Otro");
            proveedor.Text = "Seleccione un proveedor";
            puertopop3.Enabled = false;
            puertosmtp.Enabled = false;
            servidorpop3.Enabled = false;
            servidorsmtp.Enabled = false;

            this.resetPanels();
        }

        private void resetPanels()
        {
            panel1.BackColor = colorFondo;
            panel2.BackColor = colorFondo;
            panel3.BackColor = colorFondo;
            panel4.BackColor = colorFondo;
            panel5.BackColor = colorFondo;
            panel6.BackColor = colorFondo;
            panel7.BackColor = colorFondo;
            lblError.Visible = false;
        }

        private bool errorForm()
        {
            bool error = false;
            this.resetPanels();

            if (proveedor.SelectedItem == null)
            {
                error = true;
                panel1.BackColor = Color.Red;
            }

            Regex reg = new Regex(@"^[^ /?@\x00-\x1f()<>]+@([^. /?@\x00-\x1f()<>]+\.)*[a-zA-Z]{2,}\.?$");
            if (!reg.IsMatch(mail.Text))
            {
                error = true;
                panel3.BackColor = Color.Red;
            }

            if (contrasena.Text.Length == 0)
            {
                error = true;
                panel2.BackColor = Color.Red;
            }

            if ((string)proveedor.SelectedItem == "Otro")
            {
                //valido tambien los campos del "otro" proveedor
                if (servidorpop3.Text.Length == 0)
                {
                    error = true;
                    panel4.BackColor = Color.Red;
                }
                
                ushort puerto;
                bool excpop3 = false;
                try{
                    puerto = ushort.Parse(puertopop3.Text);
                } catch{
                    excpop3 =true;
                }

                if (puertopop3.Text.Length == 0 || excpop3)
                {
                    error = true;
                    panel5.BackColor = Color.Red;
                }
                if (servidorsmtp.Text.Length == 0)
                {
                    error = true;
                    panel6.BackColor = Color.Red;
                }

                bool excsmtp = false;
                try
                {
                    puerto = ushort.Parse(puertosmtp.Text);
                }
                catch
                {
                    excsmtp = true;
                }

                if (puertosmtp.Text.Length == 0 || excsmtp)
                {
                    error = true;
                    panel7.BackColor = Color.Red;
                }
            }

            return error;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string servidorPOP3, servidorSMTP;
            ushort puertoPOP3, puertoSMTP;
            bool sslPOP3, sslSMTP;
            SQLiteCommand cmd = null;

            if (this.errorForm())
            {
                lblError.Visible = true;
                return;
            }

            else if ((string)proveedor.SelectedItem == "Otro")
            {
                servidorPOP3 = servidorpop3.Text;
                servidorSMTP = servidorsmtp.Text;
                puertoPOP3 = ushort.Parse(puertopop3.Text);
                puertoSMTP = ushort.Parse(puertosmtp.Text);
                sslPOP3 = sslpop3.Checked;
                sslSMTP = sslsmtp.Checked;
            }
            else
            {
                cmd = new SQLiteCommand(G.conexion_principal);
                cmd.CommandText = "SELECT servidor_pop3, puerto_pop3, ssl_pop3, servidor_smtp, puerto_smtp, ssl_smtp " +
                                  "FROM Proveedor_mail WHERE proveedor = ?";
                SQLiteParameter param = new SQLiteParameter();
                cmd.Parameters.Add(param);
                param.Value = proveedor.SelectedItem.ToString();
                SQLiteDataReader dr = cmd.ExecuteReader();
                dr.Read();
                servidorPOP3 = dr.GetString(0);
                servidorSMTP = dr.GetString(3);
                puertoPOP3 = (ushort)dr.GetInt16(1);
                puertoSMTP = (ushort)dr.GetInt16(4);
                sslPOP3 = dr.GetBoolean(2);
                sslSMTP = dr.GetBoolean(5);
                dr.Close();
                dr.Dispose();
                cmd.Dispose();
            }
            cmd = new SQLiteCommand(G.conexion_principal);
            cmd.CommandText = "INSERT INTO Usuario (servidor_smtp, servidor_pop3, puerto_smtp, puerto_pop3, ssl_smtp, ssl_pop3, " + 
                              "mail, contrasena) VALUES (?, ?, ?, ?, ?, ?, ?, ?);";
            SQLiteParameter paramServidorSMTP = new SQLiteParameter();
            cmd.Parameters.Add(paramServidorSMTP);
            paramServidorSMTP.Value = servidorSMTP;
            SQLiteParameter paramServidorPOP3 = new SQLiteParameter();
            cmd.Parameters.Add(paramServidorPOP3);
            paramServidorPOP3.Value = servidorPOP3;
            SQLiteParameter paramPuertoSMTP = new SQLiteParameter();
            cmd.Parameters.Add(paramPuertoSMTP);
            paramPuertoSMTP.Value = puertoSMTP;
            SQLiteParameter paramPuertoPOP3 = new SQLiteParameter();
            cmd.Parameters.Add(paramPuertoPOP3);
            paramPuertoPOP3.Value = puertoPOP3;
            SQLiteParameter paramSSLSMTP = new SQLiteParameter();
            cmd.Parameters.Add(paramSSLSMTP);
            paramSSLSMTP.Value = sslSMTP ? 1 : 0;
            SQLiteParameter paramSSLPOP3 = new SQLiteParameter();
            cmd.Parameters.Add(paramSSLPOP3);
            paramSSLPOP3.Value = sslPOP3 ? 1 : 0;
            SQLiteParameter paramMail = new SQLiteParameter();
            cmd.Parameters.Add(paramMail);
            paramMail.Value = mail.Text;
            SQLiteParameter paramContrasena = new SQLiteParameter();
            cmd.Parameters.Add(paramContrasena);
            paramContrasena.Value = contrasena.Text;
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            // CONSIGO MI ID, seteo mi global user
            SQLiteCommand cmd2 = new SQLiteCommand(G.conexion_principal);
            cmd2.CommandText = "select id, servidor_smtp, servidor_pop3, puerto_smtp, puerto_pop3 from usuario where mail == ?";
            SQLiteParameter paramMail2 = new SQLiteParameter();
            cmd2.Parameters.Add(paramMail2);
            paramMail2.Value = mail.Text;
            SQLiteDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                G.user = new User(dr2.GetInt16(0));
                RichForm form1 = new entrenamiento.entrenamiento_1(G.user.ID.ToString() + ".pav");
                form1.Show();
            }
        }

        private void proveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (proveedor.SelectedItem.ToString() == "Otro")
            {
                puertopop3.Enabled = true;
                puertosmtp.Enabled = true;
                servidorpop3.Enabled = true;
                servidorsmtp.Enabled = true;
                this.Width = 820;
                logoPic.Location = new Point(316, -35);
            }
            else
            {
                puertopop3.Enabled = false;
                puertosmtp.Enabled = false;
                servidorpop3.Enabled = false;
                servidorsmtp.Enabled = false;
                this.Width = 420;
                logoPic.Location = new Point(116, -35);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
            new iniciar_sesion.iniciar_sesion().Show();
        }

    }
}
