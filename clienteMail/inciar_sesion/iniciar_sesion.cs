using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace clienteMail.iniciar_sesion {
    public partial class iniciar_sesion : Form {
        public iniciar_sesion() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
            cmd.CommandText = "select id, servidor_smtp, servidor_pop3, puerto_smtp, puerto_pop3, perfil from usuario where mail == ? and contrasena == ? ";
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
                if (dr.IsDBNull(5)) {
                    // NO HAY UN PERFIL PAV PARA ESTA PERSONA.. DEBER DE ENTRENAR
                    RichForm form1 = new entrenamiento.entrenamiento_1(G.user.ID.ToString() + ".pav");
                    form1.Show();
                } else {
                    //TENIA UN PERFIL, ENTONCES LO SETEO COMO GLOBAL DEL USER PARA QUE COMANDO LO USE
                    G.user.PAV = dr.GetString(5);
                    RichForm formulario_activo = new Form1();
                    formulario_activo.Show();
                    G.comando_form = new comando.comando();
                    #if DEBUG
                      G.comando_form.Show();
                    #endif
                }
                this.Hide();
            }
            else
                MessageBox.Show("Usuario o contraseña inválidos");
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
        }

        private void button2_Click(object sender, EventArgs e) {
            new crear_cuenta.crear_cuenta().Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e) {
            G.crear_form_comando();
            G.comando_form.Show();
        }
    }
}
