using System;

[assembly: Anotar.Custom.LogMinimalMessage]

public static class LoggerFactory
{
    public static Action<string> LogInfo { get; set; }
    public static Action<string> LogWarn { get; set; }
    public static Action<string> LogError { get; set; }

    public static Logger GetLogger<T>()
    {
        return new Logger(LogInfo, LogWarn, LogError);
    }
}

public class Logger
{
    private readonly Action<string> logInfo;
    private readonly Action<string> logWarn;
    private readonly Action<string> logError;

    public Logger(Action<string> logInfo, Action<string> logWarn, Action<string> logError)
    {
        this.logInfo = logInfo;
        this.logWarn = logWarn;
        this.logError = logError;
    }

    public void Debug(string format, params object[] args)
    {
        throw new NotSupportedException();
    }

    public void Debug(Exception exception, string format, params object[] args)
    {
        throw new NotSupportedException();
    }

    public bool IsDebugEnabled { get { return false; } }

    public void Information(string format, params object[] args)
    {
        logInfo(string.Format(format, args));
    }

    public void Information(Exception exception, string format, params object[] args)
    {
        logInfo(string.Format(format, args) + Environment.NewLine + exception);
    }

    public bool IsInformationEnabled { get { return logInfo != null; } }

    public void Warning(string format, params object[] args)
    {
        logWarn(string.Format(format, args));
    }

    public void Warning(Exception exception, string format, params object[] args)
    {
        logWarn(string.Format(format, args) + Environment.NewLine + exception);
    }

    public bool IsWarningEnabled { get { return logWarn != null; } }

    public void Error(string format, params object[] args)
    {
        logError(string.Format(format, args));
    }

    public void Error(Exception exception, string format, params object[] args)
    {
        logError(string.Format(format, args) + Environment.NewLine + exception);
    }

    public bool IsErrorEnabled { get { return logError != null; } }

    public void Fatal(string format, params object[] args)
    {
        throw new NotSupportedException();
    }

    public void Fatal(Exception exception, string format, params object[] args)
    {
        throw new NotSupportedException();
    }

    public bool IsFatalEnabled { get { return false; } }
}