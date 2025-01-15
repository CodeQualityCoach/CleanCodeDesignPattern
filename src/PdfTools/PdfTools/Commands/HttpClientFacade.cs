using System.Net.Http;

namespace PdfTools.Commands
{
    public class HttpClientFacade : IHttpClient
    {
        public byte[] GetPdf(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var pdf = response.Content.ReadAsByteArrayAsync().Result;
            return pdf;
        }
    }
}