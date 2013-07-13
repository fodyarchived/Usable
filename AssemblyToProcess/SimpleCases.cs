using System.IO;

public class SimpleCases
{
    public void SingleDisposable()
    {
        var w = File.CreateText("log.txt");
        w.WriteLine("I'm a lumberjack an' I'm ok.");
    }

    public void AlreadyUsing()
    {
        using (var w = File.CreateText("log.txt"))
        {
            w.WriteLine("I'm a lumberjack an' I'm ok.");
        }
    }

    public void VariableReuse()
    {
        var w = File.CreateText("log.txt");
        w.WriteLine("I'm a lumberjack an' I'm ok.");
        w = File.CreateText("log2.txt");
        w.WriteLine("I'm a lumberjack an' I'm ok.");
    }

    public void NestedUsings()
    {
        var w = File.CreateText("log.txt");
        var w2 = File.CreateText("log2.txt");
        w.WriteLine("I'm a lumberjack an' I'm ok.");
        w2.WriteLine("He's a lumberjack an' He's ok.");
    }

    public void MultipleUsings()
    {
        var w = File.CreateText("log.txt");
        w.WriteLine("I'm a lumberjack an' I'm ok.");
        var w2 = File.CreateText("log2.txt");
        w2.WriteLine("He's a lumberjack an' He's ok.");
    }

    public static void NothingAfterAssignment()
    {
        var w = File.CreateText("log.txt");
    }

    public static void ThrowInsteadOfReturn()
    {
        var w = File.CreateText("log.txt");
        throw new System.NotImplementedException();
    }

    // This adds methods
    public event System.EventHandler SomeEvent;
}