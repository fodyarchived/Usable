using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public static class CecilExtensions
{
    public static IEnumerable<MethodDefinition> MethodsWithBody(this TypeDefinition type)
    {
        return type.Methods.Where(x => x.Body != null);
    }

    public static IEnumerable<PropertyDefinition> ConcreteProperties(this TypeDefinition type)
    {
        return type.Properties.Where(x => (x.GetMethod == null || !x.GetMethod.IsAbstract) && (x.SetMethod == null || !x.SetMethod.IsAbstract));
    }

    public static bool HasInterface(this TypeDefinition type, string interfaceFullName)
    {
        return (type.Interfaces.Any(i => i.FullName.Equals(interfaceFullName))
                || (type.BaseType != null && type.BaseType.Resolve().HasInterface(interfaceFullName)));
    }

    public static void InsertBefore(this ILProcessor processor, Instruction target, params Instruction[] instructions)
    {
        foreach (var instruction in instructions)
            processor.InsertBefore(target, instruction);
    }

    public static void InsertAfter(this ILProcessor processor, Instruction target, params Instruction[] instructions)
    {
        foreach (var instruction in instructions.Reverse())
            processor.InsertAfter(target, instruction);
    }

    #region GetPushDelta / GetPopDelta
    public static int GetPushDelta(this Instruction instruction)
    {
        OpCode code = instruction.OpCode;
        switch (code.StackBehaviourPush)
        {
            case StackBehaviour.Push0:
                return 0;

            case StackBehaviour.Push1:
            case StackBehaviour.Pushi:
            case StackBehaviour.Pushi8:
            case StackBehaviour.Pushr4:
            case StackBehaviour.Pushr8:
            case StackBehaviour.Pushref:
                return 1;

            case StackBehaviour.Push1_push1:
                return 2;

            case StackBehaviour.Varpush:
                if (code.FlowControl != FlowControl.Call)
                    break;

                IMethodSignature method = (IMethodSignature)instruction.Operand;
                return IsVoid(method.ReturnType) ? 0 : 1;
        }

        throw new NotSupportedException();
    }

    public static int? GetPopDelta(this Instruction instruction, MethodDefinition methodDef)
    {
        OpCode code = instruction.OpCode;
        switch (code.StackBehaviourPop)
        {
            case StackBehaviour.Pop0:
                return 0;
            case StackBehaviour.Popi:
            case StackBehaviour.Popref:
            case StackBehaviour.Pop1:
                return 1;

            case StackBehaviour.Pop1_pop1:
            case StackBehaviour.Popi_pop1:
            case StackBehaviour.Popi_popi:
            case StackBehaviour.Popi_popi8:
            case StackBehaviour.Popi_popr4:
            case StackBehaviour.Popi_popr8:
            case StackBehaviour.Popref_pop1:
            case StackBehaviour.Popref_popi:
                return 2;

            case StackBehaviour.Popi_popi_popi:
            case StackBehaviour.Popref_popi_popi:
            case StackBehaviour.Popref_popi_popi8:
            case StackBehaviour.Popref_popi_popr4:
            case StackBehaviour.Popref_popi_popr8:
            case StackBehaviour.Popref_popi_popref:
                return 3;

            case StackBehaviour.PopAll:
                return null;

            case StackBehaviour.Varpop:
                if (code == OpCodes.Ret)
                    return methodDef.ReturnType.IsVoid() ? 0 : 1;

                if (code.FlowControl != FlowControl.Call)
                    break;

                IMethodSignature method = (IMethodSignature)instruction.Operand;
                int count = method.HasParameters ? method.Parameters.Count : 0;
                if (method.HasThis && code != OpCodes.Newobj)
                    ++count;
                if (code == OpCodes.Calli)
                    ++count; // calli takes a function pointer in additional to the normal args

                return count;
        }

        throw new NotSupportedException();
    }

    public static bool IsVoid(this TypeReference type)
    {
        while (type is OptionalModifierType || type is RequiredModifierType)
            type = ((TypeSpecification)type).ElementType;
        return type.MetadataType == MetadataType.Void;
    }

    public static bool IsValueTypeOrVoid(this TypeReference type)
    {
        while (type is OptionalModifierType || type is RequiredModifierType)
            type = ((TypeSpecification)type).ElementType;
        if (type is ArrayType)
            return false;
        return type.IsValueType || type.IsVoid();
    }
    #endregion

    /// <summary>
    /// Gets the (exclusive) end offset of this instruction.
    /// </summary>
    public static int GetEndOffset(this Instruction inst)
    {
        if (inst == null)
            throw new ArgumentNullException("inst");
        return inst.Offset + inst.GetSize();
    }

    public static string OffsetToString(int offset)
    {
        return string.Format("IL_{0:x4}", offset);
    }
}