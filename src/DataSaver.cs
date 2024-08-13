using System;
using System.IO;

namespace SyntheticDataProject
{
    public class DataSaver
    {
        public static void SaveData(string filePath, double[] features, int label)
        {
            var line = string.Join(",", features) + "," + label;
            File.AppendAllText(filePath, line + Environment.NewLine);
        }
    }
}
