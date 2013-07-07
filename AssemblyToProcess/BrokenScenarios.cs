using System;
using System.IO;

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

    //public static void ThrowInsteadOfReturn()
    //{
    //    var w = File.CreateText("log.txt");
    //    throw new System.NotImplementedException();
    //}
}