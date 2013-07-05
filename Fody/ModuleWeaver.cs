using System;
using System.Linq;
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
        var methodBody = method.Body;

        var disposables = methodBody.Variables.Where(vdef => vdef.VariableType.Resolve().HasInterface("System.IDisposable"));

        Instruction returnInstruction = null;

        if (disposables.Any())
        {
            methodBody.SimplifyMacros();
            returnInstruction = FixReturns(method);
        }

        var il = methodBody.GetILProcessor();

        foreach (var disposable in disposables)
        {
            var storeLoc = methodBody.Instructions.Where(i => i.OpCode == OpCodes.Stloc && i.Operand == disposable).OnlyOrDefault();
            if (storeLoc == null)
                continue;

            var disposeCall = il.Create(OpCodes.Callvirt, ModuleDefinition.Import(typeof(IDisposable).GetMethod("Dispose")));

            var firstPartOfFinally = il.Create(OpCodes.Ldloc, disposable);
            var endFinally = il.Create(OpCodes.Endfinally);

            il.InsertBefore(returnInstruction,
                firstPartOfFinally,
                il.Create(OpCodes.Brfalse, endFinally),
                il.Create(OpCodes.Ldloc, disposable),
                disposeCall,
                endFinally
            );

            var handler = new ExceptionHandler(ExceptionHandlerType.Finally)
            {
                TryStart = storeLoc.Next,
                TryEnd = firstPartOfFinally,
                HandlerStart = firstPartOfFinally,
                HandlerEnd = returnInstruction,
            };

            methodBody.ExceptionHandlers.Add(handler);
        }

        if (disposables.Any())
        {
            methodBody.InitLocals = true;
            methodBody.OptimizeMacros();
        }
    }

    private Instruction FixReturns(MethodDefinition method)
    {
        if (method.ReturnType == ModuleDefinition.TypeSystem.Void)
        {
            var instructions = method.Body.Instructions;
            var lastRet = Instruction.Create(OpCodes.Ret);
            instructions.Add(lastRet);

            for (var index = 0; index < instructions.Count - 1; index++)
            {
                var instruction = instructions[index];
                if (instruction.OpCode == OpCodes.Ret)
                {
                    instructions[index] = Instruction.Create(OpCodes.Leave, lastRet);
                }
            }
            return lastRet;
        }
        else
        {
            var instructions = method.Body.Instructions;
            var returnVariable = new VariableDefinition(method.ReturnType);
            method.Body.Variables.Add(returnVariable);
            var lastLd = Instruction.Create(OpCodes.Ldloc, returnVariable);
            instructions.Add(lastLd);
            instructions.Add(Instruction.Create(OpCodes.Ret));

            for (var index = 0; index < instructions.Count - 2; index++)
            {
                var instruction = instructions[index];
                if (instruction.OpCode == OpCodes.Ret)
                {
                    instructions[index] = Instruction.Create(OpCodes.Leave, lastLd);
                    instructions.Insert(index, Instruction.Create(OpCodes.Stloc, returnVariable));
                    index++;
                }
            }
            return lastLd;
        }
    }
}