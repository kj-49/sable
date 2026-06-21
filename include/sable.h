#ifndef SABLE_H
#define SABLE_H

/* The host-provided hardware functions. Filled in by the backend (simulator or
   real hardware) and bound before setup()/loop() run. */
typedef struct {
    void (*gpio_set)(int pin, int level);
} SableHal;

extern const SableHal *g_sable;

static inline void gpio_set(int pin, int level) { g_sable->gpio_set(pin, level); }

/* Implemented by firmware. */
void setup(void);
void loop(void);

#endif
