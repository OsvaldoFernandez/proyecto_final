#include "proto.h"

int abrir_archivo (const wchar_t * nombre, unsigned char modo_apertura, HANDLE * resultado) {
  /*
      modos:
      0: solo lectura
      1: truncado y escritura
  */
  DWORD acceso, comparticion, creacion;
  switch (modo_apertura) {
    case 0:
      acceso = GENERIC_READ;
      comparticion = FILE_SHARE_READ;
      creacion = OPEN_EXISTING;
      break;
    case 1:
      acceso = GENERIC_WRITE;
      comparticion = 0;
      creacion = CREATE_ALWAYS;
      break;
    default:
      return AVS_ERROR_INTERNO;
  }
  *resultado = CreateFileW(nombre, acceso, comparticion, NULL, creacion, FILE_ATTRIBUTE_NORMAL, NULL);
  if (*resultado == INVALID_HANDLE_VALUE) return AVS_ARCHIVO_INACCESIBLE;
  return AVS_OK;
}

long long leer_archivo (void * buffer, unsigned long long cantidad, HANDLE archivo) {
  uint32_t lectura_real;
  uint32_t lectura_actual;
  char * buffer_actual = buffer;
  int rv;
  while (cantidad) {
    lectura_actual = min(cantidad, 0x1000000);
    rv = ReadFile(archivo, buffer_actual, lectura_actual, (PDWORD) &lectura_real, NULL);
    if (!rv) return -1;
    if (!lectura_real) break;
    cantidad -= lectura_real;
    buffer_actual += lectura_real;
  }
  return buffer_actual - ((char *) buffer);
}

long long escribir_archivo (const void * buffer, unsigned long long cantidad, HANDLE archivo) {
  uint32_t escritura_real;
  uint32_t escritura_actual;
  const char * buffer_actual = buffer;
  int rv;
  while (cantidad) {
    escritura_actual = min(cantidad, 0x1000000);
    rv = WriteFile(archivo, buffer_actual, escritura_actual, (PDWORD) &escritura_real, NULL);
    if (!(rv && escritura_real)) break;
    cantidad -= escritura_real;
    buffer_actual += escritura_real;
  }
  return buffer_actual - ((const char *) buffer);
}

int posicionar_archivo (HANDLE archivo, long long posicion, unsigned char desde) {
  LARGE_INTEGER pos;
  pos.QuadPart = posicion;
  if (!SetFilePointerEx(archivo, pos, NULL, desde)) return -1;
  return 0;
}

uintmax_t obtener_valor_de_archivo (HANDLE fp, unsigned char longitud) {
  if (longitud > sizeof(uintmax_t)) return UINTMAX_MAX;
  if (!longitud) return 0;
  unsigned char buf[sizeof(uintmax_t)];
  if (leer_archivo(buf, longitud, fp) != longitud) return UINTMAX_MAX;
  uintmax_t rt = 0;
  unsigned i;
  for (i = 0; i < longitud; i ++) rt += ((uintmax_t) buf[i]) << (8 * i);
  return rt;
}

int leer_enteros_de_archivo (HANDLE archivo, unsigned cantidad, int32_t * resultado, int invertir) {
  long long lectura = leer_archivo(resultado, ((unsigned long long) cantidad) << 2, archivo);
  if (lectura < 1) return lectura;
  unsigned cantidad_real = lectura >> 2;
  if (!invertir) return cantidad_real;
  unsigned actual;
  for (actual = 0; actual < cantidad_real; actual ++) resultado[actual] = invertir_entero(resultado[actual]);
  return cantidad_real;
}

inline int32_t invertir_entero (int32_t valor) {
  return ((valor & 0xff) << 24) | (((valor >> 8) & 0xff) << 16) | (((valor >> 16) & 0xff) << 8) | ((valor >> 24) & 0xff);
}

int escribir_enteros_en_archivo (HANDLE archivo, unsigned cantidad, const int32_t * enteros) {
  long long escritura = escribir_archivo(enteros, ((unsigned long long) cantidad) << 2, archivo);
  if (escritura < 1) return escritura;
  return escritura >> 2;
}

unsigned long long obtener_posicion_de_archivo (HANDLE fp) {
  LARGE_INTEGER cero, resultado;
  cero.QuadPart = 0;
  if (!SetFilePointerEx(fp, cero, &resultado, 1)) return -1ULL;
  return resultado.QuadPart;
}
