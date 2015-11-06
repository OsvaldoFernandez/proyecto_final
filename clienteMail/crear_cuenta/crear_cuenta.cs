using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace clienteMail.crear_cuenta
{
    public partial class crear_cuenta : Form
    {
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
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string servidorPOP3, servidorSMTP;
            ushort puertoPOP3, puertoSMTP;
            bool sslPOP3, sslSMTP;
            SQLiteCommand cmd = null;
            if (proveedor.SelectedItem == null)
            {
                MessageBox.Show("Proveedor inválido");
                return;
            }
            else if ((string) proveedor.SelectedItem == "Otro")
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
                puertoPOP3 = (ushort) dr.GetInt16(1);
                puertoSMTP = (ushort) dr.GetInt16(4);
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
            RichForm form1 = new entrenamiento.entrenamiento_1("perfil.pav");
            form1.Show();
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

    }
}
