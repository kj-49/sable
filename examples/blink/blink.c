#include "sable.h"

#define LED 13

static int on;

void setup(void) { on = 0; }

void loop(void) {
    on = !on;
    gpio_set(LED, on);
}
