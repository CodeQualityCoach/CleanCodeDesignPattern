using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
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

    public class BatchCommand : ICommand
    {

        private readonly ICommand[] command;
        private readonly string name;

        public BatchCommand(string name, params ICommand[] command)
        {
            this.name = name;
            this.command = command;
        }
        public bool CanExecute(string[] context)
        {
            return string.Equals(context[0], name, StringComparison.CurrentCultureIgnoreCase) && command.All(c => c.CanExecute(context));
        }

        public void Execute(string[] context)
        {
            foreach (var cmd in command)
            {
                cmd.Execute(context);
            }
        }

        public class DownloadAndAddCodeCommand : ICommand
        {
            private ICommand cmd1 = new DownloadStrategy();
            private ICommand cmd2 = new AddCodeStrategy();

            public bool CanExecute(string[] context)
            {
                return string.Equals(context[0], "downloadandaddcode", StringComparison.CurrentCultureIgnoreCase);
            }

            public void Execute(string[] context)
            {
                cmd1.Execute(context);
                cmd2.Execute(context);
            }
        }

        public class EmptyCommand : ICommand
        {
            public bool CanExecute(string[] context)
            {
                return true;
            }

            public void Execute(string[] context)
            {
            }
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

        public class CombineStrategy : ICommand
        {
            public bool CanExecute(string[] context)
            {
                return string.Equals(context[0], "combine", StringComparison.CurrentCultureIgnoreCase) && context.Length >= 3;
            }

            public void Execute(string[] context)
            {
                CombineMultiplePDF(context.Skip(2).ToArray(), context[1]);
            }

            private static void CombineMultiplePDF(string[] fileNames, string outFile)
            {
                // todo RW: fix me
                // _logger.Trace($"Combile multiple pdf files {string.Join(",", fileNames)} into {outFile}");

                // step 1: creation of a document-object
                Document document = new Document();
                //create newFileStream object which will be disposed at the end
                using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
                {
                    // step 2: we create a writer that listens to the document
                    PdfCopy writer = new PdfCopy(document, newFileStream);

                    // step 3: we open the document
                    document.Open();

                    foreach (string fileName in fileNames)
                    {
                        // we create a reader for a certain document
                        PdfReader reader = new PdfReader(fileName);
                        reader.ConsolidateNamedDestinations();

                        // step 4: we add content
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            PdfImportedPage page = writer.GetImportedPage(reader, i);
                            writer.AddPage(page);
                        }

                        //PRAcroForm form = reader.AcroForm;
                        //if (form != null)
                        //{
                        //    writer.AddDocument(reader);
                        //}

                        reader.Close();
                    }

                    // step 5: we close the document and writer
                    writer.Close();
                    document.Close();
                }//disposes the newFileStream object
            }
        }

        public class CreateStrategy : ICommand
        {
            public bool CanExecute(string[] context)
            {
                return string.Equals(context[0], "create", StringComparison.CurrentCultureIgnoreCase) && context.Length == 3;
            }

            public void Execute(string[] context)
            {
                DoCreate(context);
            }
            private static void DoCreate(string[] args)
            {
                // todo RW: fix me
                //_logger.Trace("Creating pdf for a markdown file");

                var inFile = args[1];
                var outFile = args[2];

                var mdText = File.ReadAllText(inFile);
                var mdDoc = Markdown.Parse(mdText);

                MarkdownPdf.Write(mdDoc, outFile);
            }
        }

        public class Program
        {
            // todo RW: fix me
            private static Logger _logger;

            public static void Main(string[] args)
            {
                // todo RW: fix me
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
                    new BatchCommand("daac", new DownloadStrategy(), new AddCodeStrategy()),
                    new DownloadAndAddCodeCommand(),
                    new DownloadStrategy(),
                    new AddCodeStrategy(),
                    new ArchiveStrategy(),
                    new CombineStrategy(),
                    new CreateStrategy()
                };

                // we solve a problem:
                //  * argument out of range exception (each command checks for array length)
                //  * but what about null?

                var theAction = commands.Where(c => c.CanExecute(args)).FirstOrDefault()
                    ?? new EmptyCommand();

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
}