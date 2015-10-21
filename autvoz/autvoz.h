#ifndef ___AUTVOZ

#define ___AUTVOZ 200

#include <wchar.h>

#ifdef ___DLL
  #undef ___DLL
#endif

#ifdef ___COMP_AUTVOZ
  #ifdef __GNUC__
    #pragma GCC visibility push(default)
  #endif
  #define ___DLL __stdcall __declspec(dllexport)
#else
  #define ___DLL __stdcall __declspec(dllimport)
#endif

#if defined(NODLL) || defined(NODLL_AUTVOZ)
  #undef ___DLL
  #define ___DLL
#endif

enum avx_resultados_llamadas {
  AVS_OK                              =          0,
  AVS_PUNTERO_NULO                    = 0x80008001,
  AVS_ARGUMENTO_NO_VALIDO             = 0x80008002,
  AVS_ARCHIVO_INACCESIBLE             = 0x80008101,
  AVS_FORMATO_ARCHIVO_NO_VALIDO       = 0x80008102,
  AVS_DURACION_MENOR_A_MEDIO_SEGUNDO  = 0x80008103,
  AVS_MUESTREO_DEMASIADO_BAJO         = 0x80008104,
  AVS_MUESTREO_NO_ES_MULTIPLO_DE_4_HZ = 0x80008105,
  AVS_FALLO_ESCRITURA_ARCHIVO         = 0x80008106,
  AVS_ENTRENADOR_EJECUTANDO           = 0x80008201,
  AVS_ENTRENADOR_NO_ESTA_EJECUTANDO   = 0x80008202,
  AVS_ENTRENADOR_NO_EN_CONSTRUCCION   = 0x80008203,
  AVS_FALTAN_MUESTRAS_POSITIVAS       = 0x80008204,
  AVS_FALTAN_MUESTRAS_NEGATIVAS       = 0x80008205,
  AVS_ENTRENADOR_EN_CONSTRUCCION      = 0x80008206,
  AVS_SIN_MUESTRAS_ENTRENAMIENTO      = 0x80008207,
  AVS_MUESTRA_NO_EXISTE               = 0x80008208,
  AVS_NADA_PARA_EXPORTAR              = 0x80008209,
  AVS_ERROR_INTERNO                   = 0x80008300,
  AVS_OBJETO_OCUPADO                  = 0x80008301,
  AVS_OBJETO_DESTRUIDO                = 0x80008302,
  AVS_ERROR_CREANDO_THREADS           = 0x80008303,
  AVS_SIN_MEMORIA                     = 0x80008304
};

enum avx_parametros {
  AVP_MUESTRA_ENTRENAMIENTO   =  1,
  AVP_MUESTRA_VALIDACION      =  2
};

typedef struct {
  int estado_general;
  unsigned redes_generadas;
  unsigned redes_satisfactorias;
  unsigned redes_descartadas;
  unsigned mejor_puntaje;
  unsigned puntaje_promedio;
  unsigned tiempo_transcurrido;
  unsigned muestras_entrenamiento_negativas;
  unsigned muestras_entrenamiento_positivas;
  unsigned muestras_validacion_negativas;
  unsigned muestras_validacion_positivas;
  unsigned muestras_totales;
} avt_estado;

typedef struct {
  unsigned numero_redes;
  unsigned puntaje_promedio;
  unsigned muestras_entrenamiento;
  unsigned muestras_validacion;
} avt_informacion;

#ifdef __cplusplus
extern "C" {
#endif

___DLL int avf_crear_autenticador(const wchar_t * archivo, void ** autenticador);
___DLL int avf_destruir_autenticador(void * autenticador);

___DLL int avf_autenticar_WAV(void * autenticador, const wchar_t * archivoWAV);

___DLL int avf_obtener_informacion_autenticador(void * autenticador, avt_informacion * informacion);

___DLL int avf_crear_entrenador(void ** entrenador);
___DLL int avf_destruir_entrenador(void * entrenador);

___DLL int avf_agregar_muestra_WAV(void * entrenador, int persona, int parametros, const wchar_t * archivoWAV);
___DLL int avf_eliminar_muestra(void * entrenador, int muestra);

___DLL int avf_iniciar_entrenamiento(void * entrenador);
___DLL int avf_detener_entrenamiento(void * entrenador);

___DLL int avf_estado_entrenamiento(void * entrenador, avt_estado * estado);

___DLL int avf_exportar_entrenamiento(void * entrenador, const wchar_t * archivo);

#ifdef __cplusplus
}
#endif

#if defined(__GNUC__) && defined(___COMP_AUTVOZ)
  #pragma GCC visibility pop
#endif

#endif
