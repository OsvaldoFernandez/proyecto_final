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


namespace clienteMail
{
    public partial class leer_mail : Form
    {
        public leer_mail(Rfc822Message message)
        {
            InitializeComponent();
            txtMail.Text = message.Text.ToString();
            // proximamente: que lea como html!
        }

        private void leer_mail_Load(object sender, EventArgs e)
        {

        }
    }
}
