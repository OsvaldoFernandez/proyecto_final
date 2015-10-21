#define UNICODE
#define ___COMP_AUTVOZ

#if 'abcd' != 0x61626364
  #error Las constantes multicaracter no contienen los valores esperados
#endif

#include "autvoz.h"

#include <stdlib.h>
#include <string.h>
#include <stdint.h>
#include <complex.h>

#include <math.h>

#include <windows.h>

#define pi 0x3.243f6a8885a308d3p+0

#define THREADS_ENTRENAMIENTO            20
#define PASO_ENTRENAMIENTO               50
#define CICLO_ENTRENAMIENTO             500
#define LIMITE_ENTRENAMIENTO           3000
#define PUNTAJE_PARADA                    0.965
#define PROBABILIDAD_EXCLUSION_MINIMA     0.1
#define PROBABILIDAD_EXCLUSION_MAXIMA     0.4
#define CANTIDAD_EXCLUSION_MAXIMA         8
#define VERSION_PAV_SOPORTADA             2
#define PROBABILIDAD_PLAN_20              0.35
#define PROBABILIDAD_PLAN_30              0.4
#define PROBABILIDAD_METODO_1             0.15
#define PROBABILIDAD_METODO_2             0.15
#define CANTIDAD_MAXIMA_RESULTADOS       16

#ifdef __GNUC__
  #pragma GCC visibility push(hidden)
#endif


typedef struct RNA {
  unsigned * nodos;
  double *** pesos;
  unsigned niveles;
} RNA;

typedef struct RNG {
  uint32_t estado[624];
  unsigned short posicion;
} RNG;

typedef struct {
  double ** amplitudes;
  double * volumenes;
  int ID;
  unsigned fragmentos;
  unsigned muestreo;
  int persona;
  int tipo;
} Muestra;

typedef struct {
  double puntaje;
  double cantidad_OK;
  double alfa_0;
  double alfa_d;
  RNA * red;
  unsigned iteraciones;
  unsigned plan;
  unsigned d;
  int excluir;
  int estado; // 0: detenida, 1: iniciada, 2: aceptada
} RedEntrenamiento;

typedef struct {
  long long tiempo_ejecucion;
  HANDLE mutex_muestras;
  HANDLE mutex_redes;
  HANDLE thread_ejecucion;
  Muestra * muestras;
  RedEntrenamiento ** redes_entrenamiento;
  RNG * generador_aleatorio;
  int * personas;
  unsigned cantidad_muestras;
  int siguienteID;
  int32_t cantidad_usuarios;
  int32_t bloqueado;
  unsigned momento_iniciado;
  unsigned cantidad_redes;
  int detencion_solicitada;
  unsigned redes_generadas;
  unsigned redes_descartadas;
  unsigned cantidad_personas;
} Entrenador;

typedef struct {
  double puntaje_promedio;
  RNA ** redes;
  double * pesos;
  unsigned * planes;
  unsigned cantidad_redes;
  unsigned muestras_entrenamiento;
  unsigned muestras_validacion;
  int32_t cantidad_usuarios;
} Autenticador;

struct parametros_red {
  double a0;
  double ad;
  double b1;
  double b2;
  double L;
  unsigned d;
  unsigned z;
  unsigned plan;
  int excluir;
};

struct datos_thread_autenticacion {
  unsigned fragmentos;
  unsigned muestreo;
  double ** amplitudes;
  double * volumenes;
  unsigned * fragmentos_ordenados;
  RNA * red;
  unsigned plan;
  unsigned cantidad_resultados;
  double resultados[CANTIDAD_MAXIMA_RESULTADOS];
};

struct datos_thread_entrenamiento {
  RedEntrenamiento * red_entrenamiento;
  Muestra * muestras;
  unsigned cantidad_muestras;
  int detener;
};

