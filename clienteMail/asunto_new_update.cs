using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace clienteMail
{
    public partial class asunto_new_update : Form
    {
        private int ID = 0;
        public asunto_new_update(int id)
        {
            InitializeComponent();

            if (id > 0)
            {
                Asunto asunto = G.user.getAsunto(id);
                txtTexto.Text = asunto.Texto;

                this.Text = "Modificar asunto";
            }
            ID = id;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //validar.
            Asunto asunto = new Asunto();
            asunto.__texto = txtTexto.Text;

            if (ID == 0) //agregue
            {
                G.user.agregar_asunto(asunto);
            }
            else //modifique
            {
                asunto.__id = ID;
                G.user.modificar_asunto(asunto);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

    }
}
