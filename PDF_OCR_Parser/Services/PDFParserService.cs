using System.Text;
using PdfOcrManager.Interfaces;
using SixLabors.ImageSharp;
using Tesseract;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace PdfOcrManager.Services
{
    public class PdfParserService : IPdfParserService
    {
        public async Task<String> PDFtoText(IFormFile file)
        {
            var documentText = new StringBuilder();

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                using PdfDocument pdf = PdfDocument.Open(ms);

                using var engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default);
                documentText.AppendLine("\n\n(tesseract) initialized eng");

                var pdfPages = pdf.GetPages();
                documentText.AppendLine($"(info) found {pdfPages.Count()} pages");

                int pgNum = 1;
                foreach (UglyToad.PdfPig.Content.Page page in pdfPages)
                {
                    ++pgNum;

                    // first check if the page is actual pdf bytecode
                    IReadOnlyList<Letter> letters = page.Letters;

                    var searchableText = string.Join(string.Empty, letters.Select(x => x.Value));
                    if (!string.IsNullOrEmpty(searchableText.Trim()))
                    {
                        documentText.AppendLine($"(info) page {pgNum} is pdf bytecode");
                        documentText.AppendLine($"(info)              text equivalent:");
                        documentText.AppendLine(searchableText);
                        // ... bytecode applied as text to our string builder
                        // no more work to be done with this page
                        documentText.AppendLine($"(info) end of page {pgNum}");
                        documentText.AppendLine(". . . . ");
                        continue; // move to the next page
                    }

                    // pdf page is not bytecode, must be embedded images
                    int numImgs = page.NumberOfImages;
                    documentText.AppendLine($"(info) page {pgNum} has {numImgs} image(s)");

                    int imgNum = 0;
                    foreach (var img in page.GetImages())
                    {
                        ++imgNum;

                        // png extraction is the safest as that is what the ocr needs...
                        if (!img.TryGetPng(out var bytes))
                        {
                            // but if the embedded image is not pdf, try to get bytes
                            if (!img.TryGetBytes(out var b))
                            {
                                // last hope, just use the raw bytes
                                b = img.RawBytes;
                            }

                            if (b != null)
                            {
                                documentText.AppendLine(
                                    $"(info) page {pgNum} image {imgNum} bytes extracted"
                                );
                                bytes = b.Select(i => (byte)i).ToArray();
                            }
                            else
                            {
                                documentText.AppendLine(
                                    $"(error) page {pgNum} image {imgNum} bytes failed to extract - page text not available"
                                );
                            }
                        }

                        if (bytes != null)
                        {
                            using var stream = new MemoryStream(bytes.ToArray());

                            stream.Position = 0;
                            using var image = Image.Load(stream);

                            string pageImage = $"page_{pgNum}_{imgNum}.png";
                            image.SaveAsPng(pageImage);

                            // perform ocr
                            using (Pix _img = Pix.LoadFromFile(pageImage))
                            {
                                using Tesseract.Page recognizedPage = engine.Process(_img);
                                documentText.AppendLine(
                                    $"(tesseract) page {pgNum}, mean confidence: {recognizedPage.GetMeanConfidence()}"
                                );
                                documentText.AppendLine("(tesseract) recognized text:");
                                documentText.AppendLine(recognizedPage.GetText());
                            }
                            File.Delete(pageImage);
                        }
                    }
                }
            }

            return documentText.ToString();
        }
    }
}
