#include "proto.h"

int inicializar_autenticador (Autenticador * autenticador, const wchar_t * archivo_datos) {
  HANDLE fp;
  int rv = abrir_archivo(archivo_datos, 0, &fp);
  if (rv) return rv;
  int32_t encabezado[4];
  if (leer_archivo(encabezado, 16, fp) != 16) {
    CloseHandle(fp);
    return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  }
  int invertir_enteros = 0;
  int version = numero_version(*encabezado);
  if (version < 0) {
    invertir_enteros = 1;
    encabezado[1] = invertir_entero(encabezado[1]);
    encabezado[2] = invertir_entero(encabezado[2]);
    encabezado[3] = invertir_entero(encabezado[3]);
  } else if ((!version) || (abs(version) > VERSION_PAV_SOPORTADA)) {
    CloseHandle(fp);
    return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  }
  autenticador -> cantidad_redes = encabezado[1];
  autenticador -> puntaje_promedio = ldexp((unsigned) encabezado[2], -32);
  autenticador -> muestras_entrenamiento = encabezado[3] & 0xffff;
  autenticador -> muestras_validacion = (encabezado[3] >> 16) & 0xffff;
  autenticador -> redes = calloc(autenticador -> cantidad_redes, sizeof(RNA *));
  autenticador -> pesos = malloc(sizeof(double) * autenticador -> cantidad_redes);
  autenticador -> planes = malloc(sizeof(unsigned) * autenticador -> cantidad_redes);
  uint32_t * parametros = malloc(autenticador -> cantidad_redes << 4);
  if (!(autenticador -> redes && autenticador -> pesos && autenticador -> planes && parametros)) {
    destruir_autenticador(*autenticador);
    free(parametros);
    CloseHandle(fp);
    return AVS_SIN_MEMORIA;
  }
  if (leer_enteros_de_archivo(fp, autenticador -> cantidad_redes << 2, (int32_t *) parametros, invertir_enteros) != (autenticador -> cantidad_redes << 2)) {
    destruir_autenticador(*autenticador);
    free(parametros);
    CloseHandle(fp);
    return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  }
  unsigned red;
  for (red = 0; red < autenticador -> cantidad_redes; red ++) {
    red[autenticador -> planes] = parametros[3 + (red << 2)];
    red[autenticador -> pesos] = ldexp(parametros[2 + (red << 2)], -28);
    rv = cargar_red(fp, parametros[red << 2], parametros[1 + (red << 2)], autenticador -> redes + red, invertir_enteros);
    if (rv) {
      free(parametros);
      destruir_autenticador(*autenticador);
      CloseHandle(fp);
      return rv;
    }
  }
  free(parametros);
  CloseHandle(fp);
  return AVS_OK;
}

void destruir_autenticador (Autenticador aut) {
  unsigned red;
  for (red = 0; red < aut.cantidad_redes; red ++) destruir_red_neuronal(red[aut.redes]);
  free(aut.redes);
  free(aut.pesos);
  free(aut.planes);
}

int cargar_red (HANDLE archivo, unsigned posicion, unsigned tamano, RNA ** red, int invertir_enteros) {
  if (posicionar_archivo(archivo, ((unsigned long long) posicion) << 2, 0)) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  uint32_t * buffer = malloc(((unsigned long long) tamano) << 2);
  if (!buffer) return AVS_SIN_MEMORIA;
  if (leer_enteros_de_archivo(archivo, tamano, (int32_t *) buffer, invertir_enteros) != tamano) {
    free(buffer);
    return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  }
  *red = cargar_red_neuronal(buffer, tamano);
  free(buffer);
  if (!*red) return AVS_FORMATO_ARCHIVO_NO_VALIDO;
  return AVS_OK;
}

int numero_version (uint32_t encabezado) {
  unsigned char determinante;
  int inversion;
  if ((encabezado & 0xffffff) == 0x564150) {
    determinante = encabezado >> 24;
    inversion = 0;
  } else if ((encabezado & 0xffffff00U) == 0x50415600) {
    determinante = encabezado & 0xff;
    inversion = 1;
  } else
    return 0;
  if (determinante <= 0x30) return 0;
  if (determinante <= 0x39) return (determinante - 0x30) * (inversion ? -1 : 1);
  if (((determinante & 0xc0) == 0x40) && (determinante & 0x1f) && ((determinante & 0x1f) <= 26))
    return (9 + (determinante & 0x1f)) * (inversion ? -1 : 1);
  return 0;
}
