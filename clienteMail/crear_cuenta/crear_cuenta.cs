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
            cmd.CommandText = "SELECT proveedor, servidor_pop3, puerto_pop3, servidor_smtp, puerto_smtp FROM Proveedor_mail";
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proveedor.Items.Add(dr.GetString(0));
            }
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
            string servidorPOP3;
            string servidorSMTP;
            ushort puertoPOP3;
            ushort puertoSMTP;
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
            }
            else
            {
                cmd = new SQLiteCommand(G.conexion_principal);
                cmd.CommandText = "select id, proveedor, servidor_pop3, puerto_pop3, servidor_smtp, puerto_smtp from Proveedor_mail where proveedor==?;";
                SQLiteParameter param = new SQLiteParameter();
                cmd.Parameters.Add(param);
                param.Value = proveedor.SelectedItem.ToString();
                SQLiteDataReader dr = cmd.ExecuteReader();
                dr.Read();
                servidorPOP3 = dr.GetString(2);
                servidorSMTP = dr.GetString(4);
                puertoPOP3 = (ushort) dr.GetInt16(3);
                puertoSMTP = (ushort) dr.GetInt16(5);
                dr.Close();
                dr.Dispose();
                cmd.Dispose();
            }
            cmd = new SQLiteCommand(G.conexion_principal);
            cmd.CommandText = "INSERT INTO Usuario (servidor_smtp, servidor_pop3, puerto_smtp, puerto_pop3, mail, contrasena)  VALUES (?, ?, ?, ?, ?, ?);";
            SQLiteParameter paramServidorSMTP= new SQLiteParameter();
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
            SQLiteParameter paramMail = new SQLiteParameter();
            cmd.Parameters.Add(paramMail);
            paramMail.Value = mail.Text;
            SQLiteParameter paramContrasena = new SQLiteParameter();
            cmd.Parameters.Add(paramContrasena);
            paramContrasena.Value = contrasena.Text;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
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
