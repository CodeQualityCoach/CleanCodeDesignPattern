using System;
using PdfTools.PdfServices;

namespace PdfTools.Commands
{
    public class ArchiveCommand : ICommand
    {
        private readonly PdfArchiver _pdfArchiver;

        public ArchiveCommand(PdfArchiver pdfArchiver)
        {
            _pdfArchiver = pdfArchiver;
        }
        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], "archive", StringComparison.CurrentCultureIgnoreCase) && context.Length == 2;
        }

        public void Execute(string[] context)
        {
            _pdfArchiver.Archive(context[1]);
            _pdfArchiver.SaveAs(context[2]);
        }
    }
}