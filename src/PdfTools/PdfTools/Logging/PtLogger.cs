using System.IO;

namespace PdfTools.Logging
{
    public class PtFileLogger : ILogger
    {
        public PtFileLogger()
        {
        }

        public void Trace(string message)
        {
            var path = Path.Combine(Path.GetTempPath(), "PdfTools.log");
            File.AppendAllLines(path, new[] { message });
        }
    }
}