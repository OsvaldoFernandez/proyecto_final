#include "proto.h"

RedEntrenamiento * crear_red_nueva (Entrenador * entrenador) {
  RedEntrenamiento * red = calloc(1, sizeof(RedEntrenamiento));
  red -> puntaje = red -> cantidad_OK = 0;
  red -> iteraciones = red -> estado = 0;
  struct parametros_red parametros = generar_parametros_red(entrenador -> generador_aleatorio, entrenador -> personas, entrenador -> cantidad_personas);
  red -> alfa_0 = parametros.a0;
  red -> alfa_d = parametros.ad;
  red -> d = parametros.d;
  red -> plan = parametros.plan;
  red -> excluir = parametros.excluir;
  unsigned nodos[8];
  cantidades_nodos(parametros.z, (parametros.plan & 0xfff) << 2, parametros.b1, parametros.b2, nodos);
  // estos dos pasos pueden ser optimizados en uno reescribiendo las funciones de rna.c, pero esto afecta en menos de un 0,1% el tiempo de entrenamiento
  red -> red = crear_red_neuronal(parametros.z, nodos + 1, *nodos, entrenador -> generador_aleatorio);
  reinicializar_red_neuronal(red -> red, parametros.L, entrenador -> generador_aleatorio);
  entrenador -> redes_generadas ++;
  return red;
}

void destruir_red_entrenamiento (RedEntrenamiento * red) {
  destruir_red_neuronal(red -> red);
  free(red);
}

struct parametros_red generar_parametros_red (RNG * generador, const int * personas, unsigned cantidad_personas) {
  struct parametros_red resultado;
  resultado.z = entero_aleatorio(generador, 8);
  if (resultado.z < 3) resultado.z += 4;
  resultado.d = 4000 + entero_aleatorio(generador, 26000);
  resultado.a0 = 0.6 + 0.3 * aleatorio(generador);
  resultado.ad = 0.3 + aleatorio(generador) * 0.25;
  resultado.b2 = aleatorio(generador);
  resultado.b1 = aleatorio(generador) * 2.25 - 1.0;
  resultado.L = aleatorio(generador) * 0.45 + 0.05;
  resultado.plan = generar_plan_red(generador);
  if ((cantidad_personas >= 2) && (aleatorio(generador) < calcular_probabilidad_exclusion(cantidad_personas)))
    resultado.excluir = personas[entero_aleatorio(generador, cantidad_personas)];
  else
    resultado.excluir = 0;
  return resultado;
}

double calcular_probabilidad_exclusion (unsigned cantidad_personas) {
  if (cantidad_personas < 2) return 0;
  if (cantidad_personas >= CANTIDAD_EXCLUSION_MAXIMA) return PROBABILIDAD_EXCLUSION_MAXIMA;
  double x = (cantidad_personas - 2);
  x /= CANTIDAD_EXCLUSION_MAXIMA - 2;
  return PROBABILIDAD_EXCLUSION_MINIMA + x * (PROBABILIDAD_EXCLUSION_MAXIMA - PROBABILIDAD_EXCLUSION_MINIMA);
}

void cantidades_nodos (unsigned niveles, unsigned entradas, double b1, double b2, unsigned * resultado) {
  *resultado = entradas;
  unsigned nivel;
  for (nivel = 1; nivel < niveles; nivel ++) resultado[nivel] = nodos_por_nivel(niveles, nivel, entradas, b1, b2);
}

unsigned nodos_por_nivel (unsigned niveles, unsigned nivel, unsigned entradas, double b1, double b2) {
  return 0.5 + b2 * pow(entradas, 1.0 - (double) nivel / (double) niveles) + (1.0 - b2) * (
    (double) entradas + 
    (((double) entradas - 1.0) / ((double) niveles * (double) niveles)) * ((double) (nivel * niveles) * (b1 - 1.0) - b1 * (double) (nivel * nivel))
  );
}

unsigned generar_plan_red (RNG * generador) {
  double rv = aleatorio(generador);
  unsigned plan;
  if (rv < PROBABILIDAD_PLAN_20)
    plan = 20;
  else if (rv < (PROBABILIDAD_PLAN_20 + PROBABILIDAD_PLAN_30))
    plan = 30;
  else
    plan = 40;
  rv = aleatorio(generador);
  if (rv < PROBABILIDAD_METODO_1)
    plan |= 0x1000;
  else if (rv < (PROBABILIDAD_METODO_1 + PROBABILIDAD_METODO_2))
    plan |= 0x2000;
  return plan;
}
