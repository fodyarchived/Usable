using System.Runtime.CompilerServices;
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
        ApprovalTests.Namers.NamerFactory.AsEnvironmentSpecificTest(() => "Debug");
#else
        ApprovalTests.Namers.NamerFactory.AsEnvironmentSpecificTest(() => "Release");
#endif
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void ReturnLocalDisposable()
    {
        Approvals.Verify(Decompiler.Decompile(AssemblyWeaver.AfterAssemblyPath, "NotHandled::ReturnLocalDisposable"));
    }
}