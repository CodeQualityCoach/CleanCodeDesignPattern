using System;
using System.IO;
using PdfTools.Logging;
using PdfTools.Logging.Contracts;

namespace PdfTools
{
    public class PtLoggerFactory : IPtLoggerFactory
    {
        public IPtLogger CreateLogger()
        {
            var dailyFileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            var fileName = Path.Combine(Path.GetTempPath(), dailyFileName);
            return new PtFileLogger(fileName);
        }
    }
}