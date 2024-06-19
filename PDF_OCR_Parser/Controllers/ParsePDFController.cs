using Microsoft.AspNetCore.Mvc;
using PdfOcrManager.Interfaces;

namespace PdfOcrManager.Controllers
{
    public class ParsePDFController : Controller
    {
        public ParsePDFController(IPdfParserService pdfParserService)
        {
            _pdfParserService = pdfParserService;
        }

        private readonly IPdfParserService _pdfParserService;

        [HttpPost]
        [Route("api/parsePDF")]
        [Produces("application/json")]
        public async Task<IActionResult> ParsePDF(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }
            var results = await _pdfParserService.PDFtoText(file);
            return Ok(results);
        }
    }
}
