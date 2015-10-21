#include "proto.h"

int autenticar (Autenticador * autenticador, double ** amplitudes, double * volumenes, unsigned fragmentos, unsigned muestreo) {
  struct datos_thread_autenticacion * datos_threads = malloc(autenticador -> cantidad_redes * sizeof(struct datos_thread_autenticacion));
  HANDLE * threads = calloc(sizeof(HANDLE), autenticador -> cantidad_redes);
  unsigned * fragmentos_ordenados = malloc(sizeof(unsigned) * fragmentos);
  if (!(datos_threads && threads && fragmentos_ordenados)) {
    free(datos_threads);
    free(threads);
    free(fragmentos_ordenados);
    return AVS_SIN_MEMORIA;
  }
  int rv = ordenar_fragmentos(volumenes, fragmentos, fragmentos_ordenados);
  if (rv) {
    free(datos_threads);
    free(threads);
    free(fragmentos_ordenados);
    return rv;
  }
  unsigned i;
  for (i = 0; i < autenticador -> cantidad_redes; i ++) {
    datos_threads[i].fragmentos = fragmentos;
    datos_threads[i].muestreo = muestreo;
    datos_threads[i].amplitudes = amplitudes;
    datos_threads[i].volumenes = volumenes;
    datos_threads[i].fragmentos_ordenados = fragmentos_ordenados;
    datos_threads[i].red = i[autenticador -> redes];
    datos_threads[i].plan = i[autenticador -> planes];
    datos_threads[i].cantidad_resultados = 0;
    if (!(threads[i] = crear_thread(&procesar_red_autenticacion, datos_threads + i, NULL))) {
      for (; i < autenticador -> cantidad_redes; i --) {
        TerminateThread(threads[i], -1);
        CloseHandle(threads[i]);
      }
      free(datos_threads);
      free(threads);
      free(fragmentos_ordenados);
      return AVS_ERROR_CREANDO_THREADS;
    }
  }
  rv = esperar_threads(threads, autenticador -> cantidad_redes);
  for (i = 0; i < autenticador -> cantidad_redes; i ++) {
    if (thread_ejecutando(threads[i])) TerminateThread(threads[i], -1);
    CloseHandle(threads[i]);
  }
  free(threads);
  if (rv) {
    free(datos_threads);
    free(fragmentos_ordenados);
    return rv;
  }
  double * resultados;
  double * pesos;
  int cantidad = generar_resultados_parciales(datos_threads, autenticador -> pesos, autenticador -> cantidad_redes, &resultados, &pesos);
  free(datos_threads);
  free(fragmentos_ordenados);
  if (cantidad < 0) return cantidad;
  rv = ponderar_resultados(resultados, pesos, cantidad);
  free(resultados);
  free(pesos);
  return rv;
}

int procesar_red_autenticacion (void * datos_thread) {
  struct datos_thread_autenticacion * dt = datos_thread;
  dt -> cantidad_resultados = 0;
  unsigned cantidad_entradas = ((dt -> plan) & 0xfff) << 2;
  unsigned metodo_seleccion = ((dt -> plan) & 0xf000) >> 12;
  unsigned p;
  if (cantidad_entradas > (dt -> muestreo >> 3)) return 1;
  if (!(dt -> fragmentos))
    return 1;
  else if (metodo_seleccion > (dt -> fragmentos))
    metodo_seleccion = dt -> fragmentos;
  if (metodo_seleccion < 2) metodo_seleccion = 1 - metodo_seleccion;
  if (!metodo_seleccion) {
    double * entradas = malloc(sizeof(double) * cantidad_entradas);
    if (!entradas) return 1;
    dt -> cantidad_resultados = 1;
    generar_entradas_de_fragmento(dt -> amplitudes[*(dt -> fragmentos_ordenados)], dt -> volumenes[*(dt -> fragmentos_ordenados)], cantidad_entradas, entradas);
    *(dt -> resultados) = procesar_entradas_RNA(dt -> red, entradas);
    free(entradas);
  } else {
    dt -> cantidad_resultados = metodo_seleccion;
    for (p = 0; p < dt -> cantidad_resultados; p ++)
      p[dt -> resultados] = procesar_entradas_RNA(dt -> red, dt -> amplitudes[dt -> fragmentos_ordenados[p]]);
  }
  return 0;
}

int generar_resultados_parciales (const struct datos_thread_autenticacion * datos_threads, const double * pesos_redes, unsigned cantidad_redes,
                                  double ** resultados, double ** pesos) {
  unsigned pos, i;
  unsigned cantidad_resultados = 0;
  *resultados = *pesos = NULL;
  for (pos = 0; pos < cantidad_redes; pos ++) cantidad_resultados += datos_threads[pos].cantidad_resultados;
  if (!cantidad_resultados) return AVS_ERROR_INTERNO;
  *resultados = malloc(sizeof(double) * cantidad_resultados);
  *pesos = malloc(sizeof(double) * cantidad_resultados);
  if (!(*resultados || *pesos)) {
    free(*resultados);
    free(*pesos);
    *resultados = *pesos = NULL;
    return AVS_SIN_MEMORIA;
  }
  unsigned posicion_actual = 0;
  double peso_actual;
  for (pos = 0; pos < cantidad_redes; pos ++) {
    if (!datos_threads[pos].cantidad_resultados) continue;
    if (datos_threads[pos].cantidad_resultados > CANTIDAD_MAXIMA_RESULTADOS) {
      free(*resultados);
      free(*pesos);
      *resultados = *pesos = NULL;
      return AVS_ERROR_INTERNO;
    }
    memcpy(*resultados + posicion_actual, datos_threads[pos].resultados, sizeof(double) * datos_threads[pos].cantidad_resultados);
    peso_actual = pesos_redes[pos] / datos_threads[pos].cantidad_resultados;
    for (i = 0; i < datos_threads[pos].cantidad_resultados; i ++) (*pesos)[posicion_actual + i] = peso_actual;
    posicion_actual += datos_threads[pos].cantidad_resultados;
  }
  return cantidad_resultados;
}
