using System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Threading;
using System.Speech.Recognition;

public partial class RichForm : Form
{
    public RichForm form_padre;

    public virtual void manejar_comando(string comando) {}
    public virtual void manejar_comando_entrenamiento(SpeechRecognizedEventArgs e) {}
    public virtual void agregar_contacto(int id) {}
    public virtual void agregar_asunto(int id) {}
    public virtual void agregar_mensaje(int id) {}
    public virtual void manejar_aceptar(string contexto) {}
    public virtual void manejar_cerrar(string contexto) {}
}

public static class G
{
    public static SQLiteConnection conexion_principal = null;

    public static User user = null;
    public static int sensibilidad = 50;
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
}