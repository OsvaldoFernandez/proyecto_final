#include "proto.h"

RNG * crear_RNG (unsigned valor_inicial) {
  RNG * resultado = malloc(sizeof(RNG));
  if (!resultado) return NULL;
  resultado -> posicion = 0;
  *(resultado -> estado) = valor_inicial;
  unsigned p;
  for (p = 1; p <= 623; p ++) p[resultado -> estado] = 0x6c078965 * (resultado -> estado[p - 1] ^ (resultado -> estado[p - 1] >> 30)) + p;
  return resultado;
}

void destruir_RNG (RNG * rng) {
  free(rng);
}

unsigned entero_aleatorio (RNG * rng, unsigned limite) {
  unsigned valor;
  if (limite > 1) {
    unsigned maximo = 0x100000000ULL / limite;
    do
      valor = entero_aleatorio(rng, 0);
    while ((valor / limite) >= maximo);
    return valor % limite;
  } else if (limite == 1)
    return 0;
  if (!rng) return 0;
  if (!(rng -> posicion)) {
    unsigned p;
    for (p = 0; p <= 623; p ++) {
      valor = (p[rng -> estado] & 0x80000000U) | (rng -> estado[(p != 623) ? p + 1 : 0] & 0x7fffffffU);
      p[rng -> estado] = rng -> estado[(p >= 227) ? (p - 227) : (p + 397)] ^ (valor >> 1) ^ ((valor & 1) ? 0x9908b0dfU : 0);
    }
  }
  valor = rng -> estado[rng -> posicion ++];
  if (rng -> posicion == 624) rng -> posicion = 0;
  valor ^= valor >> 11;
  valor ^= (valor << 7) & 0x9d2c5680U;
  valor ^= (valor << 15) & 0xefc60000U;
  valor ^= valor >> 18;
  return valor;
}

double aleatorio (RNG * rng) {
  return ldexp(entero_aleatorio(rng, 0) >> 4, -28) + ldexp(entero_aleatorio(rng, 0) >> 7, -53);
}
