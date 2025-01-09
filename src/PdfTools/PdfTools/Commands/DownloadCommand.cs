using System;
using System.IO;
using System.Net.Http;
using PdfTools.Logging;

namespace PdfTools.Commands
{
    public class DownloadCommand : ICommand
    {
        private readonly ILogger _logger;

        public DownloadCommand(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], "download", StringComparison.CurrentCultureIgnoreCase) && context.Length == 3;
        }

        public void Execute(string[] context)
        {
            var client = new HttpClient();
            var response = client.GetAsync(context[1]).Result;
            var pdf = response.Content.ReadAsByteArrayAsync().Result;

            File.WriteAllBytes(context[2], pdf);
        }
    }
}