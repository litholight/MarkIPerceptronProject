using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarkIPerceptronProject
{
    public class DataLoader
    {
        public static (double[][] inputs, int[] labels) LoadData(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Skip(1); // Skip the header
            var inputs = new List<double[]>();
            var labels = new List<int>();

            foreach (var line in lines)
            {
                try
                {
                    // Check if the line contains a '?'
                    if (line.Contains('?'))
                    {
                        Console.WriteLine($"Skipping line due to missing value: {line}");
                        continue;
                    }

                    var values = line.Split(',').Select(double.Parse).ToArray();
                    var input = values.Take(values.Length - 1).ToArray(); // All but the last column
                    var label = (int)values.Last() == 0 ? -1 : 1; // Convert 0 to -1 for the labels
                    inputs.Add(input);
                    labels.Add(label);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing line: {line}");
                    Console.WriteLine(ex.Message);
                }
            }

            return (inputs.ToArray(), labels.ToArray());
        }
    }
}
