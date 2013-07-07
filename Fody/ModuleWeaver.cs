using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.ILAst;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

public class ModuleWeaver
{
    public Action<string> LogInfo { get; set; }
    public Action<string> LogError { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public IAssemblyResolver AssemblyResolver { get; set; }
    public string[] DefineConstants { get; set; }

    public ModuleWeaver()
    {
        LogInfo = s => { };
        LogError = s => { };
        DefineConstants = new string[0];
    }

    public void Execute()
    {
        var types = ModuleDefinition.GetTypes()
            .ToList();

        foreach (var type in types)
        {
            ProcessType(type);
        }
    }

    private void ProcessType(TypeDefinition type)
    {
        foreach (var method in type.MethodsWithBody())
            ProcessBody(method);

        foreach (var property in type.ConcreteProperties())
        {
            if (property.GetMethod != null)
                ProcessBody(property.GetMethod);
            if (property.SetMethod != null)
                ProcessBody(property.SetMethod);
        }
    }

    private void ProcessBody(MethodDefinition method)
    {
        method.Body.SimplifyMacros();

        var ilProcessor = method.Body.GetILProcessor();

        var ilMethod = Decompile(method);

        var visitor = new UsableVisitor(method);
        visitor.Visit(ilMethod);

        if (visitor.EarlyReturns.Any() && method.Body.Instructions.Last().OpCode == OpCodes.Ret)
            FixEarlyReturns(method.Body, visitor.EarlyReturns);

        var usingBlocks = FixUsingBlocks(ilProcessor,
            visitor.UsingRanges.Select(r => Tuple.Create(method.Body.Instructions.AtOffset(r.From), method.Body.Instructions.AtOffset(r.To))));

        foreach (var usingBlock in usingBlocks.OrderBy(u => u.Item1.Offset))
            AddUsing(method.Body, usingBlock);

        method.Body.OptimizeMacros();

        method.Body.ExceptionHandlers.ReplaceCollection(method.Body.ExceptionHandlers.OrderBy(x => x, new ExceptionHandlerComparer()));
    }

    private static ILBlock Decompile(MethodDefinition method)
    {
        bool inlineVariables = false;

        ILAstBuilder astBuilder = new ILAstBuilder();
        ILBlock ilMethod = new ILBlock();
        DecompilerContext context = new DecompilerContext(method.Module) { CurrentType = method.DeclaringType, CurrentMethod = method };
        ilMethod.Body = astBuilder.Build(method, inlineVariables, context);

        new ILAstOptimizer().Optimize(context, ilMethod, ILAstOptimizationStep.None);
        return ilMethod;
    }

    private void FixEarlyReturns(MethodBody body, List<int> earlyReturns)
    {
        var il = body.GetILProcessor();
        var lastReturn = body.Instructions.Last();
        foreach (var instruction in earlyReturns.Select(offset => body.Instructions.AtOffset(offset)))
        {
            il.Replace(instruction, il.Create(OpCodes.Br, lastReturn));
        }
    }

    private IEnumerable<Tuple<Instruction, Instruction>> FixUsingBlocks(ILProcessor ilProcessor, IEnumerable<Tuple<Instruction, Instruction>> usingBlocks)
    {
        foreach (var groupedEndings in usingBlocks.GroupBy(u => u.Item2))
        {
            Instruction previous = null;
            foreach (var usingBlock in groupedEndings.OrderBy(u => u.Item1.Offset))
            {
                if (previous == null)
                    previous = usingBlock.Item2;
                var nop = ilProcessor.Create(OpCodes.Nop);
                ilProcessor.InsertBefore(previous, nop);
                previous = nop;

                yield return Tuple.Create(usingBlock.Item1, previous);
            }
        }
    }

    private void AddUsing(MethodBody methodBody, Tuple<Instruction, Instruction> usingBlock)
    {
        var stloc = usingBlock.Item1.Previous;
        var variable = (VariableDefinition)stloc.Operand;

        if (!variable.VariableType.Resolve().HasInterface("System.IDisposable"))
            return;

        var il = methodBody.GetILProcessor();

        var disposeCall = il.Create(OpCodes.Callvirt, ModuleDefinition.Import(typeof(IDisposable).GetMethod("Dispose")));

        methodBody.OptimizeMacros();
        methodBody.SimplifyMacros();

        if (usingBlock.Item2.Previous.OpCode == OpCodes.Br)
        {
            var leave = il.Create(OpCodes.Leave, (Instruction)usingBlock.Item2.Previous.Operand);
            il.Replace(usingBlock.Item2.Previous, leave);
        }
        else
        {
            var leave = il.Create(OpCodes.Leave, usingBlock.Item2);
            il.InsertBefore(usingBlock.Item2, leave);
        }

        var firstPartOfFinally = il.Create(OpCodes.Ldloc, variable);
        var endFinally = il.Create(OpCodes.Endfinally);

        il.InsertBefore(usingBlock.Item2,
            firstPartOfFinally,
            il.Create(OpCodes.Brfalse, endFinally),
            il.Create(OpCodes.Ldloc, variable),
            disposeCall,
            endFinally
        );

        var handler = new ExceptionHandler(ExceptionHandlerType.Finally)
        {
            TryStart = usingBlock.Item1,
            TryEnd = firstPartOfFinally,
            HandlerStart = firstPartOfFinally,
            HandlerEnd = usingBlock.Item2,
        };

        ReplaceBranchesWithLeaves(il, handler);

        methodBody.ExceptionHandlers.Add(handler);
    }

    private void ReplaceBranchesWithLeaves(ILProcessor il, ExceptionHandler handler)
    {
        var current = handler.TryStart;
        while (current != handler.TryEnd)
        {
            var next = current.Next;
            if (current.OpCode == OpCodes.Br)
                il.Replace(current, il.Create(OpCodes.Leave, (Instruction)current.Operand));
            current = next;
        }
    }
}