using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PdfTools.Commands;
using PdfTools.Logging;

namespace PdfTools
{
    public class Program
    {
        //private static IPtLogger _logger;
        private static IPtLogger _logger;

        public static void Main(string[] args)
        {
            // logik für Objekterstellung
            var factory = new PtLoggerFactory();
            _logger = factory.CreateLogger();

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
                new CreateCommand(factory.CreateLogger())
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

    public interface IPtLoggerFactory
    {
        IPtLogger CreateLogger();
    }

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