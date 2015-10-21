#include "proto.h"

___DLL int avf_crear_autenticador (const wchar_t * archivo, void ** autenticador) {
  if (!autenticador) return AVS_PUNTERO_NULO;
  *autenticador = calloc(1, sizeof(Autenticador));
  if (!*autenticador) return AVS_SIN_MEMORIA;
  int rv = inicializar_autenticador(*((Autenticador **) autenticador), archivo);
  if (rv) {
    free(*autenticador);
    *autenticador = NULL;
    return rv;
  }
  return AVS_OK;
}

___DLL int avf_destruir_autenticador (void * autenticador) {
  if (!autenticador) return AVS_PUNTERO_NULO;
  volatile Autenticador * aut = autenticador;
  if (InterlockedCompareExchange((LONG volatile *) &(aut -> cantidad_usuarios), -1, 0)) return AVS_OBJETO_OCUPADO;
  destruir_autenticador(*aut);
  free((void *) aut);
  return AVS_OK;
}

___DLL int avf_autenticar_WAV (void * autenticador, const wchar_t * archivoWAV) {
  if (!(autenticador && archivoWAV)) return AVS_PUNTERO_NULO;
  HANDLE fp;
  int rv = abrir_archivo(archivoWAV, 0, &fp);
  if (rv) return rv;
  Autenticador * aut = autenticador;
  rv = incrementar_cantidad_usuarios(&(aut -> cantidad_usuarios));
  if (rv) {
    CloseHandle(fp);
    return rv;
  }
  int resultado;
  unsigned muestreo;
  double ** amplitudes = NULL;
  double * volumenes = NULL;
  int fragmentos = obtener_amplitudes_de_fragmentos(fp, &amplitudes, &volumenes, &muestreo);
  CloseHandle(fp);
  if (fragmentos < 0) {
    resultado = fragmentos;
    goto fin;
  }
  resultado = autenticar(aut, amplitudes, volumenes, fragmentos, muestreo);
  fin:
  destruir_amplitudes_de_fragmentos(amplitudes);
  free(volumenes);
  InterlockedDecrement((LONG volatile *) &(aut -> cantidad_usuarios));
  return resultado;
}

___DLL int avf_obtener_informacion_autenticador (void * autenticador, avt_informacion * informacion) {
  if (!(autenticador && informacion)) return AVS_PUNTERO_NULO;
  Autenticador * aut = autenticador;
  informacion -> numero_redes = aut -> cantidad_redes;
  informacion -> puntaje_promedio = (unsigned) (aut -> puntaje_promedio * 1e6);
  informacion -> muestras_entrenamiento = aut -> muestras_entrenamiento;
  informacion -> muestras_validacion = aut -> muestras_validacion;
  return AVS_OK;
}

___DLL int avf_crear_entrenador (void ** entrenador) {
  if (!entrenador) return AVS_PUNTERO_NULO;
  *entrenador = malloc(sizeof(Entrenador));
  if (!*entrenador) return AVS_SIN_MEMORIA;
  inicializar_entrenador(*((Entrenador **) entrenador));
  return AVS_OK;
}

___DLL int avf_destruir_entrenador (void * entrenador) {
  if (!entrenador) return AVS_PUNTERO_NULO;
  volatile Entrenador * ent = entrenador;
  int rv = puede_destruir_entrenador(ent);
  if (rv) return rv;
  destruir_entrenador(*ent);
  free((void *) ent);
  return AVS_OK;
}

___DLL int avf_agregar_muestra_WAV (void * entrenador, int persona, int parametros, const wchar_t * archivoWAV) {
  if (!(entrenador && archivoWAV)) return AVS_PUNTERO_NULO;
  if ((parametros < 1) || (parametros > 3)) return AVS_ARGUMENTO_NO_VALIDO;
  volatile Entrenador * ent = entrenador;
  int resultado_operacion = obtener_entrenador_sin_bloquear(ent);
  if (resultado_operacion) return resultado_operacion;
  HANDLE fp = NULL;
  double ** amplitudes = NULL;
  double * volumenes = NULL;
  unsigned muestreo;
  if (ent -> tiempo_ejecucion >= 0) {
    resultado_operacion = AVS_ENTRENADOR_NO_EN_CONSTRUCCION;
    goto fin;
  }
  resultado_operacion = abrir_archivo(archivoWAV, 0, &fp);
  if (resultado_operacion) goto fin;
  int fragmentos = obtener_amplitudes_de_fragmentos(fp, &amplitudes, &volumenes, &muestreo);
  CloseHandle(fp);
  if (fragmentos < 0) {
    resultado_operacion = fragmentos;
    goto fin;
  }
  resultado_operacion = agregar_muestra_a_entrenador(ent, parametros, persona, fragmentos, muestreo, amplitudes, volumenes);
  if (resultado_operacion < 0) goto fin;
  InterlockedDecrement((LONG volatile *) &(ent -> cantidad_usuarios));
  return resultado_operacion;
  fin:
  InterlockedDecrement((LONG volatile *) &(ent -> cantidad_usuarios));
  destruir_amplitudes_de_fragmentos(amplitudes);
  free(volumenes);
  return resultado_operacion;
}

