#include "proto.h"

inline double transferencia_RNA (double n) {
  return 1.0 / (1.0 + exp(-n));
}

inline double transferencia_derivada_RNA (double n) {
  double q = transferencia_RNA(n);
  return q * (1.0 - q);
}

RNA * crear_red_neuronal (unsigned niveles, const unsigned * nodos, unsigned entradas, RNG * generador) {
  if (!(niveles && entradas)) return NULL;
  if ((niveles > 1) && !nodos) return NULL;
  RNA * rna = calloc(1, sizeof(RNA));
  rna -> niveles = niveles;
  if (!(
    (rna -> nodos = calloc(niveles + 1, sizeof(unsigned))) &&
    (rna -> pesos = calloc(niveles, sizeof(double **)))
  )) {
    destruir_red_neuronal(rna);
    return NULL;
  }
  *(rna -> nodos) = entradas;
  unsigned actual, nivel, i, k;
  for (nivel = 1; nivel <= niveles; nivel ++) {
    nivel[rna -> nodos] = actual = (nivel == niveles) ? 1 : nodos[nivel - 1];
    if (!actual) {
      destruir_red_neuronal(rna);
      return NULL;
    }
    if (!(rna -> pesos[nivel - 1] = calloc(entradas, sizeof(double *)))) {
      destruir_red_neuronal(rna);
      return NULL;
    }
    for (i = 0; i < entradas; i ++) {
      if (!(rna -> pesos[nivel - 1][i] = malloc(actual * sizeof(double)))) {
        destruir_red_neuronal(rna);
        return NULL;
      }
      for (k = 0; k < actual; k ++) rna -> pesos[nivel - 1][i][k] = (aleatorio(generador) - 0.5) * nivel / 5;
    }
    entradas = actual;
  }
  return rna;
}

void destruir_red_neuronal (RNA * rna) {
  if (!rna) return;
  unsigned n, i;
  if (rna -> pesos) {
    for (n = 0; n < rna -> niveles; n ++) 
      if (rna -> pesos[n]) {
        for (i = 0; i < rna -> nodos[n]; i ++) free(rna -> pesos[n][i]);
        free(rna -> pesos[n]);
      }
    free(rna -> pesos);
  }
  free(rna -> nodos);
  free(rna);
}

void reinicializar_red_neuronal (RNA * rna, double factor_pesos, RNG * generador) {
  if (!rna) return;
  unsigned nivel, i, k;
  for (nivel = 0; nivel < rna -> niveles; nivel ++) for (i = 0; i < nivel[rna -> nodos]; i ++) for (k = 0; k < rna -> nodos[nivel + 1]; k ++)
    rna -> pesos[nivel][i][k] = (aleatorio(generador) - 0.5) * nivel * 2.0 * factor_pesos;
}

double procesar_entradas_RNA (RNA * rna, const double * entradas) {
  if (!(rna && entradas)) return NAN;
  double ** nodos = calloc(1 + rna -> niveles, sizeof(double *));
  double r;
  if (!nodos) return NAN;
  unsigned n, i, k;
  for (n = 0; n <= rna -> niveles; n ++) {
    nodos[n] = malloc(sizeof(double) * n[rna -> nodos]);
    if (!(nodos[n])) {
      while (n --) free(nodos[n]);
      free(nodos);
      return NAN;
    }
    if (n)
      for (i = 0; i < n[rna -> nodos]; i ++) {
        r = 0;
        for (k = 0; k < rna -> nodos[n - 1]; k ++) r += rna -> pesos[n - 1][k][i] * nodos[n - 1][k];
        nodos[n][i] = transferencia_RNA(r);
      }
    else
      memcpy(*nodos, entradas, *(rna -> nodos) * sizeof(double));
  }
  r = *(nodos[rna -> niveles]);
  for (n = 0; n <= rna -> niveles; n ++) free(nodos[n]);
  free(nodos);
  return r;
}

double entrenar_entradas_RNA (RNA * rna, const double * entradas, double salida_esperada, double tasa) {
  if (!(rna && entradas)) return NAN;
  double ** nodos = calloc(1 + rna -> niveles, sizeof(double *));
  double ** derivadas = calloc(rna -> niveles, sizeof(double *));
  double ** entrada_neta = calloc(rna -> niveles, sizeof(double *));
  double r, err;
  if (!(nodos && derivadas && entrada_neta)) {
    free(nodos);
    free(derivadas);
    return NAN;
  }
  unsigned n, i, k;
  for (n = 0; n <= rna -> niveles; n ++) {
    nodos[n] = malloc(sizeof(double) * n[rna -> nodos]);
    if (!(nodos[n])) {
      while (n --) free(nodos[n]);
      free(nodos);
      free(derivadas);
      return NAN;
    }
    if (n) {
      entrada_neta[n - 1] = malloc(sizeof(double) * n[rna -> nodos]);
      derivadas[n - 1] = malloc(sizeof(double) * n[rna -> nodos]);
      if (!(entrada_neta[n - 1] && derivadas[n - 1])) {
        free(nodos[n]);
        while (n --) {
          free(nodos[n]);
          free(derivadas[n]);
          free(entrada_neta[n]);
        }
        free(nodos);
        free(entrada_neta);
        free(derivadas);
        return NAN;
      }
      for (i = 0; i < n[rna -> nodos]; i ++) {
        r = 0;
        for (k = 0; k < rna -> nodos[n - 1]; k ++) r += rna -> pesos[n - 1][k][i] * nodos[n - 1][k];
        entrada_neta[n - 1][i] = r;
        nodos[n][i] = transferencia_RNA(r);
      }
    }
    else
      memcpy(*nodos, entradas, *(rna -> nodos) * sizeof(double));
  }
  r = *(nodos[rna -> niveles]);
  err = (salida_esperada - r) * (salida_esperada - r);
  for (n = rna -> niveles - 1; n < rna -> niveles; n --) for (i = 0; i < rna -> nodos[n + 1]; i ++) {
    if (n == (rna -> niveles - 1))
      r = derivadas[n][i] = r - salida_esperada;
    else {
      r = 0;
      for (k = 0; k < rna -> nodos[n + 2]; k ++) r += derivadas[n + 1][k] * transferencia_derivada_RNA(entrada_neta[n + 1][k]) * rna -> pesos[n + 1][i][k];
      derivadas[n][i] = r;
    }
    for (k = 0; k < n[rna -> nodos]; k ++) rna -> pesos[n][k][i] -= tasa * derivadas[n][i] * transferencia_derivada_RNA(entrada_neta[n][i]) * nodos[n][k];
  }
  for (n = 0; n < rna -> niveles; n ++) {
    free(nodos[n]);
    free(derivadas[n]);
    free(entrada_neta[n]);
  }
  free(nodos[rna -> niveles]);
  free(nodos);
  free(derivadas);
  free(entrada_neta);
  return err;
}

