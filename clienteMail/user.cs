using System;
using System.Collections;
using System.Collections.Generic;
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

    public Contacto getContacto(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "SELECT nombre, apellido, mail, cant_enviados, cant_recibidos FROM Contacto WHERE id = ?";
        SQLiteParameter param = new SQLiteParameter();
        cmd.Parameters.Add(param);
        param.Value = id;
        SQLiteDataReader dr = cmd.ExecuteReader();

        Contacto contacto = new Contacto();

        try
        {
            if (!dr.Read()) throw new Exception("...");
            
            contacto.__nombre = dr.GetString(0);
            contacto.__apellido = dr.GetString(1);
            contacto.__mail = dr.GetString(2);
            contacto.__cant_enviados = dr.GetInt32(3);
            contacto.__cant_recibidos = dr.GetInt32(4);
            contacto.__id = id;
        }
        finally
        {
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            
        }

        return contacto;
    }

    public Contacto[] contactos()
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "SELECT nombre, apellido, mail, cant_enviados, cant_recibidos, id FROM Contacto WHERE usuario_id = ? order by cant_enviados desc, cant_recibidos desc, fecha_creacion desc";
        SQLiteParameter param = new SQLiteParameter();
        cmd.Parameters.Add(param);
        param.Value = this.ID;
        SQLiteDataReader dr = cmd.ExecuteReader();

        List<Contacto> lista_contactos = new List<Contacto>();

        while (dr.Read())
        {
            Contacto contacto = new Contacto();
            contacto.__nombre = dr.GetString(0);
            contacto.__apellido = dr.GetString(1);
            contacto.__mail = dr.GetString(2);
            contacto.__cant_enviados = dr.GetInt32(3);
            contacto.__cant_recibidos = dr.GetInt32(4);
            contacto.__id = dr.GetInt32(5);
            lista_contactos.Add(contacto);
        }

        dr.Close();
        dr.Dispose();
        cmd.Dispose();

        return lista_contactos.ToArray();
    }

    public Contacto[] contactosPag(int nro)
    {
        List<Contacto> lista_contactos_pag = new List<Contacto>();
        Contacto[] arrayContactos = this.contactos();

        int contacto_desde = (nro - 1) * 8;
        int contacto_hasta;
        if (nro * 8 < arrayContactos.Length)
        {
            contacto_hasta = nro * 8 -1;
        }
        else
        {
            contacto_hasta = arrayContactos.Length-1;
        }

        for (int i = contacto_desde; i <= contacto_hasta; i++)
        {

            lista_contactos_pag.Add(arrayContactos[i]);
        }


        return lista_contactos_pag.ToArray();

    }

    public void agregar_contacto(Contacto contacto)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "insert into Contacto(usuario_id, nombre, apellido, mail, cant_enviados, cant_recibidos, fecha_creacion) values(?, ? , ?, ?, 0,0, date('now'))";
        
        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = this.ID;
        
        SQLiteParameter paramNombre = new SQLiteParameter();
        cmd.Parameters.Add(paramNombre);
        paramNombre.Value = contacto.Nombre;

        SQLiteParameter paramApellido = new SQLiteParameter();
        cmd.Parameters.Add(paramApellido);
        paramApellido.Value = contacto.Apellido;

        SQLiteParameter paramMail = new SQLiteParameter();
        cmd.Parameters.Add(paramMail);
        paramMail.Value = contacto.Mail;

        cmd.ExecuteNonQuery();
        
        cmd.Dispose();

    }

    public void modificar_contacto(Contacto contacto)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "update Contacto set nombre = ?, apellido = ?, mail = ? where id = ?";

        SQLiteParameter paramNombre = new SQLiteParameter();
        cmd.Parameters.Add(paramNombre);
        paramNombre.Value = contacto.Nombre;

        SQLiteParameter paramApellido = new SQLiteParameter();
        cmd.Parameters.Add(paramApellido);
        paramApellido.Value = contacto.Apellido;

        SQLiteParameter paramMail = new SQLiteParameter();
        cmd.Parameters.Add(paramMail);
        paramMail.Value = contacto.Mail;

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = contacto.ID;

        cmd.ExecuteNonQuery();

        cmd.Dispose();

    }

    public void eliminar_contacto(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "delete from Contacto  where id = ?";

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = id;

        cmd.ExecuteNonQuery();

        cmd.Dispose();

    }

    public Asunto getAsunto(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "SELECT usuario_id, texto, cant_veces_usado FROM Asunto WHERE id = ?";
        SQLiteParameter param = new SQLiteParameter();
        cmd.Parameters.Add(param);
        param.Value = id;
        SQLiteDataReader dr = cmd.ExecuteReader();

        Asunto asunto = new Asunto();

        try
        {
            if (!dr.Read()) throw new Exception("...");

            asunto.__id = dr.GetInt32(0);
            asunto.__texto = dr.GetString(1);
            asunto.__cant_veces_usado = dr.GetInt32(2);
            asunto.__id = id;
        }
        finally
        {
            dr.Close();
            dr.Dispose();
            cmd.Dispose();

        }

        return asunto;
    }

    public Asunto[] asuntos()
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "SELECT texto, id FROM Asunto WHERE usuario_id = ? order by cant_veces_usado desc, fecha_creacion desc";
        SQLiteParameter param = new SQLiteParameter();
        cmd.Parameters.Add(param);
        param.Value = this.ID;
        SQLiteDataReader dr = cmd.ExecuteReader();

        List<Asunto> lista_asuntos = new List<Asunto>();

        while (dr.Read())
        {
            Asunto asunto = new Asunto();
            asunto.__texto = dr.GetString(0);
            asunto.__id = dr.GetInt32(1);
            lista_asuntos.Add(asunto);
        }

        dr.Close();
        dr.Dispose();
        cmd.Dispose();

        return lista_asuntos.ToArray();
    }

    public Asunto[] asuntosPag(int nro)
    {
        List<Asunto> lista_asuntos_pag = new List<Asunto>();
        Asunto[] arrayAsuntos = this.asuntos();

        int asunto_desde = (nro - 1) * 8;
        int asunto_hasta;
        if (nro * 8 < arrayAsuntos.Length)
        {
            asunto_hasta = nro * 8 - 1;
        }
        else
        {
            asunto_hasta = arrayAsuntos.Length - 1;
        }

        for (int i = asunto_desde; i <= asunto_hasta; i++)
        {
            lista_asuntos_pag.Add(arrayAsuntos[i]);
        }


        return lista_asuntos_pag.ToArray();

    }

    public void agregar_asunto(Asunto asunto)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "insert into Asunto(usuario_id, texto, cant_veces_usado, fecha_creacion) values(?, ? , 0, date('now'))";

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = this.ID;

        SQLiteParameter paramNombre = new SQLiteParameter();
        cmd.Parameters.Add(paramNombre);
        paramNombre.Value = asunto.Texto;

        cmd.ExecuteNonQuery();

        cmd.Dispose();
    }

    public void modificar_asunto(Asunto asunto)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "update Asunto set texto= ? where id = ?";

        SQLiteParameter paramNombre = new SQLiteParameter();
        cmd.Parameters.Add(paramNombre);
        paramNombre.Value = asunto.Texto;

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = asunto.ID;

        cmd.ExecuteNonQuery();

        cmd.Dispose();

    }

    public void eliminar_asunto(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "delete from Asunto  where id = ?";

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = id;

        cmd.ExecuteNonQuery();

        cmd.Dispose();
    }

    public Mensaje getMensaje(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "SELECT usuario_id, texto, cant_veces_usado FROM Mensaje WHERE id = ?";
        SQLiteParameter param = new SQLiteParameter();
        cmd.Parameters.Add(param);
        param.Value = id;
        SQLiteDataReader dr = cmd.ExecuteReader();

        Mensaje mensaje = new Mensaje();

        try
        {
            if (!dr.Read()) throw new Exception("...");

            mensaje.__id = dr.GetInt32(0);
            mensaje.__texto = dr.GetString(1);
            mensaje.__cant_veces_usado = dr.GetInt32(2);
            mensaje.__id = id;
        }
        finally
        {
            dr.Close();
            dr.Dispose();
            cmd.Dispose();

        }

        return mensaje;
    }

    public Mensaje[] mensajes()
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "SELECT texto, id FROM Mensaje WHERE usuario_id = ? order by cant_veces_usado desc, fecha_creacion desc";
        SQLiteParameter param = new SQLiteParameter();
        cmd.Parameters.Add(param);
        param.Value = this.ID;
        SQLiteDataReader dr = cmd.ExecuteReader();

        List<Mensaje> lista_mensajes = new List<Mensaje>();

        while (dr.Read())
        {
            Mensaje mensaje = new Mensaje();
            mensaje.__texto = dr.GetString(0);
            mensaje.__id = dr.GetInt32(1);
            lista_mensajes.Add(mensaje);
        }

        dr.Close();
        dr.Dispose();
        cmd.Dispose();

        return lista_mensajes.ToArray();
    }

    public Mensaje[] mensajesPag(int nro)
    {
        List<Mensaje> lista_mensajes_pag = new List<Mensaje>();
        Mensaje[] arrayMensajes = this.mensajes();

        int mensaje_desde = (nro - 1) * 8;
        int mensaje_hasta;
        if (nro * 8 < arrayMensajes.Length)
        {
            mensaje_hasta = nro * 8 - 1;
        }
        else
        {
            mensaje_hasta = arrayMensajes.Length - 1;
        }

        for (int i = mensaje_desde; i <= mensaje_hasta; i++)
        {
            lista_mensajes_pag.Add(arrayMensajes[i]);
        }


        return lista_mensajes_pag.ToArray();

    }

    public void agregar_mensaje(Mensaje mensaje)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "insert into Mensaje(usuario_id, texto, cant_veces_usado, fecha_creacion) values(?, ? , 0, date('now'))";

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = this.ID;

        SQLiteParameter paramTexto = new SQLiteParameter();
        cmd.Parameters.Add(paramTexto);
        paramTexto.Value = mensaje.Texto;

        cmd.ExecuteNonQuery();

        cmd.Dispose();
    }

    public void modificar_mensaje(Mensaje mensaje)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "update Mensaje set texto= ? where id = ?";

        SQLiteParameter paramTexto = new SQLiteParameter();
        cmd.Parameters.Add(paramTexto);
        paramTexto.Value = mensaje.Texto;

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = mensaje.ID;

        cmd.ExecuteNonQuery();

        cmd.Dispose();

    }

    public void eliminar_mensaje(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "delete from Mensaje where id = ?";

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = id;

        cmd.ExecuteNonQuery();

        cmd.Dispose();
    }

    public void contacto_enviado(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "update Contacto set cant_enviados = cant_enviados + 1 where id = ?";

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = id;

        cmd.ExecuteNonQuery();

        cmd.Dispose();
    }

    public void asunto_usado(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "update Asunto set cant_veces_usado = cant_veces_usado + 1 where id = ?";

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = id;

        cmd.ExecuteNonQuery();

        cmd.Dispose();
    }

    public void mensaje_usado(int id)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "update Mensaje set cant_veces_usado = cant_veces_usado + 1 where id = ?";

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = id;

        cmd.ExecuteNonQuery();

        cmd.Dispose();
    }

    public void guardarMail(mail_enviado mail)
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "insert into Mails_enviados(usuario_id, para, asunto, mensaje, fecha_creacion) values(?, ?, ?, ?, datetime('now'))";

        SQLiteParameter paramId = new SQLiteParameter();
        cmd.Parameters.Add(paramId);
        paramId.Value = this.ID;

        SQLiteParameter paramPara = new SQLiteParameter();
        cmd.Parameters.Add(paramPara);
        paramPara.Value = mail.Para;

        SQLiteParameter paramAsunto = new SQLiteParameter();
        cmd.Parameters.Add(paramAsunto);
        paramAsunto.Value = mail.Asunto;

        SQLiteParameter paramMensaje = new SQLiteParameter();
        cmd.Parameters.Add(paramMensaje);
        paramMensaje.Value = mail.Mensaje;

        cmd.ExecuteNonQuery();

        cmd.Dispose();
    }


    public mail_enviado[] mailsEnviados()
    {
        SQLiteCommand cmd = new SQLiteCommand(G.conexion_principal);
        cmd.CommandText = "SELECT mensaje, asunto, para, id, fecha_creacion FROM Mails_enviados WHERE usuario_id = ? order by fecha_creacion desc";
        SQLiteParameter param = new SQLiteParameter();
        cmd.Parameters.Add(param);
        param.Value = this.ID;
        SQLiteDataReader dr = cmd.ExecuteReader();

        List<mail_enviado> lista_mails = new List<mail_enviado>();

        while (dr.Read())
        {
            mail_enviado mail = new mail_enviado();
            mail.__mensaje = dr.GetString(0);
            mail.__asunto = dr.GetString(1);
            mail.__para = dr.GetString(2);
            mail.__id = dr.GetInt32(3);
            mail.__fecha_creacion = dr.GetDateTime(4);
            lista_mails.Add(mail);
        }

        dr.Close();
        dr.Dispose();
        cmd.Dispose();

        return lista_mails.ToArray();
    }


    public mail_enviado[] mailEnviadoPag(int nro)
    {
        List<mail_enviado> lista_mails_pag = new List<mail_enviado>();
        mail_enviado[] arrayMails = this.mailsEnviados();

        int mail_desde = (nro - 1) * 8;
        int mail_hasta;
        if (nro * 8 < arrayMails.Length)
        {
            mail_hasta = nro * 8 - 1;
        }
        else
        {
            mail_hasta = arrayMails.Length - 1;
        }

        for (int i = mail_desde; i <= mail_hasta; i++)
        {

            lista_mails_pag.Add(arrayMails[i]);
        }


        return lista_mails_pag.ToArray();

    }
}