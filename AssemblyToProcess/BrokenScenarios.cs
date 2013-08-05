using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class BrokenScenarios
{
    // Must throw error as this scenario cannot be handled
    public void ReassignmentInsideConditional(bool x)
    {
        var w = File.CreateText("log.txt");
        if (x)
            w = File.CreateText("log2.txt");
        w.WriteLine("I'm a lumberjack an' I'm ok.");
    }

    // Reproduction of issue #7
    public async Task Issue7()
    {
        var file = await GetFileAsync();
        using (var stream = await OpenFileAsync(file))
        {
        }
    }

    private static Task<FileInfo> GetFileAsync()
    {
        return Task.FromResult(new FileInfo("log.txt"));
    }

    private static Task<FileStream> OpenFileAsync(FileInfo file)
    {
        return Task.FromResult(file.OpenRead());
    }
}