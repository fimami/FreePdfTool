using Microsoft.Extensions.Logging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using System.Text;

namespace FreePdfTool.Converter.Services
{
    public class PdfOperations : IPdfOperations
    {
        private readonly ILogger<PdfOperations> _logger;

        public PdfOperations(ILogger<PdfOperations> logger)
        {
            _logger = logger;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public void ConcatenateDocuments(List<byte[]> orderedFilePaths, string outPath, bool openAfterSave)
        {
            // Open the output document
            PdfDocument outputDocument = new PdfDocument();
            
            // Iterate files
            foreach (byte[] file in orderedFilePaths)
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
    }
}
