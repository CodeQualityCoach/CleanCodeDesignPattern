using System;
using System.Text;
using System.Threading.Tasks;
using PdfTools.Commands;

namespace PdfTools.Decorator
{
    internal class EmptyDecorator : ICommand
    {
        private readonly ICommand _command;

        public EmptyDecorator(ICommand command)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public bool CanExecute(string[] context)
        {
            return _command.CanExecute(context);
        }

        public void Execute(string[] context)
        {
            _command.Execute(context);
        }
    }
}
