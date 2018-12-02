using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plagiarism.CoreLibrary.Models;
using System.Diagnostics;

namespace Plagiarism.CoreLibrary.Libraries
{
    public class PlagiarismChecker
    {
        public string Language { get; set; } = "English";
        private string TransformString(string[] array)
        {
            if (array != null && array.Length > 0)
            {
                string result = string.Empty;
                result = array.Aggregate((current, next) => current + "\n" + next);
                return result;
            }
            return string.Empty;
        }


        public PlagiarismModel Check(string[] target, string[] source)
        {
            PlagiarismModel result = new PlagiarismModel();
            try
            {
                result.TargetArray = target;
                result.SourceArray = source;

                string targetStr = TransformString(target);
                string sourceStr = TransformString(source);

                int targetLength = targetStr.Length;
                int sourceLength = sourceStr.Length;

                result.Target = targetStr;
                result.Source = sourceStr;

                Stopwatch stopwatch = Stopwatch.StartNew();

                result.Distance = EditDistance.CalculateLevenshtein(sourceStr, targetStr);

                result.Max = Math.Max(targetLength, sourceLength);

                result.Percentage = ((1 - ((float)result.Distance / (float)result.Max)) * 100);

                int percentageInt = Convert.ToInt32(result.Percentage);
                result.PercentageInteger = percentageInt;

                if (percentageInt < 30)
                {
                    switch (Language.ToLower().Trim())
                    {
                        case "Indonesia":
                            result.Description = "Level plagiarisme rendah";
                            break;
                        default:
                            result.Description = "Plagiarism level is low";
                            break;

                    }
                }
                else if (percentageInt <= 70)
                {

                    switch (Language.ToLower().Trim())
                    {
                        case "Indonesia":
                            result.Description = "Level plagiarisme sedang";
                            break;
                        default:
                            result.Description = "Plagiarism level is intermediate";
                            break;
                    }
                }
                else if (percentageInt > 70)
                {
                    switch (Language.ToLower().Trim())
                    {
                        case "Indonesia":
                            result.Description = "Level plagiarisme tinggi";
                            break;
                        default:
                            result.Description = "Plagiarism level is high";
                            break;
                    }
                }

                stopwatch.Stop();
                result.CalculationTime = stopwatch.Elapsed;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            return result;
        }
        public PlagiarismModel Check(string[] target, string sourceFromDb)
        {
            PlagiarismModel result = new PlagiarismModel();
            try
            {
                result.TargetArray = target;
                result.SourceArray = null;

                string targetStr = TransformString(target);
                string sourceStr = sourceFromDb;

                int targetLength = targetStr.Length;
                int sourceLength = sourceStr.Length;

                result.Target = targetStr;
                result.Source = sourceStr;

                Stopwatch stopwatch = Stopwatch.StartNew();

                result.Distance = EditDistance.CalculateLevenshtein(sourceStr, targetStr);

                result.Max = Math.Max(targetLength, sourceLength);

                result.Percentage = ((1 - ((float)result.Distance / (float)result.Max)) * 100);

                int percentageInt = Convert.ToInt32(result.Percentage);
                result.PercentageInteger = percentageInt;

                if (percentageInt < 30)
                {
                    switch (Language.ToLower().Trim())
                    {
                        case "Indonesia":
                            result.Description = "Level plagiarisme rendah";
                            break;
                        default:
                            result.Description = "Plagiarism level is low";
                            break;

                    }
                }
                else if (percentageInt <= 70)
                {

                    switch (Language.ToLower().Trim())
                    {
                        case "Indonesia":
                            result.Description = "Level plagiarisme sedang";
                            break;
                        default:
                            result.Description = "Plagiarism level is intermediate";
                            break;
                    }
                }
                else if (percentageInt > 70)
                {
                    switch (Language.ToLower().Trim())
                    {
                        case "Indonesia":
                            result.Description = "Level plagiarisme tinggi";
                            break;
                        default:
                            result.Description = "Plagiarism level is high";
                            break;
                    }
                }

                stopwatch.Stop();
                result.CalculationTime = stopwatch.Elapsed;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            return result;
        }
    }
}
