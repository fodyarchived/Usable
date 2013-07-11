using System;
using System.Collections.Generic;
using System.Linq;
using Anotar.Custom;
using ICSharpCode.Decompiler.ILAst;
using Mono.Cecil;

public class UsableVisitor : ILNodeVisitor
{
    private readonly MethodDefinition method;
    private readonly Dictionary<Tuple<ILVariable, int>, int> starts;
    private readonly List<int> currentTrys;
    private int currentScope;

    public UsableVisitor(MethodDefinition method)
    {
        this.method = method;
        UsingRanges = new List<ILRange>();
        EarlyReturns = new List<int>();
        starts = new Dictionary<Tuple<ILVariable, int>, int>();
        currentTrys = method.Body.ExceptionHandlers.Select(handler => handler.TryStart.Offset).ToList();
    }

    public List<ILRange> UsingRanges { get; private set; }
    public List<int> EarlyReturns { get; private set; }

    protected override ILExpression VisitExpression(ILExpression expression)
    {
        if (expression.Code == ILCode.Stloc)
        {
            var variable = (ILVariable)expression.Operand;

            var key = Tuple.Create(variable, currentScope);

            if (starts.Keys.Any(k => k.Item1 == variable && k.Item2 != currentScope))
            {
                Log.Warning("Method {0}: Using cannot be added because reassigning a variable in a condition is not supported.", method);
            }
            else
            {
                if (starts.ContainsKey(key))
                {
                    UsingRanges.Add(new ILRange { From = starts[key], To = expression.FirstILOffset() });
                    starts.Remove(key);
                }

                if (!currentTrys.Contains(expression.LastILOffset()))
                    starts.Add(key, expression.LastILOffset());
            }
        }
        if (expression.Code == ILCode.Ret && currentScope > 1)
        {
            EarlyReturns.Add(expression.ILRanges.First().From);
        }

        return base.VisitExpression(expression);
    }

    protected override ILBlock VisitBlock(ILBlock block)
    {
        currentScope++;

        var result = base.VisitBlock(block);

        currentScope--;

        if (block.Body.Count == 0)
            return result;

        var toOffset = block.LastILOffset();

        foreach (var start in starts.Where(kvp => kvp.Key.Item2 == currentScope + 1).ToList())
        {
            UsingRanges.Add(new ILRange { From = start.Value, To = toOffset });
            starts.Remove(start.Key);
        }

        return result;
    }
}