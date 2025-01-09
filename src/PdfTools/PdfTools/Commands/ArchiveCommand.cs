using System;
using PdfTools.PdfServices;

namespace PdfTools.Commands
{
    public class ArchiveCommand : ICommand
    {
        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], "archive", StringComparison.CurrentCultureIgnoreCase) && context.Length == 2;
        }

        public void Execute(string[] context)
        {
            var archiver = new PdfArchiver();
            archiver.Archive(context[1]);
            archiver.SaveAs(context[2]);
        }
    }
}