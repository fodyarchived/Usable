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
        ApprovalTests.Namers.NamerFactory.AsMachineSpecificTest(() => "Debug");
#else
        ApprovalTests.Namers.NamerFactory.AsMachineSpecificTest(() => "Release");
#endif
    }

    [Test]
    public void Conditional()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "MultipleReturns::Conditional"));
    }

    [Test]
    public void EarlyReturn()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "MultipleReturns::EarlyReturn"));
    }
}