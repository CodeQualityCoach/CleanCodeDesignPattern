using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using FSharp.Markdown;
using FSharp.Markdown.Pdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NLog;
using QRCoder;
using Image = iTextSharp.text.Image;

namespace PdfTools
{
    public interface ICommand
    {
        bool CanExecute(string[] context);
        void Execute(string[] context);
    }

    public interface ICommand<TEntity> : ICommand
    {
        bool CanExecute(TEntity context);
        void Execute(TEntity context);
    }

    public class ArchiveStrategy : ICommand
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

    public class AddCodeStrategy : ICommand
    {
        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], "addcode", StringComparison.CurrentCultureIgnoreCase) && context.Length == 4;
        }


        public void Execute(string[] context)
        {
            var enhancer = new PdfCodeEnhancer(context[2]);

            enhancer.AddTextAsCode(context[1]);

            if (context.Length == 5)
                enhancer.SaveAs(context[4]);
            else
                enhancer.SaveAs(context[2]);
        }
    }

    public class DownloadStrategy : ICommand
    {
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

    //public class CombineStrategy : ICommand
    //{
    //    public void Execute(string[] argsWithoutCommand)
    //    {
    //        CombineMultiplePDF(argsWithoutCommand.Skip(1).ToArray(), argsWithoutCommand[0]);
    //    }

    //    private static void CombineMultiplePDF(string[] fileNames, string outFile)
    //    {
    //        // Todo_RM: _logger.Trace($"Combile multiple pdf files {string.Join(",", fileNames)} into {outFile}");

    //        // step 1: creation of a document-object
    //        Document document = new Document();
    //        //create newFileStream object which will be disposed at the end
    //        using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
    //        {
    //            // step 2: we create a writer that listens to the document
    //            PdfCopy writer = new PdfCopy(document, newFileStream);

    //            // step 3: we open the document
    //            document.Open();

    //            foreach (string fileName in fileNames)
    //            {
    //                // we create a reader for a certain document
    //                PdfReader reader = new PdfReader(fileName);
    //                reader.ConsolidateNamedDestinations();

    //                // step 4: we add content
    //                for (int i = 1; i <= reader.NumberOfPages; i++)
    //                {
    //                    PdfImportedPage page = writer.GetImportedPage(reader, i);
    //                    writer.AddPage(page);
    //                }

    //                //PRAcroForm form = reader.AcroForm;
    //                //if (form != null)
    //                //{
    //                //    writer.AddDocument(reader);
    //                //}

    //                reader.Close();
    //            }

    //            // step 5: we close the document and writer
    //            writer.Close();
    //            document.Close();
    //        }//disposes the newFileStream object
    //    }
    //}

    //public class CreateStrategy : ICommand
    //{
    //    public void Execute(string[] argsWithoutCommand)
    //    {
    //        DoCreate(argsWithoutCommand);
    //    }
    //    private static void DoCreate(string[] args)
    //    {
    //        if (args.Length != 2)
    //            throw new ArgumentException("at least in and out parameter is required");

    //        //_logger.Trace("Creating pdf for a markdown file");

    //        var inFile = args[0];
    //        var outFile = args[1];

    //        var mdText = File.ReadAllText(inFile);
    //        var mdDoc = Markdown.Parse(mdText);

    //        MarkdownPdf.Write(mdDoc, outFile);
    //    }
    //}

    public class Program
    {
        private static Logger _logger;

        public static void Main(string[] args)
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();

#if DEBUG
            // just a hack in case you hit play in VS
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Args (Comma Separated): ");
                var arg = Console.ReadLine() ?? "help";
                args = arg.Split(',').Select(x => x.Trim()).ToArray();
            }
#endif

            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }

            var commands = new List<ICommand>() {
                new DownloadStrategy(),
                new AddCodeStrategy(),
                new ArchiveStrategy()
            };

            var theAction = commands.Where(c => c.CanExecute(args)).FirstOrDefault();

            theAction.Execute(args);

            //if (args.Length == 0)
            //    throw new ArgumentException("at least an action is required");

            //var action = args[0].ToLower();

            //ICommand theAction = null;

            //var actionDictionary = new Dictionary<string, IStrategy>() {
            //    { "download", new DownloadStrategy() },
            //    { "addcode", new AddCodeStrategy() },
            //    { "archive", new ArchiveStrategy() }
            //};
            //theAction = actionDictionary[action];
            //theAction.Do(args.Skip(1).ToArray());

            // markdown-in, pdf-out
            //if (string.Equals(action, "create", StringComparison.CurrentCultureIgnoreCase))
            //    theAction = new CreateStrategy();

            //// pdf-in, qrcodetext, optional outfile
            //if (string.Equals(action, "addcode", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    theAction = new AddCodeStrategy();
            //}

            //// url, outfile
            //if (string.Equals(action, "download", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    theAction = new DownloadStrategy();
            //}

            //// url, outfile
            //if (string.Equals(action, "archive", StringComparison.CurrentCultureIgnoreCase))
            //{
            //   theAction = new ArchiveStrategy();
            //}

            ////// url, outfile
            //if (string.Equals(action, "combine", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    theAction = new CombineStrategy();
            //}

            // Strategy nutzen ohne implementierung zu kennen
            theAction.Execute(args);

#if DEBUG
            Console.ReadKey();
#endif
        }
    }

    public class PdfArchiver
    {
        private readonly string _tempFile;

        public PdfArchiver()
        {
            _tempFile = Path.GetTempFileName();
        }
        public void Archive(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var pdf = response.Content.ReadAsByteArrayAsync().Result;

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

    public class PdfCodeEnhancer
    {
        private readonly string _pdfFile;
        private readonly string _tempFile;

        public PdfCodeEnhancer(string pdfFile)
        {
            _pdfFile = pdfFile;
            _tempFile = Path.GetTempFileName();
        }

        public void AddTextAsCode(string text)
        {
            using (Stream inputPdfStream = new FileStream(_pdfFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new MemoryStream())
            using (Stream outputPdfStream = new FileStream(_tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var code = CreateInitCode(text);
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
            var qrCodeData = qrCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(2);
        }

        public void SaveAs(string destFile)
        {
            File.Copy(_tempFile, destFile, true);
        }
    }
}
