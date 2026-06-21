# Sable

*Test firmware without hardware.*

Embedded development is gated by hardware: write firmware, flash a board, find a bug, reflash,
repeat. Sable lets you run that firmware on your computer instead.

You write firmware against a generic hardware interface (`sable.h`) — GPIO, ADC, UART, SPI,
I2C, timers. The same firmware can run two ways:

- **On a real MCU**, through a hardware backend you provide.
- **In simulation**, against Sable's virtual MCU.

In simulation you attach virtual chips to the buses, drive inputs, run your firmware, and get
a waveform (VCD) of every pin and bus transaction.

Sable proves your logic and protocols are correct. It is not a cycle-accurate emulator; the
last mile of electrical and timing validation still happens on real hardware.
