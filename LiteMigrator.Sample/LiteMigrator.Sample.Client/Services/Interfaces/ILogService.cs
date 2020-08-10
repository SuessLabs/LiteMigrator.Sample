namespace LiteMigrator.Sample.Client.Services
{
  public interface ILogService
  {
    void Debug(string message, params object[] args);

#pragma warning disable CA1716 // Identifiers should not match keywords

    void Error(string message, params object[] args);

#pragma warning restore CA1716 // Identifiers should not match keywords

    void Fatal(string message, params object[] args);

    void Info(string message, params object[] args);

    //// void Log(LogLevel lvl, string msg, params object[] args);

    void Warn(string message, params object[] args);
  }
}