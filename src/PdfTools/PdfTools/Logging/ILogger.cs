namespace PdfTools.Logging
{
    public interface ILogger
    {
        void Trace(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Trace(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}