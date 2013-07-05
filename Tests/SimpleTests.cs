using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

[TestFixture]
[UseReporter(typeof(DiffReporter))]
public class SimpleTests
{
    [Test]
    public void SingleDisposable()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::SingleDisposable"));
    }

    [Test]
    public void AlreadyUsing()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::AlreadyUsing"));
    }

    [Test]
    public void VariableReuse()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::VariableReuse"));
    }

    [Test]
    public void NestedUsings()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::NestedUsings"));
    }

    [Test]
    public void MultipleUsings()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::MultipleUsings"));
    }
}