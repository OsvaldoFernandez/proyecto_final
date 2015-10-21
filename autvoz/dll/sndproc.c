#include "proto.h"

int obtener_amplitudes_de_fragmentos (HANDLE archivoWAV, double *** amplitudes, double ** volumenes, unsigned * muestreo) {
  double * muestras = NULL;
  int cantidad_muestras = obtener_muestras_de_archivo(archivoWAV, &muestras, muestreo);
  if (cantidad_muestras < 0) return cantidad_muestras;
  if (*muestreo < 8000) {
    free(muestras);
    return AVS_MUESTREO_DEMASIADO_BAJO;
  } else if (*muestreo & 3) {
    free(muestras);
    return AVS_MUESTREO_NO_ES_MULTIPLO_DE_4_HZ;
  } else if (*muestreo > (cantidad_muestras << 1)) {
    free(muestras);
    return AVS_DURACION_MENOR_A_MEDIO_SEGUNDO;
  }
  unsigned muestras_por_fragmento = *muestreo >> 2;
  unsigned fragmentos = cantidad_muestras / muestras_por_fragmento;
  *amplitudes = calloc(sizeof(double *), fragmentos + 1);
  *volumenes = calloc(sizeof(double), fragmentos);
  if (!(*amplitudes && *volumenes)) {
    free(amplitudes);
    free(volumenes);
    free(muestras);
    return AVS_SIN_MEMORIA;
  }
  unsigned fragmento;
  for (fragmento = 0; fragmento < fragmentos; fragmento ++) {
    fragmento[*volumenes] = obtener_volumen_de_fragmento(muestras + fragmento * muestras_por_fragmento, muestras_por_fragmento);
    if (!(fragmento[*amplitudes] = obtener_amplitudes_de_fragmento(muestras + fragmento * muestras_por_fragmento, muestras_por_fragmento))) {
      free(muestras);
      destruir_amplitudes_de_fragmentos(*amplitudes);
      free(*volumenes);
      *amplitudes = NULL;
      *volumenes = NULL;
      return AVS_SIN_MEMORIA;
    }
  }
  free(muestras);
  return fragmentos;
}

void destruir_amplitudes_de_fragmentos (double ** amplitudes) {
  if (!amplitudes) return;
  double ** actual;
  for (actual = amplitudes; *actual; actual ++) free(*actual);
  free(amplitudes);
}

inline double obtener_amplitud_de_coeficiente (double complex coeficiente) {
  return 20.0 * log10(2.0 * cabs(coeficiente));
}

double * obtener_amplitudes_de_fragmento (const double * muestras, unsigned cantidad) {
  double complex * muestras_complejas = malloc(sizeof(double complex) * cantidad);
  if (!muestras_complejas) return NULL;
  unsigned i;
  for (i = 0; i < cantidad; i ++) muestras_complejas[i] = muestras[i];
  double * resultado = malloc(sizeof(double) * ((cantidad >> 1) + 1));
  double complex * transformada = DFT(muestras_complejas, cantidad);
  free(muestras_complejas);
  if (!(resultado && transformada)) {
    free(resultado);
    free(transformada);
    return NULL;
  }
  cantidad >>= 1;
  for (i = 0; i <= cantidad; i ++) resultado[i] = obtener_amplitud_de_coeficiente(transformada[i]);
  free(transformada);
  return resultado;
}

double obtener_volumen_de_fragmento (const double * muestras, unsigned cantidad) {
  double acum = 0;
  unsigned muestra;
  for (muestra = 0; muestra < cantidad; muestra ++) acum += muestras[muestra] * muestras[muestra];
  return 20.0 * log10(sqrt(acum / cantidad));
}
