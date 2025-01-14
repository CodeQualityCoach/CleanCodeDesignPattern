using System;
using System.IO;
using PdfTools.Logging.Contracts;

namespace PdfTools.Logging
{
    public class PtFileLogger : IPtLogger
    {
        private readonly string _fileName;

        public PtFileLogger(string fileName)
        {
            _fileName = fileName;
        }

        public void Trace(string message)
        {
            File.AppendAllLines(_fileName, new[] { message });
        }
    }
}