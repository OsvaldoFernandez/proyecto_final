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
    public partial class splashScreen : Form
    {
        public splashScreen()
        {
            InitializeComponent();
        }

        private void timer1_Tick (object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            if (progressBar1.Value == 100) progressBar1.Value = 0;
        }

        private void splashScreen_Load (object sender, EventArgs e)
        {
            timer1.Start();
            timer1_Tick(null, EventArgs.Empty);
        }
    }
}
