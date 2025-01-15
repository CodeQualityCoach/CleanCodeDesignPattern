using System;

namespace PdfTools.Commands
{
    public class DownloadAndAddCodeCommand : ICommand
    {
        private readonly ICommand _downloadCommand;
        private readonly ICommand _addCodeCommand;

        public DownloadAndAddCodeCommand(IDownloadCommand downloadCommand, IAddCodeCommand addCodeCommand)
        {
            this._downloadCommand = downloadCommand;
            this._addCodeCommand = addCodeCommand;
        }

        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], "downloadandaddcode", StringComparison.CurrentCultureIgnoreCase);
        }

        public void Execute(string[] context)
        {
            _downloadCommand.Execute(context);
            _addCodeCommand.Execute(context);
        }
    }
}