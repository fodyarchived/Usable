using System.Linq;
using ApprovalTests.Reporters;
using NUnit.Framework;

[TestFixture]
[UseReporter(typeof(DiffReporter))]
public class LoggingTests
{
    [Test]
    public void InfoMessages()
    {
        ApprovalTests.Approvals.VerifyAll(AssemblyWeaver.Infos.OrderBy(s => s), "Info");
    }

    [Test]
    public void WarningMessages()
    {
        ApprovalTests.Approvals.VerifyAll(AssemblyWeaver.Warnings.OrderBy(s => s), "Warning");
    }

    [Test]
    public void ErrorMessages()
    {
        ApprovalTests.Approvals.VerifyAll(AssemblyWeaver.Errors.OrderBy(s => s), "Error");
    }
}