struct encabezado_archivo_WAV {
  unsigned longitud;
  unsigned muestreo;
  unsigned short canales;
  unsigned short bytes_por_muestra;
  unsigned short bits_significativos;
};

struct parametros_exportacion {
  unsigned long long posicion;
  unsigned long long longitud;
  unsigned plan;
  double ponderacion;
};

struct datos_ordenamiento_fragmento {
  unsigned numero;
  double volumen;
};

struct datos_muestra_entrenamiento {
  int tipo; // bit 0: 1 = positiva, 0 = negativa; bit 1: entrenamiento; bit 2: validacion
  unsigned cantidad_fragmentos;
  double * datos_fragmentos[CANTIDAD_MAXIMA_RESULTADOS];
};


#undef min
#define min(a, b) ((a) < (b) ? (a) : (b))

// entrbase.c
void inicializar_entrenador(Entrenador *);
int puede_destruir_entrenador(volatile Entrenador *);
void destruir_entrenador(Entrenador);
int obtener_entrenador_sin_bloquear(volatile Entrenador *);
int bloquear_entrenador(volatile Entrenador *);
void destruir_muestra(Muestra);

// rna.c
RNA * crear_red_neuronal(unsigned, const unsigned *, unsigned, RNG *);
void destruir_red_neuronal(RNA *);
void reinicializar_red_neuronal(RNA *, double, RNG *);
double procesar_entradas_RNA(RNA *, const double *);
double entrenar_entradas_RNA(RNA *, const double *, double, double);
unsigned long long persistir_red_neuronal(RNA *, uint32_t **);
RNA * cargar_red_neuronal(const uint32_t *, unsigned);

// rng.c
RNG * crear_RNG(unsigned);
void destruir_RNG(RNG *);
unsigned entero_aleatorio(RNG *, unsigned);
double aleatorio(RNG *);

// archivos.c
int abrir_archivo(const wchar_t *, unsigned char, HANDLE *);
long long leer_archivo(void *, unsigned long long, HANDLE);
long long escribir_archivo(const void *, unsigned long long, HANDLE);
int posicionar_archivo(HANDLE, long long, unsigned char);
uintmax_t obtener_valor_de_archivo(HANDLE, unsigned char);
int leer_enteros_de_archivo(HANDLE, unsigned, int32_t *, int);
int32_t invertir_entero(int32_t);
int escribir_enteros_en_archivo(HANDLE, unsigned, const int32_t *);
unsigned long long obtener_posicion_de_archivo(HANDLE);

// autcarga.c
int inicializar_autenticador(Autenticador *, const wchar_t *);
void destruir_autenticador(Autenticador);
int cargar_red(HANDLE, unsigned, unsigned, RNA **, int);
int numero_version(uint32_t);

// mthread.c
int incrementar_cantidad_usuarios(int32_t volatile *);
HANDLE crear_thread(int (*) (void *), void *, int *);
int esperar_threads(HANDLE *, unsigned);
int thread_ejecutando(HANDLE);
DWORD WINAPI punto_entrada_threads(LPVOID);
void esperar_mutex(HANDLE *);

// sndproc.c
int obtener_amplitudes_de_fragmentos(HANDLE, double ***, double **, unsigned *);
void destruir_amplitudes_de_fragmentos(double **);
double * obtener_amplitudes_de_fragmento(const double *, unsigned);
double obtener_volumen_de_fragmento(const double *, unsigned);

// datoswav.c
int obtener_muestras_de_archivo(HANDLE, double **, unsigned *);
int obtener_encabezado_de_archivo(HANDLE, struct encabezado_archivo_WAV *);
int saltear_fragmento(HANDLE);
int siguiente_fragmento(HANDLE);
int procesar_parte_de_archivo(const void *, unsigned, struct encabezado_archivo_WAV, double *);

// autproc.c
int autenticar(Autenticador *, double **, double *, unsigned, unsigned);
int procesar_red_autenticacion(void *);
int generar_resultados_parciales(const struct datos_thread_autenticacion *, const double *, unsigned, double **, double **);

