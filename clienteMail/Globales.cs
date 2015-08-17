using System;
using System.Data.SQLite;

public static class G
{
    public static SQLiteConnection conexion_principal = null;

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