#include "proto.h"

int thread_control_entrenamiento (void * entrenador) {
  HANDLE threads[THREADS_ENTRENAMIENTO];
  struct datos_thread_entrenamiento datos_threads[THREADS_ENTRENAMIENTO];
  unsigned cantidad_threads = THREADS_ENTRENAMIENTO;
  memset(threads, 0, sizeof(HANDLE) * THREADS_ENTRENAMIENTO);
  volatile Entrenador * ent = entrenador;
  actualizar_listado_redes(ent, THREADS_ENTRENAMIENTO);
  cantidad_threads = min(THREADS_ENTRENAMIENTO, ent -> cantidad_redes);
  unsigned i;
  for (i = 0; i < cantidad_threads; i ++) {
    datos_threads[i].red_entrenamiento = i[ent -> redes_entrenamiento];
    datos_threads[i].detener = 0;
    datos_threads[i].muestras = ent -> muestras;
    datos_threads[i].cantidad_muestras = ent -> cantidad_muestras;
    datos_threads[i].red_entrenamiento -> estado = 1;
    threads[i] = crear_thread(&thread_entrenamiento, datos_threads + i, NULL);
    if (!threads[i]) {
      cantidad_threads = i;
      break;
    }
  }
  while (!(ent -> detencion_solicitada)) {
    WaitForMultipleObjects(cantidad_threads, threads, 0, 100);
    for (i = 0; i < cantidad_threads; i ++) if (!thread_ejecutando(threads[i])) {
      CloseHandle(threads[i]);
      reemplazar_red((Entrenador *) ent, i, cantidad_threads);
      datos_threads[i].red_entrenamiento = i[ent -> redes_entrenamiento];
      datos_threads[i].detener = 0;
      datos_threads[i].red_entrenamiento -> estado = 1;
      threads[i] = crear_thread(&thread_entrenamiento, datos_threads + i, NULL);
    }
  }
  for (i = 0; i < cantidad_threads; i ++) datos_threads[i].detener = 1;
  esperar_threads(threads, cantidad_threads);
  for (i = 0; i < cantidad_threads; i ++) {
    if (thread_ejecutando(threads[i])) TerminateThread(threads[i], -1);
    CloseHandle(threads[i]);
  }
  return 0;
}

void actualizar_listado_redes (volatile Entrenador * entrenador, unsigned cantidad) {
  esperar_mutex((HANDLE *) &(entrenador -> mutex_redes));
  unsigned pos;
  int actual = cantidad;
  if (entrenador -> cantidad_redes < cantidad) {
    entrenador -> redes_entrenamiento = realloc(entrenador -> redes_entrenamiento, sizeof(RedEntrenamiento *) * cantidad);
    memset(entrenador -> redes_entrenamiento + entrenador -> cantidad_redes, 0, sizeof(RedEntrenamiento *) * (cantidad - entrenador -> cantidad_redes));
    entrenador -> cantidad_redes = cantidad;
  }
  for (pos = 0; pos < cantidad; pos ++) {
    if (pos[entrenador -> redes_entrenamiento] && (pos[entrenador -> redes_entrenamiento] -> estado == 2)) aceptar_red((Entrenador *) entrenador, pos);
    if (!(pos[entrenador -> redes_entrenamiento])) {
      if (actual > 0) actual = buscar_red_disponible((Entrenador *) entrenador, actual);
      if (actual > 0)
        mover_red_a_posicion((Entrenador *) entrenador, actual, pos);
      else
        pos[entrenador -> redes_entrenamiento] = crear_red_nueva((Entrenador *) entrenador);
      continue;
    }
    if (debe_descartar_red((Entrenador *) entrenador, pos)) {
      descartar_red((Entrenador *) entrenador, pos);
      pos[entrenador -> redes_entrenamiento] = crear_red_nueva((Entrenador *) entrenador);
    }
  }
  ReleaseMutex(entrenador -> mutex_redes);
}

int determinar_aceptacion_red (RedEntrenamiento * red) {
  return red -> puntaje > PUNTAJE_PARADA;
}

void reemplazar_red (Entrenador * entrenador, unsigned numero_red, unsigned limite) {
  esperar_mutex((HANDLE *) &(entrenador -> mutex_redes));
  if (determinar_aceptacion_red(numero_red[entrenador -> redes_entrenamiento]))
    aceptar_red(entrenador, numero_red);
  else if (debe_descartar_red(entrenador, numero_red))
    descartar_red(entrenador, numero_red);
  if (!(numero_red[entrenador -> redes_entrenamiento])) {
    int posicion = buscar_red_disponible(entrenador, limite);
    if (posicion > 0)
      mover_red_a_posicion(entrenador, posicion, numero_red);
    else
      numero_red[entrenador -> redes_entrenamiento] = crear_red_nueva(entrenador);
  }
  ReleaseMutex(entrenador -> mutex_redes);
}

void aceptar_red (Entrenador * entrenador, unsigned numero_red) {
  numero_red[entrenador -> redes_entrenamiento] -> estado = 2;
  entrenador -> redes_entrenamiento = realloc(entrenador -> redes_entrenamiento, sizeof(RedEntrenamiento *) * (entrenador -> cantidad_redes + 1));
  (entrenador -> redes_entrenamiento)[entrenador -> cantidad_redes ++] = numero_red[entrenador -> redes_entrenamiento];
  numero_red[entrenador -> redes_entrenamiento] = NULL;
}

int buscar_red_disponible (Entrenador * entrenador, unsigned inicio) {
  unsigned posicion;
  for (posicion = inicio; posicion < entrenador -> cantidad_redes; posicion ++)
    if (posicion[entrenador -> redes_entrenamiento] -> estado != 2) return posicion;
  return -1;
}

void mover_red_a_posicion (Entrenador * entrenador, unsigned red, unsigned posicion) {
  posicion[entrenador -> redes_entrenamiento] = red[entrenador -> redes_entrenamiento];
  red[entrenador -> redes_entrenamiento] == (entrenador -> redes_entrenamiento)[-- (entrenador -> cantidad_redes)];
  entrenador -> redes_entrenamiento = realloc(entrenador -> redes_entrenamiento, sizeof(RedEntrenamiento *) * (entrenador -> cantidad_redes));
}

int debe_descartar_red (Entrenador * entrenador, unsigned numero_red) {
  RedEntrenamiento * red = numero_red[entrenador -> redes_entrenamiento];
  if (red -> iteraciones < CICLO_ENTRENAMIENTO) return 0;
  if (red -> iteraciones > LIMITE_ENTRENAMIENTO) return 1;
  double p = 1.125 - 1.25 * (red -> puntaje);
  double cc = ((double) (red -> iteraciones)) / LIMITE_ENTRENAMIENTO;
  double limite;
  if (p < 0)
    limite = cc;
  else if (p > 1)
    limite = 1.0;
  else
    limite = 1.0 - (1.0 - p) * (1.0 - cc);
  return aleatorio(entrenador -> generador_aleatorio) < limite;
}

void descartar_red (Entrenador * entrenador, unsigned numero_red) {
  destruir_red_entrenamiento(numero_red[entrenador -> redes_entrenamiento]);
  numero_red[entrenador -> redes_entrenamiento] = NULL;
  entrenador -> redes_descartadas ++;
}
