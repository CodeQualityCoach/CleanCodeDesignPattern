using System;
using System.Collections.Generic;
using PdfTools.Commands;
using PdfTools.Decorator;
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
                new BatchCommand("daac", new DownloadCommand(logger, new HttpClientFacade()), new AddCodeCommand(LoggerFactory.Value)),
                new DownloadAndAddCodeCommand(new DownloadCommand(logger, new HttpClientFacade()), new AddCodeCommand(LoggerFactory.Value)),
                new LoggingCommandDecorator( new DownloadCommand(logger, new HttpClientFacade()), logger),
                new LoggingCommandDecorator(new AddCodeCommand(LoggerFactory.Value), logger),
                new LoggingCommandDecorator( new ArchiveCommand(new PdfArchiver(LoggerFactory.Value, new HttpClientFacade())), logger),
                new LoggingCommandDecorator( new CombineCommand(logger, null), logger),
                new LoggingCommandDecorator(  new CreateCommand(LoggerFactory.Value.CreateLogger()), logger)
            };
        }

        public static IPtLoggerFactory GetLoggerFactory()
        {
            return LoggerFactory.Value;
        }
    }
}