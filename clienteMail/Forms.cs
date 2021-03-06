﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Speech.Recognition;
using System.Collections.Generic;

public class RichForm : Form {
  public RichForm form_padre;

  public virtual void manejar_comando(string comando) {}
  public virtual void manejar_comando_entrenamiento(SpeechRecognizedEventArgs e) {}
  public virtual void agregar_contacto(int id) {}
  public virtual void agregar_asunto(int id) {}
  public virtual void agregar_mensaje(int id) {}
  public virtual void manejar_aceptar(string contexto) {}
  public virtual void manejar_cerrar(string contexto) {}
  public virtual void actualizar_estado_microfono(bool estado) {}

  private void agregar_controles_de (Dictionary<string, Control> col, Control ctl) {
    foreach (Control c in ctl.Controls) {
      if ((c.Name != null) && (c.Name != "")) col.Add(c.Name, c);
      agregar_controles_de(col, c);
    }
  }

  public Dictionary<string, Control> Controles { get {
    var col = new Dictionary<string, Control>();
    agregar_controles_de(col, this);
    return col;
  }}
}

public class FormComandos : RichForm {
  protected struct Comando {
    public string nombre;
    public Action evento;

    public Comando (string nombre, Action evento) {
      this.nombre = nombre;
      this.evento = evento;
    }

    public static Comando Evento (string nombre, EventHandler evento) {
      return new Comando(nombre, () => evento(null, EventArgs.Empty));
    }
  }

  protected struct ComandoEquivalente {
    string _comando;
    string _equivalencia;

    public ComandoEquivalente (string comando, string equivalencia) {
      _comando = comando;
      _equivalencia = equivalencia;
    }

    public string comando { get {
      return _comando;
    }}

    public string equivalencia { get {
      return _equivalencia;
    }}
  }

  protected static readonly ComandoEquivalente[] comandos_equivalentes = new ComandoEquivalente[] {
    new ComandoEquivalente("enviar", "enviados"),
    new ComandoEquivalente("enviados", "enviar")
  };

  protected bool manejar_comando_basico (string comando, params Comando[] comandos_posibles) {
    if (comando == null) return true;
    foreach (Comando c in comandos_posibles) {
      if (c.nombre != comando) continue;
      if (c.evento != null) c.evento();
      return true;
    }
    foreach (ComandoEquivalente eq in comandos_equivalentes) {
      if (eq.comando != comando) continue;
      if (manejar_comando_basico(eq.equivalencia, comandos_posibles)) return true;
    }
    return false;
  }

  protected void actualizar_banderas (Control bandera_positiva, Control bandera_negativa) {
    if (G.confianza_autenticacion == AV.AVS_PUNTERO_NULO)
      bandera_negativa.Visible = bandera_positiva.Visible = false;
    else if (G.confianza_autenticacion >= (100 * G.sensibilidad_autenticacion)) {
      bandera_positiva.Visible = true;
      bandera_negativa.Visible = false;
    } else {
      bandera_positiva.Visible = false;
      bandera_negativa.Visible = true;
    }
  }
}

public class FormPaginado : FormComandos {
  protected int pagActual = 1;
  protected readonly Color varcolor = Color.FromArgb(174, 225, 242);

  protected void actualizar_pagina (int cantidad_elementos, Button boton_anterior, Button boton_siguiente, Label numero_pagina) {
    int cantPaginas = (cantidad_elementos + 7) / 8;
    if (cantidad_elementos == 0) cantPaginas = 1;
    numero_pagina.Text = "Página " + pagActual.ToString() + " de " + cantPaginas.ToString();
    boton_anterior.Enabled = pagActual != 1;
    boton_anterior.Visible = pagActual != 1;
    boton_siguiente.Enabled = (cantPaginas != 0) && (pagActual != cantPaginas);
    boton_siguiente.Visible = (cantPaginas != 0) && (pagActual != cantPaginas);
  }

  protected virtual void resetPanels () {
    for (int i = 1; i <= 8; i ++)
      Controls["panel" + i.ToString()].BackColor =  (new Color[] {Color.White, Color.FromArgb(241, 255, 255)})[i % 2];
  }

  protected int numero_desde_texto (string texto) {
    switch (texto) {
      case "uno":    return 1;
      case "dos":    return 2;
      case "tres":   return 3;
      case "cuatro": return 4;
      case "cinco":  return 5;
      case "seis":   return 6;
      case "siete":  return 7;
      case "ocho":   return 8;
      default:       return 0;
    }
  }

  protected bool manejar_comando_basico (string comando, Action<int> comando_numerico, params Comando[] otros_comandos) {
    // retorna true si manejo el comando, o false si no era uno de los comandos reconocidos
    if (comando == null) return true;
    int numero = numero_desde_texto(comando);
    if (numero != 0) {
      if (comando_numerico != null) comando_numerico(numero);
      return true;
    }
    return manejar_comando_basico(comando, otros_comandos);
  }

  protected void agregar_eventos (Action<int> manejador, bool doble_click, params string[] controles) {
    for (int i = 1; i <= 8; i ++) {
      int k = i;
      foreach (string control in controles) {
        Control ctl = Controles[control + i.ToString()];
        if (doble_click)
          ctl.DoubleClick += (object sender, EventArgs e) => manejador(k);
        else
          ctl.Click += (object sender, EventArgs e) => manejador(k);
      }
    }
  }

  protected virtual void seleccionar_elemento (int elemento, string control_validacion, string control_seleccion, DataGridView grilla) {
    this.resetPanels();
    if (Controles[control_validacion + elemento.ToString()].Visible) {
      Controles[control_seleccion + elemento.ToString()].BackColor = varcolor;
      grilla.Rows[elemento - 1].Selected = true;
    }
  }
}