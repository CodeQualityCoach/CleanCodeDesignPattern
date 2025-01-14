using System;
using System.Collections.Generic;
using PdfTools.Commands;
using PdfTools.Logging.Contracts;
using PdfTools.Logging.NLog;
using PdfTools.PdfServices;

namespace PdfTools.ServiceLocator
{
    /// <summary>
    ///  Lifecycle Management
    /// </summary>
    public static class PtServiceLocator
    {
        // Singleton implementation - lazy instead of eager initialization
        private static readonly Lazy<IPtLoggerFactory> LoggerFactory
            //= new Lazy<IPtLoggerFactory>(() => new PtLoggerFactory());
            = new Lazy<IPtLoggerFactory>(() => new PtNlogLoggerFactory());

        public static IEnumerable<ICommand> GetCommands()
        {
            // logik für Objekterstellung
            var logger = LoggerFactory.Value.CreateLogger();

            return new List<ICommand>() {
                new BatchCommand("daac", new DownloadCommand(logger), new AddCodeCommand()),
                new DownloadAndAddCodeCommand(new DownloadCommand(logger), new AddCodeCommand()),
                new DownloadCommand(logger),
                new AddCodeCommand(),
                new ArchiveCommand(new PdfArchiver(LoggerFactory.Value)),
                new CombineCommand(logger),
                new CreateCommand(LoggerFactory.Value.CreateLogger())
            };
        }

        public static IPtLoggerFactory GetLoggerFactory()
        {
            return LoggerFactory.Value;
        }
    }
}