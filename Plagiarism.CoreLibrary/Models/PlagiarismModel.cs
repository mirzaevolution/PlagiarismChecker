using System;

namespace Plagiarism.CoreLibrary.Models
{
    public class PlagiarismModel
    {
        public float Percentage { get; set; }
        public int PercentageInteger { get; set; }
        public string Description { get; set; }
        public int Distance { get; set; }
        public int Max { get; set; }
        public string[] TargetArray { get; set; }
        public string Target { get; set; }
        public string[] SourceArray { get; set; }
        public string Source { get; set; }
        public TimeSpan CalculationTime { get; set; }
    }
}
