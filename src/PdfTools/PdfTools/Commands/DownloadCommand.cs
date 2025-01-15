using System;
using System.IO;
using PdfTools.Logging;
using PdfTools.Logging.Contracts;

namespace PdfTools.Commands
{
    public class DownloadCommand : ICommand, IDownloadCommand
    {
        private readonly IPtLogger _logger;
        private readonly IHttpClient _client;

        public DownloadCommand(IPtLogger logger, IHttpClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));

        }

        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], "download", StringComparison.CurrentCultureIgnoreCase) && context.Length == 3;
        }

        public void Execute(string[] context)
        {
            var pdf = _client.GetPdf(context[1]);
            File.WriteAllBytes(context[2], pdf);
        }
    }

    public interface IDownloadCommand : ICommand
    {
    }
}