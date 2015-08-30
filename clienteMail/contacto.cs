using System;
using System.Data.SQLite;

public class Contacto
{
    public string __mail, __nombre, __apellido;
    public int __id, __usuario_id, __cant_enviados, __cant_recibidos;
    public DateTime __fecha_creacion;

    public string Nombre { get { return __nombre; } }
    public string Apellido { get { return __apellido; } }
    public string Mail { get { return __mail; } }
    public int ID { get { return __id; } }
    
    public int Usuario_id { get { return __usuario_id; } }
    public int Cant_enviados { get { return __cant_enviados; } }
    public int Cant_recibidos { get { return __cant_recibidos; } }
    public DateTime Fecha_creacion { get { return __fecha_creacion; } }
    
}