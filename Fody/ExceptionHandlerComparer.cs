using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;

public class ExceptionHandlerComparer : IComparer<ExceptionHandler>
{
    public int Compare(ExceptionHandler x, ExceptionHandler y)
    {
        var overlap = x.TryEnd.Offset > y.TryStart.Offset || y.TryEnd.Offset > x.TryStart.Offset;

        if (x.TryStart.Offset == y.TryStart.Offset)
        {
            if (overlap)
                return Comparer<int>.Default.Compare(x.TryEnd.Offset, y.TryEnd.Offset);
            else
                return Comparer<int>.Default.Compare(y.TryEnd.Offset, x.TryEnd.Offset);
        }
        else
        {
            if (overlap)
                return Comparer<int>.Default.Compare(y.TryStart.Offset, x.TryStart.Offset);
            else
                return Comparer<int>.Default.Compare(x.TryStart.Offset, y.TryStart.Offset);
        }
    }
}