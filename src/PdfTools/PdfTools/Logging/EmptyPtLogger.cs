﻿using PdfTools.Logging.Contracts;

namespace PdfTools.Logging
{
    public class EmptyPtLogger : IPtLogger
    {
        public void Trace(string message)
        {
        }
    }
}