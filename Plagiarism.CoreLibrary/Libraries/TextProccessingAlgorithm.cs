using Annytab;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Plagiarism.CoreLibrary.Libraries
{
    public class TextProcessing
    {
        private string _tokenizingPattern;
        private string _purifyingWordsPattern;
        private string _commonEnglishWords;
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

        public TextProcessing(string commonEnglishWords)
        {
            this._tokenizingPattern = @"[\s\n\r\f]";
            this._purifyingWordsPattern = @"((^[\.\!\@\#\,\?\(\)]+?$)|(^[\.\!\@\#\,\?\(\)]+?)|([\.\!\@\#\,\?\(\)]+?$)|(^[1-9]+?$)|(^[\d\W]+?$))";
            this._commonEnglishWords = commonEnglishWords;
        }

        public TextProcessing(string tokenizingPattern, string purifyingWordsPattern, string commonEnglishWords)
        {
            this._tokenizingPattern = tokenizingPattern;
            this._purifyingWordsPattern = purifyingWordsPattern;
            this._commonEnglishWords = commonEnglishWords;
        }

        public string TokenizingPattern
        {
            get
            {
                return _tokenizingPattern;
            }
            set
            {
                if (value != _tokenizingPattern)
                    _tokenizingPattern = value;
            }
        }

        public string PurifyingWordsPattern
        {
            get
            {
                return _purifyingWordsPattern;
            }
            set
            {
                if (value != _purifyingWordsPattern)
                    _purifyingWordsPattern = value;
            }
        }

        public string CommonEnglishWords
        {
            get
            {
                return _commonEnglishWords;
            }
            set
            {
                if (value != _commonEnglishWords)
                    _commonEnglishWords = value;
            }
        }

        public string[] Process(string text)
        {
            //Check if string is empty/null.
            //If empty/null, just throw it.

            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text), "Parameter 'text' cannot be null");

            //1. Extract common words

            string[] commonWords = ExtractCommonEnglishWords(_commonEnglishWords);


            //2. Tokenizing process
            string[] tokenizedString = Regex.Split(text, _tokenizingPattern);

            //3. Purifying left over words from other common unexpected symbols
            tokenizedString = tokenizedString
                .Where(x => !string.IsNullOrEmpty(x) || !string.IsNullOrWhiteSpace(x))
                .Select(x => Regex.Replace(x.Trim(), _purifyingWordsPattern, ""))
                .ToArray();

            //4. Stopwords Removal process
            string[] stopwordsRemoval = tokenizedString.Select(x => x.ToLower()).Except(commonWords).ToArray();

            //5. Stemming process
            EnglishStemmer stemmer = new EnglishStemmer();
            string[] stemmedWords = stemmer.GetSteamWords(stopwordsRemoval);


            //6. Sorting process
            string[] sortedString = stemmedWords.Select(x => Regex.Replace(x,
                @"((^[\.\!\@\#\,\?\(\)]+?$)|(^[\.\!\@\#\,\?\(\)]+?)|([\.\!\@\#\,\?\(\)]+?$)|(^[1-9]+?$)|(^\W+$))", ""))
                .Where(x => !string.IsNullOrEmpty(x) || !string.IsNullOrWhiteSpace(x)).OrderBy(x => x)
                .ToArray();

            return sortedString;

        }
    }

}
