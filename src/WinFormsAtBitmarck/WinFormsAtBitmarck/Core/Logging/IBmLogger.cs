namespace WinFormsAtBitmarck.Core.Logging;

public interface IBmLogger
{
    // define logging functions
    void LogDebug(string message);
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception ex);
}