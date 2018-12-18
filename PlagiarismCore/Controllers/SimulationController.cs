using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using Plagiarism.CoreLibrary.Libraries;
using Plagiarism.CoreLibrary.Models;
using PlagiarismCore.Models.IdentityModels;
using System.Text;
using Annytab;
using PlagiarismCore.Extensions;

namespace PlagiarismCore.Controllers
{
    public class SimulationController : BaseController
    {
        #region Core Methods
        private string[] ExtractCommonEnglishWords(string commonEnglishWords)
        {

            string[] result = null;
            try
            {

                result = Regex.Split(commonEnglishWords, @"\s*?\,\s*?");
            }
            catch { result = null; }
            return result;
        }
        public string Tokenizing(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            string tokenizingPattern = @"[\s\n\r\f]";
            var resultArr = Regex.Split(text, tokenizingPattern);
            StringBuilder sb = new StringBuilder();
            int totalWords = 0;
            foreach (var str in resultArr)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    totalWords++;
                    sb.AppendLine(str);
                }
            }
            sb.Insert(0, $"Total Words: {totalWords}\n");
            return sb.ToString();
        }
        public string[] TokenizingArray(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new string[0];
            string tokenizingPattern = @"[\s\n\r\f]";
            var resultArr = Regex.Split(text, tokenizingPattern);
            return resultArr;
        }

        public string Purifying(string text)
        {
            string[] tokenizedString = TokenizingArray(text);
            if(tokenizedString.Length>0)
            {
                string purifyingWordsPattern = @"((^[\.\!\@\#\,\?\(\)\;\:\-\'\""\`\’]+?$)|(^[\.\!\@\#\,\?\(\)\'\-\;\:\""\`\’]+?)|([\.\!\@\#\,\?\(\)\-\;\:\'\-\""\`\’]+?$)|(^[1-9]+?$)|(^[\d\W]+?$))";
                tokenizedString = tokenizedString
               .Where(x => !string.IsNullOrEmpty(x) || !string.IsNullOrWhiteSpace(x))
               .Select(x => Regex.Replace(x.Trim(), purifyingWordsPattern, ""))
               .ToArray();
                int totalWords = 0;
                StringBuilder sb = new StringBuilder();
                foreach(var r in tokenizedString)
                {
                    if(!string.IsNullOrEmpty(r))
                    {
                        totalWords++;
                        sb.AppendLine(r);
                    }
                }
                sb.Insert(0, $"Total Words: {totalWords}\n");
                return sb.ToString();
            }
            return string.Empty;
        }
        public string[] PurifyingArray(string text)
        {
            string[] tokenizedString = TokenizingArray(text);
            if (tokenizedString.Length > 0)
            {
                string purifyingWordsPattern = @"((^[\.\!\@\#\,\?\(\)\;\:\-\'\""\`\’]+?$)|(^[\.\!\@\#\,\?\(\)\'\-\;\:\""\`\’]+?)|([\.\!\@\#\,\?\(\)\-\;\:\'\-\""\`\’]+?$)|(^[1-9]+?$)|(^[\d\W]+?$))";
                tokenizedString = tokenizedString
               .Where(x => !string.IsNullOrEmpty(x) || !string.IsNullOrWhiteSpace(x))
               .Select(x => Regex.Replace(x.Trim(), purifyingWordsPattern, ""))
               .ToArray();
                return tokenizedString;
            }
            return new string[0];
        }

        public string StopwordsRemoval(string text)
        {

            string[] tokenizedString = PurifyingArray(text);
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            string[] baseWordsArray = ExtractCommonEnglishWords(BaseWords.GetBaseWords());
            string[] stopwordsRemoval = tokenizedString.Select(x => x.ToLower()).Except(baseWordsArray).ToArray();
            int totalWords = 0;
            StringBuilder sb = new StringBuilder();
            foreach(var word in stopwordsRemoval)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    totalWords++;
                    sb.AppendLine(word);
                }
            }
            sb.Insert(0, $"Total Words: {totalWords}\n");

            return sb.ToString();
        }
        public string[] StopwordsRemovalArray(string text)
        {
            string[] tokenizedString = PurifyingArray(text);
            if (tokenizedString.Length == 0)
            {
                return new string[0];
            }
            string[] baseWordsArray = ExtractCommonEnglishWords(BaseWords.GetBaseWords());

            string[] stopwordsRemoval = tokenizedString.Select(x => x.ToLower()).Except(baseWordsArray).ToArray();
            return stopwordsRemoval;
        }

        public string Stemming(string text)
        {
            string[] words = StopwordsRemovalArray(text);
            if (words.Length == 0)
                return string.Empty;
            EnglishStemmer stemmer = new EnglishStemmer();
            string[] result = stemmer.GetSteamWords(words);
            int totalWords = 0;
            StringBuilder sb = new StringBuilder();
            foreach(var i in result)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    sb.AppendLine(i);
                }
            }
            sb.Insert(0, $"Total Words: {totalWords}\n");

            return sb.ToString();
        }
        public string[] StemmingArray(string text)
        {
            string[] words = StopwordsRemovalArray(text);
            if (words.Length == 0)
                return new string[0];
            EnglishStemmer stemmer = new EnglishStemmer();
            string[] result = stemmer.GetSteamWords(words);
            return result;
        }

        public string Sorting(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            var stemmedWords = StemmingArray(text);
            var result = stemmedWords.Select(x => Regex.Replace(x,
               @"((^[\.\!\@\#\,\?\(\)]+?$)|(^[\.\!\@\#\,\?\(\)]+?)|([\.\!\@\#\,\?\(\)]+?$)|(^[1-9]+?$)|(^\W+$))", ""))
                .Where(x => !string.IsNullOrEmpty(x) || !string.IsNullOrWhiteSpace(x)).OrderBy(x => x)
                .ToArray();
            int totalWords = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var i in result)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    totalWords++;
                    sb.AppendLine(i);
                }
            }
            sb.Insert(0, $"Total Words: {totalWords}\n");

            return sb.ToString();
        }
        public string[] SortingArray(string text)
        {

            if (string.IsNullOrEmpty(text))
                return new string[0];
            var stemmedWords = StemmingArray(text);
            var result = stemmedWords.Select(x => Regex.Replace(x,
               @"((^[\.\!\@\#\,\?\(\)]+?$)|(^[\.\!\@\#\,\?\(\)]+?)|([\.\!\@\#\,\?\(\)]+?$)|(^[1-9]+?$)|(^\W+$))", ""))
                .Where(x => !string.IsNullOrEmpty(x) || !string.IsNullOrWhiteSpace(x)).OrderBy(x => x)
                .ToArray();
            return result;
        }


        public int WordCount(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string pattern = @"[\w\d\@!_?""'&()%-]";
                return Regex.Matches(text, pattern, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase).Count;
            }
            return 0;
        }
        #endregion

        #region Web

        public ActionResult Index()
        {
            return View(new UploadSimulationModel
            {
                ComparerText = "",
                SampleText = "",
                Step = UploadSimulationSteps.INIT
            });
        }
        [HttpPost]
        public ActionResult UploadHandler(UploadSimulationModel model)
        {
            if (Request.Files.Count > 0 && Request.Files.Count>1)
            {
                try
                {
                    var sampleFile = Request.Files["sampleFile"];
                    var comparerFile = Request.Files["comparerFile"];
                    string sampleFileOriginalText = string.Empty;
                    string comparerFileOriginalText = string.Empty;
                    sampleFileOriginalText = PDFReader.ExtractTextFromPdf(sampleFile.InputStream);
                    comparerFileOriginalText = PDFReader.ExtractTextFromPdf(comparerFile.InputStream);

                    int sampleCharCount = sampleFileOriginalText.Replace("\r","").Length;
                    int comparerCharCount = comparerFileOriginalText.Replace("\r", "").Length;

                    int sampleWordCount = WordCount(sampleFileOriginalText);
                    int comparerWordCount = WordCount(comparerFileOriginalText);
                    model.SampleCharCount = sampleCharCount;
                    model.SampleWordCount = sampleWordCount;
                    model.ComparerCharCount = comparerCharCount;
                    model.ComparerWordCount = comparerWordCount;
                    model.Step = UploadSimulationSteps.INIT;
                    model.SampleText = sampleFileOriginalText;
                    model.ComparerText = comparerFileOriginalText;
                    model.IsReadyToProcess = "true";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return View("Index", model);
        }

        [HttpPost]
        public JsonResult TokenizingHandler(string sample, string comparer)
        {
            string sampleResult = Tokenizing(sample);
            string comparerResult = Tokenizing(comparer);

            return Json(new UploadSimulationModel
            {
                IsReadyToProcess = "true",
                ComparerText = comparerResult,
                SampleText = sampleResult,
                Step = UploadSimulationSteps.TOKENIZING
            });
        }

        [HttpPost]
        public JsonResult PurifyingHandler(string sample, string comparer)
        {
            string sampleResult = Purifying(sample);
            string comparerResult = Purifying(comparer);
            return Json(new UploadSimulationModel
            {
                IsReadyToProcess = "true",
                ComparerText = comparerResult,
                SampleText = sampleResult,
                Step = UploadSimulationSteps.PURIFYING
            });
        }

        [HttpPost]
        public JsonResult StopwordsHandler(string sample, string comparer)
        {
            string sampleResult = StopwordsRemoval(sample);
            string comparerResult = StopwordsRemoval(comparer);
            return Json(new UploadSimulationModel
            {
                IsReadyToProcess = "true",
                ComparerText = comparerResult,
                SampleText = sampleResult,
                Step = UploadSimulationSteps.STOPWORDS
            });
        }

        [HttpPost]
        public JsonResult StemmingHandler(string sample, string comparer)
        {
            string sampleResult = Stemming(sample);
            string comparerResult = Stemming(comparer);
            return Json(new UploadSimulationModel
            {
                IsReadyToProcess = "true",
                ComparerText = comparerResult,
                SampleText = sampleResult,
                Step = UploadSimulationSteps.STEMMING
            });
        }

        [HttpPost]
        public JsonResult SortingHandler(string sample, string comparer)
        {
            string sampleResult = Sorting(sample);
            string comparerResult = Sorting(comparer);
            return Json(new UploadSimulationModel
            {
                IsReadyToProcess = "true",
                ComparerText = comparerResult,
                SampleText = sampleResult,
                Step = UploadSimulationSteps.SORTING
            });
        }

        [HttpPost]
        public JsonResult Check(string sample, string comparer)
        {
            int sampleCharCount = sample.Length;
            int comparerCharCount = comparer.Length;

            int sampleWordCount = WordCount(sample);
            int comparerWordCount = WordCount(comparer);

            TextProcessing textProcessing = new TextProcessing(BaseWords.GetBaseWords());
            string[] sampleArray = textProcessing.Process(sample);
            string[] comparerArray = textProcessing.Process(comparer);
            PlagiarismChecker checker = new PlagiarismChecker();
            PlagiarismModel result = checker.Check(sampleArray, comparerArray);
            return Json(new
            {
                result.CalculationTime,
                result.Description,
                result.Distance,
                Max = Math.Max(sampleCharCount,comparerCharCount),
                result.PercentageInteger,
                SampleCharCount = sampleCharCount,
                SampleWordCount = sampleWordCount,
                ComparerCharCount = comparerCharCount,
                ComparerWordCount = comparerWordCount
            });
        }
        #endregion
    }
}