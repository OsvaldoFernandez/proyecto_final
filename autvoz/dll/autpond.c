#include "proto.h"

int ponderar_resultados (const double * resultados, const double * pesos, unsigned cantidad) {
  double coef[5];
  *coef = obtener_coeficiente_p0(resultados, pesos, cantidad);
  obtener_coeficientes_estadisticos(resultados, pesos, cantidad, coef);
  double resultado = calcular_ponderacion_final(coef);
  resultado = round(resultado * 10000.0);
  if (resultado > 10000.0)
    return 10000;
  else if (resultado < -10000.0)
    return -10000;
  return resultado;
}

double obtener_coeficiente_p0 (const double * resultados, const double * pesos, unsigned cantidad) {
  unsigned i;
  double suma_pesos = 0;
  for (i = 0; i < cantidad; i ++) suma_pesos += pesos[i];
  double ajuste_pesos = 2.0 / suma_pesos + 1.0;
  double numerador = 0, suma_ponderada = 0;
  double resultado_ajustado;
  for (i = 0; i < cantidad; i ++) {
    resultado_ajustado = fabs(resultados[i] * 2.0 - 1.0);
    numerador += resultados[i] * pesos[i] * (ajuste_pesos - resultado_ajustado);
    suma_ponderada += pesos[i] * resultado_ajustado;
  }
  return numerador / (2.0 + suma_pesos - suma_ponderada);
}

void obtener_coeficientes_estadisticos (const double * resultados, const double * pesos, unsigned cantidad, double * coeficientes) {
  double suma_pesos = 0;
  unsigned i;
  for (i = 0; i < cantidad; i ++) suma_pesos += pesos[i];
  double suma = 0;
  for (i = 0; i < cantidad; i ++) suma += resultados[i] * pesos[i];
  coeficientes[1] = suma / suma_pesos;
  double v, v3;
  suma = 0;
  for (i = 0; i < cantidad; i ++) {
    v = resultados[i] - coeficientes[1];
    suma += v * v * pesos[i];
  }
  coeficientes[2] = suma / suma_pesos;
  if (!coeficientes[2]) return;
  suma = 0;
  double suma4 = 0;
  for (i = 0; i < cantidad; i ++) {
    v = resultados[i] - coeficientes[1];
    v3 = v * v * v * pesos[i];
    suma += v3;
    suma4 += v3 * v;
  }
  v = coeficientes[2] * suma_pesos;
  coeficientes[3] = suma / (v * sqrt(coeficientes[2]));
  coeficientes[4] = suma4 / (coeficientes[2] * v);
}

double calcular_ponderacion_final (const double * p) {
  if (fabs(p[2]) < 1e-14) return p[0] * 2.0 - 1.0;
  double base = 2.0 * (p[0] + p[2] * p[3]) - 1.0;
  return base * (1.0 - p[2] * (base * base - 1.0) / pow(p[4], fabs(p[0] - p[1])));
}
