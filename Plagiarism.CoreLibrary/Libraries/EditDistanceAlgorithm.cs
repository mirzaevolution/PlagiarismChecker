using System;
using System.Diagnostics;
using System.Linq;

namespace Plagiarism.CoreLibrary.Libraries
{
    public class EditDistance
    {
        public static int CalculateLevenshtein(string original, string modified)
        {
            int originalLength = original.Length;
            int modifiedLength = modified.Length;
            int result = 0;
            try
            {
                int[,] matrix = new int[originalLength + 1, modifiedLength + 1];
                for (int i = 0; i <= originalLength; i++)
                    matrix[i, 0] = i;
                for (int j = 0; j <= modifiedLength; j++)
                    matrix[0, j] = j;

                for (int i = 1; i <= originalLength; i++)
                {
                    for (int j = 1; j <= modifiedLength; j++)
                    {
                        int cost = modified[j - 1] == original[i - 1] ? 0 : 1;
                        var vals = new int[]
                        {
                            matrix[i - 1, j] + 1,
                            matrix[i, j - 1] + 1,
                            matrix[i - 1, j - 1] + cost
                        };
                        matrix[i, j] = vals.Min();
                        if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                        {
                            matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                        }

                    }
                }
                result = matrix[originalLength, modifiedLength];
            }
            catch (IndexOutOfRangeException ex)
            {
                result = 0;
                Trace.WriteLine(ex);
            }
            catch (Exception ex)
            {
                result = 0;
                Trace.WriteLine(ex);
            }

            return result;
        }
    }
}
