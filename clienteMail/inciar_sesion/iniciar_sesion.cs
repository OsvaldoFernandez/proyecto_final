﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace clienteMail.iniciar_sesion
{
    public partial class iniciar_sesion : Form
    {
        public iniciar_sesion()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
            cmd.CommandText = "select id, servidor_smtp, servidor_pop3, puerto_smtp, puerto_pop3 from usuario where mail == ? and contrasena == ? ";
            SQLiteParameter paramMail = new SQLiteParameter();
            cmd.Parameters.Add(paramMail);
            paramMail.Value = usertxt.Text;
            SQLiteParameter paramPass = new SQLiteParameter();
            cmd.Parameters.Add(paramPass);
            paramPass.Value = passtxt.Text;
            SQLiteDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                G.user = new User(dr.GetInt16(0));
                // PENDIENTE: Buscar un .pav que se llame como mi cuenta de mail, si aparece, entonces entrené y abro form1
                RichForm formulario_activo = new Form1();
                formulario_activo.Show();
                G.comando_form = new comando.comando();
                G.comando_form.Show();
                // Si no apareció mi .pav, además de abrir comando, debo abrir entrenamiento
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña inválidos");
            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new crear_cuenta.crear_cuenta().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            G.comando_form = new comando.comando();
            G.comando_form.Show();
        }
    }
}
