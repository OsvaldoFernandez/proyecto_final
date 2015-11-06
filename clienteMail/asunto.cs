using System;
using System.Data.SQLite;

public class Asunto
{
    public string __texto;
    public int __id, __usuario_id, __cant_veces_usado;
    public DateTime __fecha_creacion;

    public string Texto { get { return __texto; } }
    public int ID { get { return __id; } }

    public int Usuario_id { get { return __usuario_id; } }
    public int Cant_veces_usado { get { return __cant_veces_usado; } }
    public DateTime Fecha_creacion { get { return __fecha_creacion; } }

}