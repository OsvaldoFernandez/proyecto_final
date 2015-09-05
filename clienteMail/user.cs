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
}