namespace Sable;

public class GpioController
{
    private readonly int[] _levels = new int[64];

    public int Read(int pin) => _levels[pin];

    internal void Set(int pin, int level) => _levels[pin] = level;
}
