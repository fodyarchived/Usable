using System;

public class NotHandled
{
    public Disposable ReturnLocalDisposable()
    {
        var instance = new Disposable();
        instance.DoSomething();
        return instance;
    }
}