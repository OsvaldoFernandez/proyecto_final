using System;
using System.Data.SQLite;

public class mail_enviado
{
    public string __mensaje, __asunto, __para, __from;
    public int __id, __usuario_id;
    public DateTime __fecha_creacion;

    public string Mensaje { get { return __mensaje; } }
    public string Asunto { get { return __asunto; } }
    public string Para { get { return __para; } }
    public string From { get { return __from; } }
    public int ID { get { return __id; } }

    public int Usuario_id { get { return __usuario_id; } }
    public DateTime Fecha_creacion { get { return __fecha_creacion; } }
}
