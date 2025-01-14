using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using iTextSharp.text.pdf;
using PdfTools.Logging;
using PdfTools.Logging.Contracts;
using QRCoder;
using Image = iTextSharp.text.Image;

namespace PdfTools.PdfServices
{
    public class PdfArchiver
    {
        private readonly string _tempFile;
        private readonly IPtLogger _logger;
        private readonly IPtWebReader _ptWebReader;

        //public PdfArchiver(IPtLogger logger) // statisch (aus Prozessicht)
        public PdfArchiver(IPtLoggerFactory loggerFactory, IPtWebReader ptWebReader = null) // dynamischen Logger
        {
            _tempFile = Path.GetTempFileName();
            _logger = loggerFactory.CreateLogger();

            _ptWebReader = ptWebReader ?? new PtWebReader();
        }

        public void Archive(string url)
        {
            var pdf = _ptWebReader.GetPdf(url);

            var tmpTempFile = Path.GetTempFileName();
            File.WriteAllBytes(tmpTempFile, pdf);

            using (Stream inputPdfStream = new FileStream(tmpTempFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new MemoryStream())
            using (Stream outputPdfStream = new FileStream(_tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var code = CreateInitCode(url);
                code.Save(inputImageStream, ImageFormat.Jpeg);
                inputImageStream.Position = 0;

                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                var image = Image.GetInstance(inputImageStream);
                image.SetAbsolutePosition(5, 5);
                pdfContentByte.AddImage(image);
                stamper.Close();
            }
        }

        private Bitmap CreateInitCode(string text)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(new PayloadGenerator.Url(text), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(2);
        }

        public void SaveAs(string destFile)
        {
            File.Copy(_tempFile, destFile, true);
        }
    }

    public class PtWebReader : IPtWebReader
    {
        public byte[] GetPdf(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            return response.Content.ReadAsByteArrayAsync().Result;
        }
    }

    public interface IPtWebReader
    {
        byte[] GetPdf(string url);
    }
}