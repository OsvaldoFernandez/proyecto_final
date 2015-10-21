using System;
using System.Runtime.InteropServices;

public static class AV {
  public const int AVS_OK                              =                          0;
  public const int AVS_PUNTERO_NULO                    = unchecked((int) 0x80008001);
  public const int AVS_ARGUMENTO_NO_VALIDO             = unchecked((int) 0x80008002);
  public const int AVS_ARCHIVO_INACCESIBLE             = unchecked((int) 0x80008101);
  public const int AVS_FORMATO_ARCHIVO_NO_VALIDO       = unchecked((int) 0x80008102);
  public const int AVS_DURACION_MENOR_A_MEDIO_SEGUNDO  = unchecked((int) 0x80008103);
  public const int AVS_MUESTREO_DEMASIADO_BAJO         = unchecked((int) 0x80008104);
  public const int AVS_MUESTREO_NO_ES_MULTIPLO_DE_4_HZ = unchecked((int) 0x80008105);
  public const int AVS_FALLO_ESCRITURA_ARCHIVO         = unchecked((int) 0x80008106);
  public const int AVS_ENTRENADOR_EJECUTANDO           = unchecked((int) 0x80008201);
  public const int AVS_ENTRENADOR_NO_ESTA_EJECUTANDO   = unchecked((int) 0x80008202);
  public const int AVS_ENTRENADOR_NO_EN_CONSTRUCCION   = unchecked((int) 0x80008203);
  public const int AVS_FALTAN_MUESTRAS_POSITIVAS       = unchecked((int) 0x80008204);
  public const int AVS_FALTAN_MUESTRAS_NEGATIVAS       = unchecked((int) 0x80008205);
  public const int AVS_ENTRENADOR_EN_CONSTRUCCION      = unchecked((int) 0x80008206);
  public const int AVS_SIN_MUESTRAS_ENTRENAMIENTO      = unchecked((int) 0x80008207);
  public const int AVS_MUESTRA_NO_EXISTE               = unchecked((int) 0x80008208);
  public const int AVS_NADA_PARA_EXPORTAR              = unchecked((int) 0x80008209);
  public const int AVS_ERROR_INTERNO                   = unchecked((int) 0x80008300);
  public const int AVS_OBJETO_OCUPADO                  = unchecked((int) 0x80008301);
  public const int AVS_OBJETO_DESTRUIDO                = unchecked((int) 0x80008302);
  public const int AVS_ERROR_CREANDO_THREADS           = unchecked((int) 0x80008303);
  public const int AVS_SIN_MEMORIA                     = unchecked((int) 0x80008304);

  public const int AVP_MUESTRA_ENTRENAMIENTO           =                          1;
  public const int AVP_MUESTRA_VALIDACION              =                          2;

  [StructLayout(LayoutKind.Sequential)]
  public struct avt_informacion {
    public uint numero_redes;
    public uint puntaje_promedio;
    public uint muestras_entrenamiento;
    public uint muestras_validacion;
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct avt_estado {
    public int estado_general;
    public uint redes_generadas;
    public uint redes_satisfactorias;
    public uint redes_descartadas;
    public uint mejor_puntaje;
    public uint puntaje_promedio;
    public uint tiempo_transcurrido;
    public uint muestras_entrenamiento_negativas;
    public uint muestras_entrenamiento_positivas;
    public uint muestras_validacion_negativas;
    public uint muestras_validacion_positivas;
    public uint muestras_totales;
  }

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_crear_autenticador(string archivo, out IntPtr autenticador);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_destruir_autenticador(IntPtr autenticador);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_autenticar_WAV(IntPtr autenticador, string archivoWAV);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_obtener_informacion_autenticador(IntPtr autenticador, out avt_informacion informacion);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_crear_entrenador(out IntPtr entrenador);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_destruir_entrenador(IntPtr entrenador);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_agregar_muestra_WAV(IntPtr entrenador, int persona, int parametros, string archivoWAV);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_eliminar_muestra(IntPtr entrenador, int muestra);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_iniciar_entrenamiento(IntPtr entrenador);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_detener_entrenamiento(IntPtr entrenador);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_estado_entrenamiento(IntPtr entrenador, out avt_estado estado);

  [DllImport("autvoz.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
  public static extern int avf_exportar_entrenamiento(IntPtr entrenador, string archivo);
}