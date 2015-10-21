#include "proto.h"

int obtener_muestras_de_archivo (HANDLE archivoWAV, double ** muestras, unsigned * muestreo) {
  struct encabezado_archivo_WAV encabezado;
  int rv;
  if (rv = obtener_encabezado_de_archivo(archivoWAV, &encabezado)) return rv;
  if (muestreo) *muestreo = encabezado.muestreo;
  unsigned cantidad_muestras = encabezado.longitud / encabezado.bytes_por_muestra;
  *muestras = malloc(sizeof(double) * cantidad_muestras);
  if (!*muestras) return AVS_SIN_MEMORIA;
  unsigned tamano_parte = encabezado.muestreo * encabezado.bytes_por_muestra / 4;
  unsigned parte, remanente;
  void * buf = malloc(tamano_parte);
  if (!buf) {
    free(*muestras);
    return AVS_SIN_MEMORIA;
  }
  for (parte = 0, remanente = encabezado.longitud; remanente >= tamano_parte; parte ++, remanente -= tamano_parte) {
    if (leer_archivo(buf, tamano_parte, archivoWAV) != tamano_parte) {
      free(buf);
      free(*muestras);
      return AVS_FORMATO_ARCHIVO_NO_VALIDO;
    }
    if (rv = procesar_parte_de_archivo(buf, tamano_parte, encabezado, *muestras + parte * (encabezado.muestreo >> 2))) {
      free(buf);
      free(*muestras);
      return rv;
    }
  }
  if (!remanente) {
    free(buf);
    return cantidad_muestras;
  }
  if (leer_archivo(buf, remanente, archivoWAV) != remanente) {
    free(buf);
    free(*muestras);
    return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  }
  rv = procesar_parte_de_archivo(buf, remanente, encabezado, *muestras + parte * (encabezado.muestreo >> 2));
  free(buf);
  if (rv) {
    free(*muestras);
    return rv;
  }
  return cantidad_muestras;
}

int obtener_encabezado_de_archivo (HANDLE fp, struct encabezado_archivo_WAV * resultado) {
  if (!resultado) return -1;
  unsigned char buf[16];
  if (leer_archivo(buf, 12, fp) != 12) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  if (memcmp(buf, "RIFF", 4) || memcmp(buf + 8, "WAVE", 4)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  int rv;
  while (1) {
    rv = siguiente_fragmento(fp);
    if (!rv) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
    if (rv == 'fmt ') break;
    if (saltear_fragmento(fp)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  }
  rv = obtener_valor_de_archivo(fp, 4);
  if (rv < 16) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  if (obtener_valor_de_archivo(fp, 2) != 1) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  resultado -> canales = obtener_valor_de_archivo(fp, 2);
  if (!(resultado -> canales) || (resultado -> canales == (unsigned short) -1)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  resultado -> muestreo = obtener_valor_de_archivo(fp, 4);
  if (!(resultado -> muestreo) || (resultado -> muestreo == (unsigned) -1)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  if (posicionar_archivo(fp, 4, 1)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  resultado -> bytes_por_muestra = obtener_valor_de_archivo(fp, 2);
  if (!(resultado -> bytes_por_muestra) || (resultado -> bytes_por_muestra == (unsigned short) -1)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  resultado -> bits_significativos = obtener_valor_de_archivo(fp, 2);
  if (!(resultado -> bits_significativos) || (resultado -> bits_significativos == (unsigned short) -1)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  if ((rv != 16) && (posicionar_archivo(fp, rv - 16, 1))) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  while (1) {
    rv = siguiente_fragmento(fp);
    if (!rv) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
    if (rv == 'data') break;
    if (saltear_fragmento(fp)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  }
  resultado -> longitud = obtener_valor_de_archivo(fp, 4);
  if (!(resultado -> longitud) || (resultado -> longitud == (unsigned) -1)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  if (resultado -> bytes_por_muestra % resultado -> canales) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  if (resultado -> muestreo & 3) return AVS_MUESTREO_NO_ES_MULTIPLO_DE_4_HZ;
  return AVS_OK;
}

int saltear_fragmento (HANDLE fp) {
  unsigned cantidad = obtener_valor_de_archivo(fp, 4);
  if (cantidad == (unsigned) -1) return 1;
  if (posicionar_archivo(fp, cantidad, 1)) return 1;
  return 0;
}

int siguiente_fragmento (HANDLE fp) {
  unsigned char buf[4];
  if (leer_archivo(buf, 4, fp) != 4) return 0;
  return (((int) *buf) << 24) | (((int) buf[1]) << 16) | (((int) buf[2]) << 8) | ((int) buf[3]);
}

int procesar_parte_de_archivo (const void * contenido, unsigned longitud, struct encabezado_archivo_WAV encabezado, double * resultado) {
  unsigned muestras = longitud / encabezado.bytes_por_muestra;
  if (!muestras) return AVS_OK;
  unsigned short canal;
  const unsigned char * actual = contenido;
  unsigned muestra;
  unsigned valor;
  unsigned char bytes = encabezado.bytes_por_muestra / encabezado.canales;
  unsigned char pos;
  if (encabezado.bits_significativos > 30) encabezado.bits_significativos = 30;
  double muestra_actual;
  for (muestra = 0; muestra < muestras; muestra ++) {
    muestra_actual = 0;
    for (canal = 0; canal < encabezado.canales; canal ++) {
      valor = 0;
      for (pos = min(bytes - 1, 3); pos < bytes; pos --) valor = (valor << 8) | actual[pos];
      actual += bytes;
      if (bytes == 1) valor ^= 128;
      valor >>= min(bytes, 4) * 8 - encabezado.bits_significativos;
      if (valor & (1 << (encabezado.bits_significativos - 1))) valor -= 1 << encabezado.bits_significativos;
      muestra_actual += ldexp((double) (int) valor, 1 - (int) encabezado.bits_significativos);
    }
    if (encabezado.canales > 1) muestra_actual /= encabezado.canales;
    resultado[muestra] = muestra_actual;
  }
  return AVS_OK;
}
