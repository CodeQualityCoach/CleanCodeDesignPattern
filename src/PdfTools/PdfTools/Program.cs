using System;
using System.Linq;
using Castle.Core.Logging;
using PdfTools.Commands;
using PdfTools.Logging;
using PdfTools.ServiceLocator;

namespace PdfTools
{
    public class Program
    {
        public static void Main(string[] args)
        {

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

            var commands = PtServiceLocator.GetCommands();

            var theAction = commands.FirstOrDefault(c => c.CanExecute(args))
                            ?? new EmptyCommand();

            // Logge das gefundene Command fürs Tracing
            var factory = PtServiceLocator.GetLoggerFactory();
            var logger = factory.CreateLogger();
            logger.Trace("Found Action: " + theAction.GetType().FullName);

            theAction.Execute(args);

            // Execute war während Mitternacht
            logger = factory.CreateLogger();
            logger.Trace("Action executed");

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}