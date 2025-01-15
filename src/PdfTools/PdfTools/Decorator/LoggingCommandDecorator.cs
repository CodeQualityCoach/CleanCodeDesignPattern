using System;
using PdfTools.Commands;
using PdfTools.Logging.Contracts;

namespace PdfTools.Decorator
{
    internal class LoggingCommandDecorator : ICommand
    {
        private readonly ICommand _command;
        private readonly IPtLogger _logger;

        public LoggingCommandDecorator(ICommand command, IPtLogger logger)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public bool CanExecute(string[] context)
        {
            _logger.Trace($"Checking if command can be executed with context: {string.Join(" ", context)}");

            return _command.CanExecute(context);
        }

        public void Execute(string[] context)
        {
            _logger.Trace($"Executing command with context: {string.Join(" ", context)}");

            _command.Execute(context);
        }
    }
}