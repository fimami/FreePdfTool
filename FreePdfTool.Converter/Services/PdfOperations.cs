using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using System.Text;

namespace FreePdfTool.Converter.Services
{
    // TODO: Consider horizontal pages
    public class PdfOperations : IPdfOperations
    {
        private readonly ILogger<PdfOperations> _logger;

        public PdfOperations(ILogger<PdfOperations> logger)
        {
            _logger = logger;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public void ConcatenateDocuments(List<byte[]> orderedFiles, string outPath, bool openAfterSave)
        {
            // Open the output document
            PdfDocument outputDocument = new PdfDocument();

            // Iterate files
            foreach (byte[] file in orderedFiles)
            {
                using (var ms = new MemoryStream(file))
                {
                    // Open the document to import pages from it.
                    PdfDocument inputDocument = PdfReader.Open(ms, PdfDocumentOpenMode.Import);

                    // Iterate pages
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        // Get the page from the external document...
                        PdfPage page = inputDocument.Pages[idx];
                        page.Size = PdfSharp.PageSize.A4;
                        // ...and add it to the output document.
                        outputDocument.AddPage(page);
                    }
                }
            }

            // Save the document...
            outputDocument.Save(outPath);
            // ...and start a viewer.
            if (openAfterSave)
                Process.Start(outPath);
        }

        public void ConcatenateDocuments(List<string> orderedFilePaths, string outPath, bool openAfterSave)
        {
            // Open the output document
            PdfDocument outputDocument = new PdfDocument();

            // Iterate files
            foreach (string path in orderedFilePaths)
            {
                // Open the document to import pages from it.
                PdfDocument inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);

                // Iterate pages
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    // Get the page from the external document...
                    PdfPage page = inputDocument.Pages[idx];
                    page.Size = PdfSharp.PageSize.A4;
                    // ...and add it to the output document.
                    outputDocument.AddPage(page);
                }
            }

            // Save the document...
            outputDocument.Save(outPath);
            // ...and start a viewer.
            if (openAfterSave)
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = outPath;
                p.Start();
            }
        }
    }
}
