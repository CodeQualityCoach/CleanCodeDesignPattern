using PdfTools.Logging.Contracts;

namespace PdfTools.Logging.NLog
{
    public class PtNlogLoggerFactory : IPtLoggerFactory
    {
        public IPtLogger CreateLogger()
        {
            return new PtNlogLogger();
        }
    }
}