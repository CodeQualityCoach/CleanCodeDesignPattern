namespace PdfTools.Commands
{
    public interface IHttpClient
    {
        byte[] GetPdf(string url);
    }
}