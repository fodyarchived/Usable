using System.Runtime.CompilerServices;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

[TestFixture]
[UseReporter(typeof(DiffReporter))]
public class MultipleReturns
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
    public void Conditional()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "MultipleReturns::Conditional"));
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void EarlyReturn()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "MultipleReturns::EarlyReturn"));
    }
}