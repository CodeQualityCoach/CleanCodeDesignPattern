using System;
using System.Linq;

namespace PdfTools.Commands
{
    public class BatchCommand : ICommand
    {

        private readonly ICommand[] command;
        private readonly string name;

        public BatchCommand(string name, params ICommand[] command)
        {
            this.name = name;
            this.command = command;
        }

        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], name, StringComparison.CurrentCultureIgnoreCase) &&
                   command.All(c => c.CanExecute(context));
        }

        public void Execute(string[] context)
        {
            foreach (var cmd in command)
            {
                cmd.Execute(context);
            }
        }
    }
}
