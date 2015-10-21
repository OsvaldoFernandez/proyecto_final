#include "proto.h"

int agregar_muestra_a_entrenador (volatile Entrenador * entrenador, int tipo_muestra, int persona, unsigned fragmentos, unsigned muestreo,
                                  double ** amplitudes, double * volumenes) {
  esperar_mutex((HANDLE *) &(entrenador -> mutex_muestras));
  int id = entrenador -> siguienteID ++;
  if (id < 0) {
    ReleaseMutex(entrenador -> mutex_muestras);
    return AVS_ERROR_INTERNO;
  }
  Muestra * p = realloc(entrenador -> muestras, sizeof(Muestra) * (1 + entrenador -> cantidad_muestras));
  if (!p) {
    ReleaseMutex(entrenador -> mutex_muestras);
    return AVS_SIN_MEMORIA;
  }
  entrenador -> muestras = p;
  p[entrenador -> cantidad_muestras ++] = (Muestra) {.amplitudes = amplitudes, .volumenes = volumenes, .fragmentos = fragmentos, .muestreo = muestreo,
                                                     .tipo = tipo_muestra, .persona = persona, .ID = id};
  ReleaseMutex(entrenador -> mutex_muestras);
  return id;
}

int eliminar_muestra_de_entrenador (volatile Entrenador * entrenador, int ID) {
  esperar_mutex((HANDLE *) &(entrenador -> mutex_muestras));
  int posicion = buscar_muestra_por_ID(entrenador -> muestras, entrenador -> cantidad_muestras, ID);
  if (posicion < 0) {
    ReleaseMutex(entrenador -> mutex_muestras);
    return AVS_MUESTRA_NO_EXISTE;
  }
  destruir_muestra(posicion[entrenador -> muestras]);
  if (-- (entrenador -> cantidad_muestras)) {
    if (entrenador -> cantidad_muestras != posicion)
      memmove(entrenador -> muestras + posicion, entrenador -> muestras + posicion + 1, (entrenador -> cantidad_muestras - posicion) * sizeof(Muestra));
    entrenador -> muestras = realloc(entrenador -> muestras, sizeof(Muestra) * entrenador -> cantidad_muestras);
  } else {
    free(entrenador -> muestras);
    entrenador -> muestras = NULL;
  }
  ReleaseMutex(entrenador -> mutex_muestras);
  return AVS_OK;
}

int buscar_muestra_por_ID (const Muestra * muestras, unsigned cantidad_muestras, int ID) {
  const Muestra * encontrado = bsearch(&ID, muestras, cantidad_muestras, sizeof(Muestra), &comparar_IDs);
  if (!encontrado) return -1;
  return encontrado - muestras;
}

int comparar_IDs (const void * id, const void * muestra) {
  const int * ID = id;
  const Muestra * datos_muestra = muestra;
  return (*ID > datos_muestra -> ID) - (*ID < datos_muestra -> ID);
}