unsigned long long persistir_red_neuronal (RNA * rna, uint32_t ** valores) {
  if (!(rna && valores)) return 0;
  size_t longitud = 1 + rna -> niveles;
  unsigned nivel;
  for (nivel = 0; nivel < rna -> niveles; nivel ++) longitud += 2 * nivel[rna -> nodos] * rna -> nodos[nivel + 1];
  *valores = malloc(longitud * sizeof(uint32_t));
  if (!*valores) return 0;
  unsigned * actual = *valores;
  *(actual ++) = rna -> niveles;
  for (nivel = 0; nivel < rna -> niveles; nivel ++) *(actual ++) = nivel[rna -> nodos];
  unsigned i, k;
  long long mantisa;
  int exponente;
  unsigned char signo;
  for (nivel = 0; nivel < rna -> niveles; nivel ++) for (i = 0; i < nivel[rna -> nodos]; i ++) for (k = 0; k < rna -> nodos[nivel + 1]; k ++) {
    mantisa = ldexp(frexp(rna -> pesos[nivel][i][k], &exponente), 53);
    if (signbit(rna -> pesos[nivel][i][k])) {
      mantisa = -mantisa;
      signo = 1;
    } else
      signo = 0;
    exponente += 1022;
    if (exponente <= 0) {
      mantisa >>= 1 - exponente;
      exponente = 0;
    } else
      mantisa -= 0x10000000000000ULL;
    if (exponente > 2046) exponente = 2047;
    *(actual ++) = mantisa & 0xffffffffU;
    *(actual ++) = ((mantisa >> 32) & 0xfffff) | (exponente << 20) | (signo ? 0x80000000u : 0);
  }
  return longitud;
}

RNA * cargar_red_neuronal (const uint32_t * valores, unsigned cantidad) {
  if (!(valores && cantidad)) return NULL;
  RNA * rna = calloc(1, sizeof(RNA));
  if (!rna) return NULL;
  const uint32_t * actual = valores;
  const uint32_t * limite = valores + cantidad;
  rna -> niveles = *(actual ++);
  rna -> nodos = malloc(sizeof(unsigned) * (1 + rna -> niveles));
  rna -> pesos = calloc(rna -> niveles, sizeof(double **));
  if (!(rna -> nodos)) {
    free(rna);
    return NULL;
  }
  unsigned nivel;
  for (nivel = 0; nivel < rna -> niveles; nivel ++) {
    if (actual >= limite) {
      free(rna -> nodos);
      free(rna -> pesos);
      free(rna);
      return NULL;
    }
    nivel[rna -> nodos] = *(actual ++);
  }
  rna -> nodos[rna -> niveles] = 1;
  unsigned i, k;
  unsigned long long mantisa;
  int exponente;
  unsigned char signo;
  for (nivel = 0; nivel < rna -> niveles; nivel ++) {
    if (!(nivel[rna -> pesos] = calloc(nivel[rna -> nodos], sizeof(double *)))) {
      destruir_red_neuronal(rna);
      return NULL;
    }
    for (i = 0; i < nivel[rna -> nodos]; i ++) {
      if (!(rna -> pesos[nivel][i] = malloc(rna -> nodos[nivel + 1] * sizeof(double)))) {
        destruir_red_neuronal(rna);
        return NULL;
      }
      for (k = 0; k < rna -> nodos[nivel + 1]; k ++) {
        mantisa = *(actual ++);
        if (actual >= limite) {
          destruir_red_neuronal(rna);
          return NULL;
        }
        mantisa |= (((unsigned long long) *actual) & 0xfffff) << 32;
        exponente = (*actual >> 20) & 0x7ff;
        signo = *actual >> 31;
        actual ++;
        if (exponente)
          mantisa |= 0x10000000000000;
        else
          exponente = 1;
        rna -> pesos[nivel][i][k] = ldexp(mantisa, exponente - 1075) * (signo ? -1.0 : 1.0);
      }
    }
  }
  return rna;
}
