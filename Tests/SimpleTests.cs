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

    [Test]
    public void add_SomeEvent()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::add_SomeEvent"));
    }

    [Test]
    public void remove_SomeEvent()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::remove_SomeEvent"));
    }

    [Test]
    public void Issue8()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::Issue8"));
    }

    [Test]
    public void Issue8WithDisposable()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "SimpleCases::Issue8WithDisposable"));
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