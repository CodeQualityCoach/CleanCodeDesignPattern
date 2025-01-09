using System;
using System.IO;
using FSharp.Markdown;
using FSharp.Markdown.Pdf;
using PdfTools.Logging;

namespace PdfTools.Commands
{
    public class CreateCommand : ICommand
    {
        private readonly ILogger _logger;

        public CreateCommand(ILogger logger)
        {
            // hack: Frage: Kennt jemand diesen Code?
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], "create", StringComparison.CurrentCultureIgnoreCase) && context.Length == 3;
        }

        public void Execute(string[] context)
        {
            DoCreate(context);
        }
        private void DoCreate(string[] args)
        {
            _logger.Trace("Creating pdf for a markdown file");

            var inFile = args[1];
            var outFile = args[2];

            var mdText = File.ReadAllText(inFile);
            var mdDoc = Markdown.Parse(mdText);

            MarkdownPdf.Write(mdDoc, outFile);
        }
    }
}