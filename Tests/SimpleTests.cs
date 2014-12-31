using System.Runtime.CompilerServices;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

[TestFixture]
[UseReporter(typeof(DiffReporter))]
public class SimpleTests
{
    [SetUp]
    public void SetApprovalConfig()
    {
#if DEBUG
        ApprovalTests.Namers.NamerFactory.AsEnvironmentSpecificTest(() => "Debug");
#else
        ApprovalTests.Namers.NamerFactory.AsEnvironmentSpecificTest(() => "Release");
#endif
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void SingleDisposable()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::SingleDisposable"));
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void AlreadyUsing()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::AlreadyUsing"));
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void VariableReuse()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::VariableReuse"));
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void NestedUsings()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::NestedUsings"));
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void MultipleUsings()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::MultipleUsings"));
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void add_SomeEvent()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::add_SomeEvent"));
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void remove_SomeEvent()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::remove_SomeEvent"));
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Issue8()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::Issue8"));
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void GenericDisposable()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::GenericDisposable"));
    }

#if DEBUG

    [Test]
    public void NothingAfterAssignment()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::NothingAfterAssignment"));
    }

    [Test]
    public void ThrowInsteadOfReturn()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::ThrowInsteadOfReturn"));
    }

#endif
}