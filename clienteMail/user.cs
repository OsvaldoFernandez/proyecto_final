using System;
using System.Data.SQLite;

public class User
{
    private string __pop3server, __smtpserver;
    private ushort __pop3port, __smtpport;
    private string __mail, __password;
    private int __id;

    public string POP3server { get { return __pop3server; } }
    public string SMTPserver { get { return __smtpserver; } }
    public ushort POP3port { get { return __pop3port; } }
    public ushort SMTPport { get { return __smtpport; } }
    public string Mail { get { return __mail; } }
    public string Password { get { return __password; } }
    public int ID { get { return __id; } }

    public User(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "SELECT servidor_pop3, puerto_pop3, servidor_smtp, puerto_smtp, mail, contrasena FROM Usuario WHERE id = ?";
        SQLiteParameter param = new SQLiteParameter();
        cmd.Parameters.Add(param);
        param.Value = id;
        SQLiteDataReader dr = cmd.ExecuteReader();
        try
        {
            if (!dr.Read()) throw new Exception("...");
            __id = id;
            __pop3server = dr.GetString(0);
            __pop3port = (ushort) dr.GetInt16(1);
            __smtpserver = dr.GetString(2);
            __smtpport = (ushort) dr.GetInt16(3);
            __mail = dr.GetString(4);
            __password = dr.GetString(5);
        }
        finally
        {
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
        }
    }
}