// frags.c
int ordenar_fragmentos(const double *, unsigned, unsigned *);
int comparar_fragmentos(const void *, const void *);
void generar_entradas_de_fragmento(const double *, double, unsigned, double *);

// autpond.c
int ponderar_resultados(const double *, const double *, unsigned);
double obtener_coeficiente_p0(const double *, const double *, unsigned);
void obtener_coeficientes_estadisticos(const double *, const double *, unsigned, double *);
double calcular_ponderacion_final(const double *);

// fourier.c
double complex * DFT(const double complex *, unsigned);
double complex * DFT_directa(const double complex *, unsigned);
double complex * FFT_factorizacion(const double complex *, unsigned, unsigned);

// entconst.c
int agregar_muestra_a_entrenador(volatile Entrenador *, int, int, unsigned, unsigned, double **, double *);
int eliminar_muestra_de_entrenador(volatile Entrenador *, int);
int buscar_muestra_por_ID(const Muestra *, unsigned, int);
int comparar_IDs(const void *, const void *);

// entrejec.c
int construir_entrenador(Entrenador *);
int validar_cantidades_muestras(Entrenador *);
int iniciar_entrenador(Entrenador *);
int detener_entrenador(Entrenador *);
void cargar_personas(Entrenador *);
int comparar_enteros(const void *, const void *);

// entfondo.c
int thread_control_entrenamiento(void *);
void actualizar_listado_redes(volatile Entrenador *, unsigned);
int determinar_aceptacion_red(RedEntrenamiento *);
void reemplazar_red(Entrenador *, unsigned, unsigned);
void aceptar_red(Entrenador *, unsigned);
int buscar_red_disponible(Entrenador *, unsigned);
void mover_red_a_posicion(Entrenador *, unsigned, unsigned);
int debe_descartar_red(Entrenador *, unsigned);
void descartar_red(Entrenador *, unsigned);

// entrred.c
int thread_entrenamiento(void *);
unsigned generar_muestras_entrenamiento(Muestra *, unsigned, unsigned, unsigned, int, int, struct datos_muestra_entrenamiento *);
void destruir_muestras_entrenamiento(struct datos_muestra_entrenamiento *, unsigned, int);
int validar_red(RedEntrenamiento *, const struct datos_muestra_entrenamiento *, unsigned);
void iterar_red(RedEntrenamiento *, const struct datos_muestra_entrenamiento *, unsigned);
int debe_usar_muestras_entrenamiento(Muestra *, unsigned, int);

// crearred.c
RedEntrenamiento * crear_red_nueva(Entrenador *);
void destruir_red_entrenamiento(RedEntrenamiento *);
struct parametros_red generar_parametros_red(RNG *, const int *, unsigned);
double calcular_probabilidad_exclusion(unsigned);
void cantidades_nodos(unsigned, unsigned, double, double, unsigned *);
unsigned nodos_por_nivel(unsigned, unsigned, unsigned, double, double);
unsigned generar_plan_red(RNG *);

// entrest.c
int estado_entrenador_en_construccion(Entrenador *, avt_estado *);
int estado_entrenador_listo(Entrenador *, avt_estado *);
int estado_entrenador_ejecutando(Entrenador *, avt_estado *);
void contar_muestras(Entrenador *, avt_estado *);
int estado_entrenador_listo_ejecutando(Entrenador *, avt_estado *);

// entrexp.c
int exportar_entrenador(Entrenador *, const wchar_t *);
double ponderacion_desde_puntaje(double);
int debe_exportar(RedEntrenamiento *);
int exportar_red(HANDLE, RedEntrenamiento *, unsigned long long *, struct parametros_exportacion *);
int exportar_parametros(HANDLE, struct parametros_exportacion *, unsigned, Entrenador *, double);

#ifdef __GNUC__
  #pragma GCC visibility pop
#endif
