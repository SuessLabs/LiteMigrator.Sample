using System;

namespace LiteMigrator.Sample.Client.Services
{
  public enum LogLevel
  {
    Debug,
    Info,
    Warn,
    Error,
    Fatal
  }

  public class LogService : ILogService
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
    private static string FormattedTime
    {
      get
      {
        string date = $"{DateTime.Now.ToString("yyyy-MM-dd")}_";
        return date + $"{DateTime.Now.Hour:00}:{DateTime.Now.Minute:00}:{DateTime.Now.Second:00}.{DateTime.Now.Millisecond:000}";
      }
    }

    public void Debug(string message, params object[] args)
    {
      LogMessage(LogLevel.Debug, message);
    }

    public void Error(string message, params object[] args)
    {
      LogMessage(LogLevel.Error, message);
    }

    public void Fatal(string message, params object[] args)
    {
      LogMessage(LogLevel.Fatal, message);
    }

    public void Info(string message, params object[] args)
    {
      LogMessage(LogLevel.Info, message);
    }

    public void Warn(string message, params object[] args)
    {
      LogMessage(LogLevel.Warn, message);
    }

    private void LogMessage(LogLevel level, string message)
    {
      string cls = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().ReflectedType.Name;
      string method = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().Name;
      string text = $"[{FormattedTime}] [{level.ToString()}] [{cls}.{method}] [{message}]";

      System.Diagnostics.Debug.WriteLine(">> " + text);
    }
  }
}
