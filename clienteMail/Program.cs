using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace clienteMail
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            G.conexion_principal = G.abrir_conexion("clienteMail.db3", false);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
