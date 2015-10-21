#include "proto.h"

void inicializar_entrenador (Entrenador * entrenador) {
  entrenador -> cantidad_usuarios = 0;
  entrenador -> bloqueado = 0;
  entrenador -> tiempo_ejecucion = -1;
  entrenador -> mutex_muestras = CreateMutex(NULL, 0, NULL);
  entrenador -> mutex_redes = NULL;
  entrenador -> cantidad_muestras = 0;
  entrenador -> muestras = NULL;
  entrenador -> siguienteID = 0;
  entrenador -> momento_iniciado = 0;
  entrenador -> thread_ejecucion = NULL;
  entrenador -> redes_entrenamiento = NULL;
  entrenador -> cantidad_redes = 0;
  entrenador -> detencion_solicitada = 0;
  entrenador -> generador_aleatorio = crear_RNG(GetTickCount());
  entrenador -> redes_generadas = 0;
  entrenador -> redes_descartadas = 0;
  entrenador -> personas = NULL;
  entrenador -> cantidad_personas = 0;
}

int puede_destruir_entrenador (volatile Entrenador * entrenador) {
  int rv = bloquear_entrenador(entrenador);
  if (rv) return rv;
  if (entrenador -> thread_ejecucion) return AVS_ENTRENADOR_EJECUTANDO;
  entrenador -> cantidad_usuarios = -1;
  return AVS_OK;
}

void destruir_entrenador (Entrenador entrenador) {
  if (entrenador.mutex_muestras) CloseHandle(entrenador.mutex_muestras);
  if (entrenador.mutex_redes) CloseHandle(entrenador.mutex_redes);
  if (entrenador.thread_ejecucion) {
    if (thread_ejecutando(entrenador.thread_ejecucion)) TerminateThread(entrenador.thread_ejecucion, -1);
    CloseHandle(entrenador.thread_ejecucion);
  }
  unsigned i;
  if (entrenador.muestras) {
    for (i = 0; i < entrenador.cantidad_muestras; i ++) destruir_muestra(i[entrenador.muestras]);
    free(entrenador.muestras);
  }
  destruir_RNG(entrenador.generador_aleatorio);
  free(entrenador.personas);
  if (entrenador.redes_entrenamiento) {
    for (i = 0; i < entrenador.cantidad_redes; i ++) destruir_red_entrenamiento(i[entrenador.redes_entrenamiento]);
    free(entrenador.redes_entrenamiento);
  }
}

int obtener_entrenador_sin_bloquear (volatile Entrenador * entrenador) {
  int rv = incrementar_cantidad_usuarios(&(entrenador -> cantidad_usuarios));
  if (rv) return rv;
  if (entrenador -> bloqueado) {
    InterlockedDecrement((LONG volatile *) &(entrenador -> cantidad_usuarios));
    return AVS_OBJETO_OCUPADO;
  }
  return AVS_OK;
}

int bloquear_entrenador (volatile Entrenador * entrenador) {
  int rv = incrementar_cantidad_usuarios(&(entrenador -> cantidad_usuarios));
  if (rv) return rv;
  rv = AVS_OBJETO_OCUPADO;
  if (InterlockedCompareExchange((LONG volatile *) &(entrenador -> bloqueado), 1, 0)) goto fin;
  if (entrenador -> cantidad_usuarios > 1) {
    entrenador -> bloqueado = 0;
    goto fin;
  }
  rv = AVS_OK;
  fin:
  InterlockedDecrement((LONG volatile *) &(entrenador -> cantidad_usuarios));
  return rv;
}

void destruir_muestra (Muestra muestra) {
  destruir_amplitudes_de_fragmentos(muestra.amplitudes);
  free(muestra.volumenes);
}
