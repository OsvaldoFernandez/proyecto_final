#include "proto.h"

int exportar_entrenador (Entrenador * entrenador, const wchar_t * archivo) {
  esperar_mutex(&(entrenador -> mutex_redes));
  unsigned * numeros_redes = malloc(sizeof(unsigned) * entrenador -> cantidad_redes);
  unsigned cantidad_exportaciones = 0;
  unsigned i;
  for (i = 0; i < entrenador -> cantidad_redes; i ++)
    if (debe_exportar(i[entrenador -> redes_entrenamiento]))
      numeros_redes[cantidad_exportaciones ++] = i;
  if (!cantidad_exportaciones) {
    ReleaseMutex(entrenador -> mutex_redes);
    free(numeros_redes);
    return AVS_NADA_PARA_EXPORTAR;
  }
  HANDLE fp;
  int rv = abrir_archivo(archivo, 1, &fp);
  if (rv) {
    ReleaseMutex(entrenador -> mutex_redes);
    free(numeros_redes);
    return rv;
  }
  unsigned long long posicion_archivo = (cantidad_exportaciones + 1) << 4;
  posicionar_archivo(fp, posicion_archivo, 0);
  SetEndOfFile(fp);
  struct parametros_exportacion * parametros = malloc(sizeof(struct parametros_exportacion) * cantidad_exportaciones);
  double suma_puntajes = 0;
  for (i = 0; i < cantidad_exportaciones; i ++) {
    rv = exportar_red(fp, entrenador -> redes_entrenamiento[numeros_redes[i]], &posicion_archivo, parametros + i);
    if (rv) goto fin;
    suma_puntajes += entrenador -> redes_entrenamiento[numeros_redes[i]] -> puntaje;
  }
  posicionar_archivo(fp, 0, 0);
  rv = exportar_parametros(fp, parametros, cantidad_exportaciones, entrenador, suma_puntajes / cantidad_exportaciones);
  fin:
  ReleaseMutex(entrenador -> mutex_redes);
  CloseHandle(fp);
  free(numeros_redes);
  free(parametros);
  return rv;
}

double ponderacion_desde_puntaje (double puntaje) {
  if (puntaje < 0.55) return 0;
  if (puntaje < 0.9) return puntaje - 0.55;
  if (puntaje < 0.95) return (puntaje * 240.0 - 431.0) * puntaje + 193.85;
  return 1.0 / ((puntaje * 100.0 - 215.0) * puntaje + 115.0);
}

int debe_exportar (RedEntrenamiento * red) {
  if (red -> puntaje <= 0.55) return 0;
  return (red -> estado == 2) || (red -> cantidad_OK > 0.9999);
}

int exportar_red (HANDLE archivo, RedEntrenamiento * red, unsigned long long * posicion, struct parametros_exportacion * parametros) {
  parametros -> posicion = *posicion;
  parametros -> plan = red -> plan;
  parametros -> ponderacion = ponderacion_desde_puntaje(red -> puntaje);
  int32_t * buffer;
  unsigned long long longitud = persistir_red_neuronal(red -> red, (uint32_t **) &buffer);
  if (!longitud) return AVS_SIN_MEMORIA;
  parametros -> longitud = longitud << 2;
  unsigned saltear = (longitud % 4) ? (4 - (longitud % 4)) << 2 : 0;
  int rv = escribir_enteros_en_archivo(archivo, longitud, buffer);
  free(buffer);
  if (rv != longitud) return AVS_FALLO_ESCRITURA_ARCHIVO;
  if (saltear) {
    posicionar_archivo(archivo, saltear, 1);
    SetEndOfFile(archivo);
  }
  *posicion = obtener_posicion_de_archivo(archivo);
  if (*posicion == -1ULL) *posicion = parametros -> posicion + parametros -> longitud + saltear;
  return AVS_OK;
}

int exportar_parametros (HANDLE archivo, struct parametros_exportacion * parametros, unsigned cantidad, Entrenador * entrenador, double puntaje_promedio) {
  unsigned me = 0, mv = 0;
  unsigned i;
  for (i = 0; i < entrenador -> cantidad_muestras; i ++) {
    if (entrenador -> muestras[i].tipo & AVP_MUESTRA_ENTRENAMIENTO) me ++;
    if (entrenador -> muestras[i].tipo & AVP_MUESTRA_VALIDACION) mv ++;
  }
  if (me > 65535) me = 65535;
  if (mv > 65535) mv = 65535;
  uint32_t * datos_exportacion = malloc((cantidad + 1) << 4);
  *datos_exportacion = 0x32564150;
  datos_exportacion[1] = cantidad;
  datos_exportacion[2] = (uint32_t) ldexp(puntaje_promedio, 32);
  datos_exportacion[3] = (mv << 16) | me;
  for (i = 0; i < cantidad; i ++) {
    datos_exportacion[(i << 2) + 4] = parametros[i].posicion >> 2;
    datos_exportacion[(i << 2) + 5] = parametros[i].longitud >> 2;
    datos_exportacion[(i << 2) + 6] = (parametros[i].ponderacion >= 16) ? 0xffffffff : (unsigned) ldexp(parametros[i].ponderacion, 28);
    datos_exportacion[(i << 2) + 7] = parametros[i].plan;
  }
  int rv = escribir_enteros_en_archivo(archivo, (cantidad + 1) << 2, datos_exportacion);
  free(datos_exportacion);
  if (rv != ((cantidad + 1) << 2)) return AVS_FALLO_ESCRITURA_ARCHIVO;
  return AVS_OK;
}
