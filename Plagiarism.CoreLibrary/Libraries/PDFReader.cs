using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Plagiarism.CoreLibrary.Libraries
{
    public class PDFReader
    {
        public static string ExtractTextFromPdf(string path)
        {
            ITextExtractionStrategy its = new LocationTextExtractionStrategy();
            string result = string.Empty;
            try
            {
                using (PdfReader reader = new PdfReader(path))
                {
                    StringBuilder text = new StringBuilder();

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        string thePage = PdfTextExtractor.GetTextFromPage(reader, i, its);
                        string[] theLines = thePage.Split('\n');
                        foreach (var theLine in theLines)
                        {
                            text.AppendLine(theLine);
                        }
                    }
                    result = text.ToString();
                }
            }
            catch(Exception ex)
            {
                result = string.Empty;
                Trace.WriteLine(ex);
            }
            return result;
        }
        public static string ExtractTextFromPdf(Stream contents)
        {
            ITextExtractionStrategy its = new LocationTextExtractionStrategy();
            string result = string.Empty;
            try
            {
                using (PdfReader reader = new PdfReader(contents))
                {
                    StringBuilder text = new StringBuilder();

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        string thePage = PdfTextExtractor.GetTextFromPage(reader, i, its);
                        string[] theLines = thePage.Split('\n');
                        foreach (var theLine in theLines)
                        {
                            text.AppendLine(theLine);
                        }
                    }
                    return text.ToString();
                }
            }
            catch(Exception ex)
            {
                result = string.Empty;
                Trace.WriteLine(ex);
            }
            return result;
        }
    }
}
