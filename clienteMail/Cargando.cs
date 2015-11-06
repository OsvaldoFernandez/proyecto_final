using System;
using System.Threading;
using System.Windows.Forms;
using clienteMail;

public class Cargando {
    private bool detener;
    private Thread thread = null;

    public Cargando()
    {
        detener = true;
    }

    public void Ejecutar ()
    {
        detener = false;
        thread = new Thread(
            () =>
            {
                splashScreen f = new splashScreen();
                f.Show();
                while (!this.detener)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
                f.Close();
                f.Dispose();
            }
        );
        thread.Start();
    }

    public void Detener()
    {
        if (detener) return;
        detener = true;
        bool st = thread.Join(1000);
        if (!st) thread.Abort();
    }
}