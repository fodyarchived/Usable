using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

[TestFixture]
[UseReporter(typeof(DiffReporter))]
public class NotHandled
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
    public void ReturnLocalDisposable()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "NotHandled::ReturnLocalDisposable"));
    }
}