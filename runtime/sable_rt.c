#include "sable.h"

const SableHal *g_sable = 0;

/* Host calls this once to hand the firmware its hardware functions. */
void sable_bind(const SableHal *hal) { g_sable = hal; }
