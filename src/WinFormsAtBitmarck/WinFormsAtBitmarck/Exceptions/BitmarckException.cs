namespace WinFormsAtBitmarck.Exceptions;

/// <summary>
/// Application exception to check in try-catch if it is app rethrown exception (already caught)
/// </summary>
internal class BitmarckException : Exception
{
    public BitmarckException(string message) : base(message) { }
}