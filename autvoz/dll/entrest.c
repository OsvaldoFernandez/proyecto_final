#include "proto.h"

int estado_entrenador_en_construccion (Entrenador * entrenador, avt_estado * estado) {
  estado -> estado_general = -1;
  estado -> redes_generadas = estado -> redes_satisfactorias = estado -> redes_descartadas = 0;
  estado -> mejor_puntaje = estado -> puntaje_promedio = 0;
  estado -> tiempo_transcurrido = 0;
  esperar_mutex(&(entrenador -> mutex_muestras));
  contar_muestras(entrenador, estado);
  ReleaseMutex(entrenador -> mutex_muestras);
  return AVS_OK;
}

int estado_entrenador_listo (Entrenador * entrenador, avt_estado * estado) {
  estado -> estado_general = 0;
  estado -> tiempo_transcurrido = entrenador -> tiempo_ejecucion;
  return estado_entrenador_listo_ejecutando(entrenador, estado);
}

int estado_entrenador_ejecutando (Entrenador * entrenador, avt_estado * estado) {
  estado -> estado_general = entrenador -> cantidad_personas;
  estado -> tiempo_transcurrido = entrenador -> tiempo_ejecucion + GetTickCount() - entrenador -> momento_iniciado;
  return estado_entrenador_listo_ejecutando(entrenador, estado);
}

void contar_muestras (Entrenador * entrenador, avt_estado * estado) {
  estado -> muestras_totales = entrenador -> cantidad_muestras;
  estado -> muestras_entrenamiento_negativas = estado -> muestras_entrenamiento_positivas =
    estado -> muestras_validacion_negativas = estado -> muestras_validacion_positivas = 0;
  unsigned muestra;
  for (muestra = 0; muestra < entrenador -> cantidad_muestras; muestra ++)
    if (muestra[entrenador -> muestras].persona) {
      if (muestra[entrenador -> muestras].tipo & AVP_MUESTRA_ENTRENAMIENTO) estado -> muestras_entrenamiento_negativas ++;
      if (muestra[entrenador -> muestras].tipo & AVP_MUESTRA_VALIDACION) estado -> muestras_validacion_negativas ++;
    } else {
      if (muestra[entrenador -> muestras].tipo & AVP_MUESTRA_ENTRENAMIENTO) estado -> muestras_entrenamiento_positivas ++;
      if (muestra[entrenador -> muestras].tipo & AVP_MUESTRA_VALIDACION) estado -> muestras_validacion_positivas ++;
    }
}

int estado_entrenador_listo_ejecutando (Entrenador * entrenador, avt_estado * estado) {
  contar_muestras(entrenador, estado);
  estado -> mejor_puntaje = estado -> puntaje_promedio = 0;
  estado -> redes_satisfactorias = 0;
  double suma = 0;
  double mejor = 0;
  unsigned red;
  esperar_mutex(&(entrenador -> mutex_redes));
  estado -> redes_generadas = entrenador -> redes_generadas;
  estado -> redes_descartadas = entrenador -> redes_descartadas;
  for (red = 0; red < entrenador -> cantidad_redes; red ++)
    if ((red[entrenador -> redes_entrenamiento] -> estado == 2) || (red[entrenador -> redes_entrenamiento] -> cantidad_OK > 0.9999)) {
      estado -> redes_satisfactorias ++;
      if (red[entrenador -> redes_entrenamiento] -> puntaje > mejor)
        mejor = red[entrenador -> redes_entrenamiento] -> puntaje;
      suma += red[entrenador -> redes_entrenamiento] -> puntaje;
    }
  ReleaseMutex(entrenador -> mutex_redes);
  if (estado -> redes_satisfactorias) estado -> puntaje_promedio = (unsigned) (suma * 1e6 / estado -> redes_satisfactorias);
  estado -> mejor_puntaje = (unsigned) (mejor * 1e6);
  return AVS_OK;
}
