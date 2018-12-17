namespace PlagiarismCore.Models.IdentityModels
{
    public class UploadSimulationSteps
    {
        public const string INIT = "INIT";
        public const string TOKENIZING = "TOKENIZING";
        public const string PURIFYING = "PURIFYING";
        public const string STOPWORDS = "STOPWORDS";
        public const string STEMMING = "STEMMING";
        public const string SORTING = "SORTING";
        public const string FINAL = "FINAL";
    }
    public class UploadSimulationModel
    {
        public string IsReadyToProcess { get; set; } = "false";
        public string SampleText { get; set; }
        public string ComparerText { get; set; }
        public string Step { get; set; }
    }
}