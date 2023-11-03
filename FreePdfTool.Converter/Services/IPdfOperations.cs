namespace FreePdfTool.Converter.Services
{
    internal interface IPdfOperations
    {
        void ConcatenateDocuments(List<byte[]> orderedFilePaths, string outPath, bool openAfterSave);
    }
}
