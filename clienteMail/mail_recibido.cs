using System;
using System.Data.SQLite;

public class mail_recibido
{
    public string __mensaje, __asunto, __remitente_nombre, __remitente_mail, __uidl;
    public int __id, __usuario_id;
    public DateTime __fecha;

    public string Mensaje { get { return __mensaje; } }
    public string Asunto { get { return __asunto; } }
    public string Remitente_nombre { get { return __remitente_nombre; } }
    public string Remitente_mail { get { return __remitente_mail; } }
    public int ID { get { return __id; } }
    public string UIDL { get { return __uidl; } }

    public int Usuario_id { get { return __usuario_id; } }
    public DateTime Fecha { get { return __fecha; } }
}