#include "proto.h"

double complex * DFT (const double complex * valores, unsigned cantidad) {
  if (!cantidad) return NULL;
  if (cantidad == 1) {
    double complex * resultado = malloc(sizeof(double complex));
    if (!resultado) return NULL;
    *resultado = *valores;
    return resultado;
  }
  if (cantidad <= 7) return DFT_directa(valores, cantidad);
  if (!(cantidad & 1)) return FFT_factorizacion(valores, 2, cantidad >> 1);
  unsigned n;
  for (n = 3; (n * n) < cantidad; n += 2) if (!(cantidad % n)) return FFT_factorizacion(valores, n, cantidad / n);
  return DFT_directa(valores, cantidad);
}

double complex * DFT_directa (const double complex * valores, unsigned cantidad) {
  unsigned n, k;
  double complex valor;
  double complex * resultado = malloc(sizeof(double complex) * cantidad);
  if (!resultado) return NULL;
  for (k = 0; k < cantidad; k ++) {
    valor = 0;
    for (n = 0; n < cantidad; n ++) valor += valores[n] * cexp(-2.0 * pi * I * n * k / cantidad);
    resultado[k] = valor;
  }
  return resultado;
}

double complex * FFT_factorizacion (const double complex * valores, unsigned factor1, unsigned factor2) {
  double complex ** temporales = malloc(sizeof(double complex) * factor1);
  double complex * parametros = malloc(sizeof(double complex) * factor2);
  double complex * resultado = malloc(sizeof(double complex) * (factor1 * factor2));
  double complex * transformada;
  if (!(temporales && parametros && resultado)) return NULL;
  unsigned n1, n2;
  for (n1 = 0; n1 < factor1; n1 ++) {
    for (n2 = 0; n2 < factor2; n2 ++) parametros[n2] = valores[n2 * factor1 + n1];
    if (!(temporales[n1] = DFT(parametros, factor2))) {
      for (n2 = 0; n2 < n1; n2 ++) free(temporales[n2]);
      free(temporales);
      free(parametros);
      free(resultado);
      return NULL;
    }
    for (n2 = 0; n2 < factor2; n2 ++) temporales[n1][n2] *= cexp(-2.0 * I * pi * n1 * n2 / (factor1 * factor2));
  }
  free(parametros);
  parametros = malloc(sizeof(double complex) * factor1);
  if (!parametros) {
    for (n1 = 0; n1 < factor1; n1 ++) free(temporales[n1]);
    free(temporales);
    free(resultado);
    return NULL;
  }
  for (n2 = 0; n2 < factor2; n2 ++) {
    for (n1 = 0; n1 < factor1; n1 ++) parametros[n1] = temporales[n1][n2];
    transformada = DFT(parametros, factor1);
    if (!transformada) {
      for (n1 = 0; n1 < factor1; n1 ++) free(temporales[n1]);
      free(temporales);
      free(resultado);
      return NULL;
    }
    for (n1 = 0; n1 < factor1; n1 ++) resultado[n1 * factor2 + n2] = transformada[n1];
    free(transformada);
  }
  free(parametros);
  for (n1 = 0; n1 < factor1; n1 ++) free(temporales[n1]);
  free(temporales);
  return resultado;
}
