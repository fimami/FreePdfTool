namespace FreePdfTool.Converter.Services
{
    public interface IPdfOperations
    {
        void ConcatenateDocuments(List<byte[]> orderedFiles, string outPath, bool openAfterSave);
        void ConcatenateDocuments(List<string> orderedFilePaths, string outPath, bool openAfterSave);
    }
}
