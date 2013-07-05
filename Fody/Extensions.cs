using System;
using System.Collections.Generic;

public static class Extensions
{
    public static T OnlyOrDefault<T>(this IEnumerable<T> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        var list = source as IList<T>;
        if (list != null)
        {
            if (list.Count == 1)
            {
                return list[0];
            }
        }
        else
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    var only = enumerator.Current;
                    if (!enumerator.MoveNext())
                        return only;
                }
            }
        }
        return default(T);
    }
}