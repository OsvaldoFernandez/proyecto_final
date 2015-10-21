#include "proto.h"

int construir_entrenador (Entrenador * entrenador) {
  int rv = validar_cantidades_muestras(entrenador);
  if (rv) return rv;
  entrenador -> tiempo_ejecucion = 0;
  CloseHandle(entrenador -> mutex_muestras);
  entrenador -> mutex_muestras = NULL;
  entrenador -> mutex_redes = CreateMutex(NULL, 0, NULL);
  cargar_personas(entrenador);
  return AVS_OK;
}

int validar_cantidades_muestras (Entrenador * entrenador) {
  unsigned positivas = 0, negativas = 0;
  esperar_mutex(&(entrenador -> mutex_muestras));
  unsigned actual;
  for (actual = 0; actual < entrenador -> cantidad_muestras; actual ++) {
    if (!(actual[entrenador -> muestras].tipo & AVP_MUESTRA_ENTRENAMIENTO)) continue;
    if (actual[entrenador -> muestras].persona)
      negativas ++;
    else
      positivas ++;
  }
  ReleaseMutex(entrenador -> mutex_muestras);
  if (positivas && negativas)
    return AVS_OK;
  else if (positivas)
    return AVS_FALTAN_MUESTRAS_NEGATIVAS;
  else if (negativas)
    return AVS_FALTAN_MUESTRAS_POSITIVAS;
  else
    return AVS_SIN_MUESTRAS_ENTRENAMIENTO;
}

int iniciar_entrenador (Entrenador * entrenador) {
  entrenador -> detencion_solicitada = 0;
  entrenador -> momento_iniciado = GetTickCount();
  entrenador -> thread_ejecucion = crear_thread(&thread_control_entrenamiento, entrenador, NULL);
  if (!(entrenador -> thread_ejecucion)) {
    entrenador -> momento_iniciado = 0;
    return AVS_ERROR_CREANDO_THREADS;
  }
  return AVS_OK;
}

int detener_entrenador (Entrenador * entrenador) {
  entrenador -> detencion_solicitada = 1;
  if (WaitForSingleObject(entrenador -> thread_ejecucion, INFINITE)) TerminateThread(entrenador -> thread_ejecucion, -1);
  entrenador -> thread_ejecucion = NULL;
  entrenador -> tiempo_ejecucion += GetTickCount() - entrenador -> momento_iniciado;
  entrenador -> momento_iniciado = 0;
  return AVS_OK;
}

void cargar_personas (Entrenador * entrenador) {
  int * ptemp = malloc(sizeof(int) * entrenador -> cantidad_muestras);
  unsigned i;
  for (i = 0; i < entrenador -> cantidad_muestras; i ++) ptemp[i] = i[entrenador -> muestras].persona;
  qsort(ptemp, entrenador -> cantidad_muestras, sizeof(int), &comparar_enteros);
  int persona_anterior = 0;
  entrenador -> personas = NULL;
  entrenador -> cantidad_personas = 0;
  for (i = 0; i < entrenador -> cantidad_muestras; i ++)
    if (ptemp[i] != persona_anterior) {
      persona_anterior = ptemp[i];
      if (persona_anterior) {
        entrenador -> personas = realloc(entrenador -> personas, sizeof(int) * (entrenador -> cantidad_personas + 1));
        (entrenador -> personas)[entrenador -> cantidad_personas ++] = persona_anterior;
      }
    }
  free(ptemp);
}

int comparar_enteros (const void * n1, const void * n2) {
  int a = *((const int *) n1);
  int b = *((const int *) n2);
  return (a > b) - (a < b);
}
