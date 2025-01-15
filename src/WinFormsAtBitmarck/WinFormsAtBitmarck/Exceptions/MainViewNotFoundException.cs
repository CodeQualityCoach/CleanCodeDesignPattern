namespace WinFormsAtBitmarck.Exceptions;

internal class MainViewNotFoundException : BitmarckException
{
    private const string Message = "Main view konnte nicht gefunden werden. Bitte DI checken";
    public MainViewNotFoundException()
        : base(Message) { }
}