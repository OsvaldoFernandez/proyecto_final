#include "proto.h"

int thread_entrenamiento (void * datos_thread) {
  struct datos_thread_entrenamiento * datos = datos_thread;
  unsigned iteraciones_locales = 0;
  int usar_muestras_entrenamiento = debe_usar_muestras_entrenamiento(datos -> muestras, datos -> cantidad_muestras, datos -> red_entrenamiento -> excluir);
  unsigned cantidad_entradas = (datos -> red_entrenamiento -> plan & 0xfff) << 2;
  unsigned metodo_seleccion = (datos -> red_entrenamiento -> plan & 0xf000) >> 12;
  if (metodo_seleccion < 2) metodo_seleccion = 1 - metodo_seleccion;
  struct datos_muestra_entrenamiento * muestras = calloc(datos -> cantidad_muestras, sizeof(struct datos_muestra_entrenamiento));
  if (!muestras) return 1;
  unsigned cantidad_muestras = generar_muestras_entrenamiento(datos -> muestras, datos -> cantidad_muestras, cantidad_entradas, metodo_seleccion,
                                                              datos -> red_entrenamiento -> excluir, usar_muestras_entrenamiento, muestras);
  if (!cantidad_muestras) {
    destruir_muestras_entrenamiento(muestras, datos -> cantidad_muestras, metodo_seleccion);
    return 1;
  }
  datos -> red_entrenamiento -> estado = 1;
  int rv;
  while (!(datos -> detener) && (iteraciones_locales < CICLO_ENTRENAMIENTO)) {
    if (iteraciones_locales && !(iteraciones_locales % PASO_ENTRENAMIENTO)) {
      rv = validar_red(datos -> red_entrenamiento, muestras, cantidad_muestras);
      if (rv) break;
    }
    iteraciones_locales ++;
    iterar_red(datos -> red_entrenamiento, muestras, cantidad_muestras);
  }
  validar_red(datos -> red_entrenamiento, muestras, cantidad_muestras);
  datos -> red_entrenamiento -> estado = 0;
  destruir_muestras_entrenamiento(muestras, cantidad_muestras, metodo_seleccion);
  return 0;
}

unsigned generar_muestras_entrenamiento (Muestra * muestras, unsigned cantidad, unsigned cantidad_entradas, unsigned metodo_seleccion, int excluir,
                                         int usar_muestras_entrenamiento, struct datos_muestra_entrenamiento * resultado) {
  unsigned actual = 0;
  unsigned muestra;
  unsigned * fragmentos_ordenados;
  int t;
  for (muestra = 0; muestra < cantidad; muestra ++) {
    if (excluir && (muestras[muestra].persona == excluir)) continue;
    if ((muestras[muestra].tipo < 1) || (muestras[muestra].tipo > 3)) continue;
    if (!(muestras[muestra].fragmentos)) continue;
    if (muestras[muestra].muestreo < (cantidad_entradas << 3)) continue;
    t = muestras[muestra].persona ? 0 : 1;
    if (usar_muestras_entrenamiento & (1 << t)) t |= 4;
    resultado[actual].tipo = t | (muestras[muestra].tipo << 1);
    fragmentos_ordenados = malloc(sizeof(unsigned) * muestras[muestra].fragmentos);
    if (!fragmentos_ordenados) return 0;
    if (ordenar_fragmentos(muestras[muestra].volumenes, muestras[muestra].fragmentos, fragmentos_ordenados)) {
      free(fragmentos_ordenados);
      return 0;
    }
    if (!metodo_seleccion) {
      resultado[actual].cantidad_fragmentos = 1;
      if (!(*(resultado[actual].datos_fragmentos) = malloc(sizeof(double) * cantidad_entradas))) {
        free(fragmentos_ordenados);
        return 0;
      }
      generar_entradas_de_fragmento(muestras[muestra].amplitudes[*fragmentos_ordenados], muestras[muestra].volumenes[*fragmentos_ordenados],
                                    cantidad_entradas, *(resultado[actual].datos_fragmentos));
    } else {
      resultado[actual].cantidad_fragmentos = min(metodo_seleccion, muestras[muestra].fragmentos);
      for (t = 0; t < resultado[actual].cantidad_fragmentos; t ++)
        t[resultado[actual].datos_fragmentos] = muestras[muestra].amplitudes[fragmentos_ordenados[t]];
    }
    free(fragmentos_ordenados);
    actual ++;
  }
  return actual;
}

void destruir_muestras_entrenamiento (struct datos_muestra_entrenamiento * muestras, unsigned cantidad, int preservar_fragmentos) {
  if (!preservar_fragmentos) {
    unsigned muestra, fragmento;
    for (muestra = 0; muestra < cantidad; muestra ++)
      for (fragmento = 0; fragmento < muestras[muestra].cantidad_fragmentos; fragmento ++)
        free(muestras[muestra].datos_fragmentos[fragmento]);
  }
  free(muestras);
}

int validar_red (RedEntrenamiento * red, const struct datos_muestra_entrenamiento * muestras, unsigned cantidad_muestras) {
  double error_cuadratico = 0;
  double e;
  unsigned muestra, fragmento, cantidad = 0, ok = 0;
  for (muestra = 0; muestra < cantidad_muestras; muestra ++) {
    if (!(muestras[muestra].tipo & 4)) continue;
    for (fragmento = 0; fragmento < muestras[muestra].cantidad_fragmentos; fragmento ++) {
      e = procesar_entradas_RNA(red -> red, muestras[muestra].datos_fragmentos[fragmento]) - (muestras[muestra].tipo & 1);
      cantidad ++;
      e *= e;
      error_cuadratico += e;
      if (e < 0.25) ok ++;
    }
  }
  e = sqrt(error_cuadratico / cantidad);
  red -> cantidad_OK = ((double) ok) / cantidad;
  red -> puntaje = red -> cantidad_OK * (1.0 - e);
  return red -> puntaje > PUNTAJE_PARADA;
}

void iterar_red (RedEntrenamiento * red, const struct datos_muestra_entrenamiento * muestras, unsigned cantidad_muestras) {
  unsigned muestra, fragmento;
  double tasa;
  if (red -> iteraciones >= red -> d)
    tasa = red -> alfa_d;
  else
    tasa = (red -> alfa_0 * (red -> d - red -> iteraciones) + red -> alfa_d * red -> iteraciones) / red -> d;
  for (muestra = 0; muestra < cantidad_muestras; muestra ++) {
    if (!(muestras[muestra].tipo & 2)) continue;
    for (fragmento = 0; fragmento < muestras[muestra].cantidad_fragmentos; fragmento ++)
      entrenar_entradas_RNA(red -> red, muestras[muestra].datos_fragmentos[fragmento], muestras[muestra].tipo & 1, tasa);
  }
  red -> iteraciones ++;
}

int debe_usar_muestras_entrenamiento (Muestra * muestras, unsigned cantidad_muestras, int excluir) {
  int resultado = 3;
  unsigned muestra;
  for (muestra = 0; muestra < cantidad_muestras; muestra ++) {
    if (excluir && (muestras[muestra].persona == excluir)) continue;
    if (!(muestras[muestra].tipo & AVP_MUESTRA_VALIDACION)) continue;
    if (muestras[muestra].persona)
      resultado &= ~1;
    else
      resultado &= ~2;
    if (!resultado) return 0;
  }
  return resultado;
}
