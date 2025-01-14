using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using PdfTools.Logging.Contracts;

namespace PdfTools.Logging.NLog
{
    internal class PtNlogLogger : IPtLogger
    {
        public void Trace(string message)
        {
            LogManager.GetCurrentClassLogger().Trace(message);
        }
    }
}
