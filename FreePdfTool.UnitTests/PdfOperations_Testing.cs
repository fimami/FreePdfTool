using FreePdfTool.Converter.Services;
using Microsoft.Extensions.Logging;
using Moq;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;

namespace FreePdfTool.UnitTests
{
    public class Tests
    {
        private Mock<ILogger<PdfOperations>> _logger;
        private PdfOperations _pdfOperations;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<PdfOperations>>();
            _pdfOperations = new PdfOperations(_logger.Object);
        }

        [Test]
        public void ConcatenateDocuments_ConcatenateTwoDocuments_MergedPdfContainsSumOfPages()
        {
            // Arrange
            var sampleFile = Properties.Resource1.github_git_cheat_sheet;
            var outPath = Path.Combine(Path.GetTempPath(), "test.pdf");
            List<byte[]> files = new List<byte[]> { sampleFile, sampleFile };

            // Act
            _pdfOperations.ConcatenateDocuments(files, outPath, false);

            // Assert
            using (var fs = new FileStream(outPath, FileMode.Open, FileAccess.Read))
            {
                PdfDocument finalDocument = PdfReader.Open(fs, PdfDocumentOpenMode.ReadOnly);
                int finalPages = finalDocument.PageCount;
                int inputPages = 0;
                foreach (var inputFile in files)
                {
                    using (var ms = new MemoryStream(inputFile, false))
                    {
                        PdfDocument input = PdfReader.Open(ms, PdfDocumentOpenMode.ReadOnly);
                        inputPages += input.PageCount;
                    }
                }
                Assert.That(inputPages, Is.EqualTo(finalPages));
            }
        }
    }
}