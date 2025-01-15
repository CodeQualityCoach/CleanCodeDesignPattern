using System;
using PdfTools.Logging.Contracts;
using PdfTools.PdfServices;

namespace PdfTools.Commands
{
    public class AddCodeCommand : ICommand, IAddCodeCommand
    {
        private readonly IPtLogger _logger;

        public AddCodeCommand(IPtLoggerFactory factory)
        {
            _logger = factory?.CreateLogger() ?? throw new ArgumentNullException(nameof(factory));
        }

        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], "addcode", StringComparison.CurrentCultureIgnoreCase) && context.Length == 4;
        }

        public void Execute(string[] context)
        {
            var enhancer = new PdfCodeEnhancer(context[2]);

            _logger.Trace("enhancer was loaded successfully");
            enhancer.AddTextAsCode(context[1]);

            if (context.Length == 5)
                enhancer.SaveAs(context[4]);
            else
                enhancer.SaveAs(context[2]);
        }
    }

    public interface IAddCodeCommand: ICommand
    {
    }
}