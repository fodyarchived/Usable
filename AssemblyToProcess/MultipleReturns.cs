using System.IO;

public class MultipleReturns
{
    // Produces multiple returns in release build
    public void Conditional(bool x)
    {
        if (x)
        {
            var w = File.CreateText("log.txt");
            w.WriteLine("I'm a lumberjack an' I'm ok.");
        }
        else
        {
            var w = File.CreateText("log2.txt");
            w.WriteLine("He's a lumberjack an' He's ok.");
        }
    }

    public void EarlyReturn()
    {
        var w = File.CreateText("log.txt");

        if (w == null)
            return;

        w.WriteLine("I'm a lumberjack an' I'm ok.");
    }

    public bool NestedEarlyReturn(bool check)
    {
        if (check == false)
        {
            if (check == false)
            {
                return true;
            }
        }

        return false;
    }
}