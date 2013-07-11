using System;
using System.Linq;

public class Disposable : IDisposable
{
    private bool disposed;

    public bool Disposed { get { return disposed; } }

    public void Dispose()
    {
        disposed = true;
    }

    public void DoSomething()
    {
    }
}