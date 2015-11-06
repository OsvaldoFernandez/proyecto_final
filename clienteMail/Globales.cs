using System;
using System.Data.SQLite;
using Email.Net;
using Email.Net.Common;
using Email.Net.Pop3;
using Email.Net.Common.Configurations;
using Email.Net.Common.Collections;
using Email.Net.Pop3.Exceptions;

public static class G
{
    public static SQLiteConnection conexion_principal = null;

    public static User user = null;
    public static int sensibilidad = 75;
    public static int sensibilidad_autenticacion = 0;
    public static int confianza_autenticacion;
    public static RichForm comando_form;

    public static SQLiteConnection abrir_conexion(string ubicacion, bool solo_lectura)
    {
        var conexion = new SQLiteConnection(String.Format("Data Source={0}; FailIfMissing=true; Read Only={1}", ubicacion,
                                                          solo_lectura ? "true" : "false"));
        conexion.Open();
        ejecutar_en_base(conexion, "PRAGMA case_sensitive_like = 1");
        return conexion;
    }

    public static void ejecutar_en_base(SQLiteConnection conexion, string consulta)
    {
        SQLiteCommand cmd = new SQLiteCommand(consulta, conexion);
        cmd.ExecuteNonQuery();
        cmd.Dispose();
    }

    public static Pop3Client crear_cliente()
    {
        Pop3Client client = new Pop3Client(G.user.POP3server, G.user.POP3port, G.user.Mail, G.user.Password);
        
        #if DEBUG
                client.Connected += ((Pop3Client c) => Console.WriteLine("Cliente conectado"));
                client.Authenticated += ((Pop3Client c) => Console.WriteLine("Cliente autenticado"));
                client.MessageReceived += ((Pop3Client c, Rfc822Message m) =>
                    Console.WriteLine("Mensaje recibido: {0}", m.Subject));
                client.Completed += ((Pop3Client c) => Console.WriteLine("Operacion completada"));
                client.Quit += ((Pop3Client c) => Console.WriteLine("Cliente cerrado"));
                client.BrokenMessage += ((Pop3Client c, Pop3MessageInfo i, string err, Rfc822Message m) =>
                    Console.WriteLine("Mensaje {0} no valido: {1}", i.Number, err));
                client.MessageDeleted += ((Pop3Client c, uint n) => Console.WriteLine("Mensaje {0} borrado", n));
        #endif

        client.SSLInteractionType = user.POP3ssl ? EInteractionType.SSLPort : EInteractionType.Plain;

        client.Login();

        return client;
    }
}