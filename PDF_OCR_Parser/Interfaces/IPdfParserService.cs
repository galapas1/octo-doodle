namespace PdfOcrManager.Interfaces
{
    public interface IPdfParserService
    {
        Task<String> PDFtoText(IFormFile file);
    }
}
