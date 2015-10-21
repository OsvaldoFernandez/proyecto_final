#include "proto.h"

int ordenar_fragmentos (const double * volumenes, unsigned cantidad, unsigned * fragmentos_ordenados) {
  if (!(volumenes && cantidad && fragmentos_ordenados)) return AVS_ERROR_INTERNO;
  struct datos_ordenamiento_fragmento * fragmentos = malloc(sizeof(struct datos_ordenamiento_fragmento) * cantidad);
  if (!fragmentos) return AVS_SIN_MEMORIA;
  unsigned p;
  for (p = 0; p < cantidad; p ++) {
    fragmentos[p].numero = p;
    fragmentos[p].volumen = volumenes[p];
  }
  qsort(fragmentos, cantidad, sizeof(struct datos_ordenamiento_fragmento), &comparar_fragmentos);
  for (p = 0; p < cantidad; p ++) fragmentos_ordenados[p] = fragmentos[p].numero;
  free(fragmentos);
  return AVS_OK;
}

int comparar_fragmentos (const void * fragmento1, const void * fragmento2) {
  const struct datos_ordenamiento_fragmento * primero = fragmento1;
  const struct datos_ordenamiento_fragmento * segundo = fragmento2;
  return (primero -> volumen < segundo -> volumen) - (primero -> volumen > segundo -> volumen);
}

void generar_entradas_de_fragmento (const double * amplitudes, double volumen, unsigned cantidad, double * resultado) {
  unsigned p;
  for (p = 0; p < cantidad; p ++) resultado[p] = amplitudes[p] - volumen;
}
