namespace PdfTools.Logging
{
    public class ConsoleLogger : IPtLogger
    {
        public void Trace(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}