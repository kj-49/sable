using System.Runtime.InteropServices;

namespace Sable;

public sealed class SimMcu : IDisposable
{
    [StructLayout(LayoutKind.Sequential)]
    private struct SableHal
    {
        public IntPtr gpio_set;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void BindDelegate(IntPtr hal);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void VoidDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GpioSetDelegate(int pin, int level);

    private IntPtr _lib;
    private IntPtr _halPtr;
    private readonly List<object> _roots = []; // keep delegates alive
    private VoidDelegate? _setup;
    private VoidDelegate? _loop;

    public GpioController Gpio { get; } = new();

    public void Load(string path)
    {
        if (_lib != IntPtr.Zero) throw new InvalidOperationException("Already loaded.");

        _lib = NativeLibrary.Load(path);

        var bind = GetExport<BindDelegate>("sable_bind");
        _setup   = GetExport<VoidDelegate>("setup");
        _loop    = GetExport<VoidDelegate>("loop");

        GpioSetDelegate gpioSet = (pin, level) => Gpio.Set(pin, level);
        _roots.Add(gpioSet);

        var hal = new SableHal { gpio_set = Marshal.GetFunctionPointerForDelegate(gpioSet) };
        _halPtr = Marshal.AllocHGlobal(Marshal.SizeOf<SableHal>());
        Marshal.StructureToPtr(hal, _halPtr, false);
        bind(_halPtr);
    }

    public void Setup()
    {
        AssertLoaded();
        _setup!();
    }

    public void Step()
    {
        AssertLoaded();
        _loop!();
    }

    public void Run(int steps)
    {
        Setup();
        for (var i = 0; i < steps; i++) Step();
    }

    private T GetExport<T>(string name) where T : Delegate =>
        Marshal.GetDelegateForFunctionPointer<T>(NativeLibrary.GetExport(_lib, name));

    private void AssertLoaded()
    {
        if (_lib == IntPtr.Zero) throw new InvalidOperationException("Call Load() first.");
    }

    public void Dispose()
    {
        if (_lib != IntPtr.Zero) { NativeLibrary.Free(_lib); _lib = IntPtr.Zero; }
        if (_halPtr != IntPtr.Zero) { Marshal.FreeHGlobal(_halPtr); _halPtr = IntPtr.Zero; }
    }
}
