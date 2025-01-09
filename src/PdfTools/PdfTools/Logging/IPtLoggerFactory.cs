using PdfTools.Logging;

namespace PdfTools
{
    public interface IPtLoggerFactory
    {
        IPtLogger CreateLogger();
    }
}