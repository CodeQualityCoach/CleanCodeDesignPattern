namespace LazyFunc;

internal class ConsoleOut : IOut
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}