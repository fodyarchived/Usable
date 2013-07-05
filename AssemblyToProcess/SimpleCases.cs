using System.IO;

public class SimpleCases
{
    public void TextWriter()
    {
        var w = File.CreateText("log.txt");
        w.WriteLine("I'm a lumberjack an' I'm ok.");
    }

    public void TextWriterExample()
    {
        using (var w = File.CreateText("log.txt"))
        {
            w.WriteLine("I'm a lumberjack an' I'm ok.");
        }
    }
}