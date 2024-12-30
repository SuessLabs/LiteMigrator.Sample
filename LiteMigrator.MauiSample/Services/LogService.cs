using System;
namespace LiteMigrator.MauiSample.Services;

public enum LogLevel
{
  Debug,
  Info,
  Warn,
  Error,
  Fatal
}

public class LogService
{
  private static string FormattedTime
  {
    get
    {
      string date = $"{DateTime.Now.ToString("yyyy-MM-dd")}_";
      return date + $"{DateTime.Now.Hour:00}:{DateTime.Now.Minute:00}:{DateTime.Now.Second:00}.{DateTime.Now.Millisecond:000}";
    }
  }

  public void Debug(string message) => LogMessage(LogLevel.Debug, message);

  public void Error(string message) => LogMessage(LogLevel.Error, message);

  public void Fatal(string message) => LogMessage(LogLevel.Fatal, message);

  public void Info(string message) => LogMessage(LogLevel.Info, message);

  public void Warn(string message) => LogMessage(LogLevel.Warn, message);

  private void LogMessage(LogLevel level, string message)
  {
    var trace = new System.Diagnostics.StackTrace().GetFrame(2);
    if (trace is null)
    {
      var txt = $"[{FormattedTime}] [{level}] [?] [{message}]";
      System.Diagnostics.Debug.WriteLine(">> " + txt);
      return;
    }

    string cls = trace.GetMethod().ReflectedType.Name;
    string method = trace.GetMethod().Name;
    string text = $"[{FormattedTime}] [{level}] [{cls}.{method}] [{message}]";

    System.Diagnostics.Debug.WriteLine(">> " + text);
  }
}
