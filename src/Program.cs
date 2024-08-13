using System;
using System.Collections.Generic;
using Gtk;
using SkiaSharp;

namespace MarkIPerceptronProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Do you want to train a new model? (yes/no)");
            string trainNewModel = Console.ReadLine().ToLower();

            if (trainNewModel == "yes")
            {
                Console.WriteLine("Enter the path to the CSV file for training data:");
                string trainingDataPath = Console.ReadLine();

                // Validate the input CSV file
                if (!File.Exists(trainingDataPath))
                {
                    Console.WriteLine("Training data file not found. Exiting.");
                    return;
                }

                // Load data and train perceptron
                var (trainingInputs, trainingLabels) = DataLoader.LoadData(trainingDataPath);
                if (trainingInputs.Length == 0 || trainingLabels.Length == 0)
                {
                    Console.WriteLine("No valid training data found. Exiting.");
                    return;
                }

                var perceptron = new Perceptron(inputSize: trainingInputs[0].Length);
                var epochErrors = perceptron.Train(trainingInputs, trainingLabels, epochs: 1000);

                // Save the weights
                string weightsFilePath = Path.ChangeExtension(trainingDataPath, ".weights.csv");
                perceptron.SaveWeights(weightsFilePath);
                Console.WriteLine($"Model trained and weights saved to {weightsFilePath}.");

                // Plot the errors
                try
                {
                    Application.Init();
                    GtkSharpPlotter.PlotErrors(epochErrors);
                    Application.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing GTK application: {ex.Message}");
                }
            }
            else if (trainNewModel == "no")
            {
                Console.WriteLine("Enter the path to the CSV file for the model weights:");
                string weightsFilePath = Console.ReadLine();

                // Validate the weights file
                if (!File.Exists(weightsFilePath))
                {
                    Console.WriteLine("Weights file not found. Exiting.");
                    return;
                }

                // Load the perceptron with existing weights
                var perceptron = new Perceptron(inputSize: 13); // assuming 13 input features
                perceptron.LoadWeights(weightsFilePath);

                // Prompt for prediction input
                Console.WriteLine("Enter the input features for prediction, separated by commas:");
                string inputFeatures = Console.ReadLine();
                var sampleInputs = Array.ConvertAll(inputFeatures.Split(','), Double.Parse);

                if (sampleInputs.Length != perceptron.InputSize)
                {
                    Console.WriteLine(
                        $"Expected {perceptron.InputSize} input features but received {sampleInputs.Length}. Exiting."
                    );
                    return;
                }

                // Make and display the prediction
                var predictionResult = perceptron.Predict(sampleInputs);
                Console.WriteLine($"Prediction result: {predictionResult}");
            }
            else
            {
                Console.WriteLine("Invalid option. Exiting.");
            }
        }
    }
}