___DLL int avf_eliminar_muestra (void * entrenador, int muestra) {
  if (!entrenador) return AVS_PUNTERO_NULO;
  if (muestra < 0) return AVS_ARGUMENTO_NO_VALIDO;
  volatile Entrenador * ent = entrenador;
  int resultado_operacion = obtener_entrenador_sin_bloquear(ent);
  if (resultado_operacion) return resultado_operacion;
  if (ent -> tiempo_ejecucion >= 0) {
    resultado_operacion = AVS_ENTRENADOR_NO_EN_CONSTRUCCION;
    goto fin;
  }
  resultado_operacion = eliminar_muestra_de_entrenador(ent, muestra);
  fin:
  InterlockedDecrement((LONG volatile *) &(ent -> cantidad_usuarios));
  return resultado_operacion;
}

___DLL int avf_iniciar_entrenamiento (void * entrenador) {
  if (!entrenador) return AVS_PUNTERO_NULO;
  volatile Entrenador * ent = entrenador;
  int rv = bloquear_entrenador(ent);
  if (rv) return rv;
  if (ent -> thread_ejecucion) {
    ent -> bloqueado = 0;
    return AVS_ENTRENADOR_EJECUTANDO;
  }
  if ((ent -> tiempo_ejecucion < 0) && (rv = construir_entrenador((Entrenador *) ent))) {
    ent -> bloqueado = 0;
    return rv;
  }
  rv = iniciar_entrenador((Entrenador *) ent);
  ent -> bloqueado = 0;
  return rv;
}

___DLL int avf_detener_entrenamiento (void * entrenador) {
  if (!entrenador) return AVS_PUNTERO_NULO;
  volatile Entrenador * ent = entrenador;
  int rv = bloquear_entrenador(ent);
  if (rv) return rv;
  if (!(ent -> thread_ejecucion)) {
    ent -> bloqueado = 0;
    return AVS_ENTRENADOR_NO_ESTA_EJECUTANDO;
  }
  rv = detener_entrenador((Entrenador *) ent);
  ent -> bloqueado = 0;
  return rv;
}

___DLL int avf_estado_entrenamiento (void * entrenador, avt_estado * estado) {
  if (!(entrenador && estado)) return AVS_PUNTERO_NULO;
  volatile Entrenador * ent = entrenador;
  int rv = bloquear_entrenador(ent);
  if (rv) return rv;
  if (ent -> tiempo_ejecucion < 0)
    rv = estado_entrenador_en_construccion((Entrenador *) ent, estado);
  else if (ent -> thread_ejecucion)
    rv = estado_entrenador_ejecutando((Entrenador *) ent, estado);
  else
    rv = estado_entrenador_listo((Entrenador *) ent, estado);
  ent -> bloqueado = 0;
  return rv;
}

___DLL int avf_exportar_entrenamiento (void * entrenador, const wchar_t * archivo) {
  if (!entrenador) return AVS_PUNTERO_NULO;
  volatile Entrenador * ent = entrenador;
  int rv = bloquear_entrenador(ent);
  if (rv) return rv;
  if (ent -> thread_ejecucion) {
    ent -> bloqueado = 0;
    return AVS_ENTRENADOR_EJECUTANDO;
  } else if (ent -> tiempo_ejecucion < 0) {
    ent -> bloqueado = 0;
    return AVS_ENTRENADOR_EN_CONSTRUCCION;
  }
  rv = exportar_entrenador((Entrenador *) ent, archivo);
  ent -> bloqueado = 0;
  return rv;
}
