using System;
using System.Collections.Generic;
using System.Linq;
using PdfTools.Commands;

// Frage: Warum so etwas komisches hier? Was bedeutet das?
using ILogger = PdfTools.Logging.ILogger;

namespace PdfTools
{
    public class Program
    {
        //private static ILogger _logger;
        private static ILogger _logger;

        public static void Main(string[] args)
        {
            // todo RW: fix me
            //_logger = NLog.LogManager.GetCurrentClassLogger();
            _logger = new PdfTools.Logging.ConsoleLogger();


#if DEBUG
            // just a hack in case you hit play in VS
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Args (Comma Separated): ");
                var arg = Console.ReadLine() ?? "help";
                args = arg.Split(',').Select(x => x.Trim()).ToArray();
            }
#endif

            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }


            var commands = new List<ICommand>() {
                new BatchCommand("daac", new DownloadCommand(_logger), new AddCodeCommand()),
                new DownloadAndAddCodeCommand(new DownloadCommand(_logger), new AddCodeCommand()),
                new DownloadCommand(_logger),
                new AddCodeCommand(),
                new ArchiveCommand(),
                new CombineCommand(_logger),
                new CreateCommand(_logger)
            };

            // we solve a problem:
            //  * argument out of range exception (each command checks for array length)
            //  * but what about null?

            var theAction = commands.FirstOrDefault(c => c.CanExecute(args))
                            ?? new EmptyCommand();

            theAction.Execute(args);



#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}