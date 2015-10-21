#include "proto.h"

struct datos_control_thread {
  int (* funcion_principal) (void *);
  void * parametro;
  int * resultado;
};

int incrementar_cantidad_usuarios (int32_t volatile * cantidad_usuarios) {
  int32_t valor_actual;
  while (1) {
    valor_actual = *cantidad_usuarios;
    if (valor_actual < 0) return AVS_OBJETO_DESTRUIDO;
    if (InterlockedCompareExchange((LONG volatile *) cantidad_usuarios, valor_actual + 1, valor_actual) == valor_actual) return AVS_OK;
  }
}

HANDLE crear_thread (int (* punto_entrada) (void *), void * parametro, int * resultado) {
  struct datos_control_thread * datos = malloc(sizeof(struct datos_control_thread));
  if (!datos) return NULL;
  datos -> funcion_principal = punto_entrada;
  datos -> parametro = parametro;
  datos -> resultado = resultado;
  return CreateThread(NULL, 0, &punto_entrada_threads, datos, 0, NULL);
}

int esperar_threads (HANDLE * threads, unsigned cantidad) {
  unsigned cantidad_espera = min(cantidad, MAXIMUM_WAIT_OBJECTS);
  unsigned rv = WaitForMultipleObjects(cantidad_espera, threads, 1, INFINITE);
  if (rv >= MAXIMUM_WAIT_OBJECTS) return AVS_ERROR_INTERNO;
  if (cantidad > cantidad_espera) return esperar_threads(threads + MAXIMUM_WAIT_OBJECTS, cantidad - MAXIMUM_WAIT_OBJECTS);
  return AVS_OK;
}

int thread_ejecutando (HANDLE thread) {
  return WaitForSingleObject(thread, 0);
}

DWORD WINAPI punto_entrada_threads (LPVOID parametro) {
  struct datos_control_thread * datos = parametro;
  int rv = datos -> funcion_principal(datos -> parametro);
  if (datos -> resultado) *(datos -> resultado) = rv;
  free(datos);
  return 0;
}

void esperar_mutex (HANDLE * mutex) {
  if (!mutex) return;
  if (!*mutex) {
    *mutex = CreateMutex(NULL, 1, NULL);
    return;
  }
  int rv = WaitForSingleObject(*mutex, INFINITE);
  if (rv) *mutex = CreateMutex(NULL, 1, NULL);
}
