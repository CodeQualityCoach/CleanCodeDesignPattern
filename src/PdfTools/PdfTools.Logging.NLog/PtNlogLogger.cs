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
        private readonly ILogger _logger;

        public PtNlogLogger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }
    }
}
