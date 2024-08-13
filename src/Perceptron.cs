using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarkIPerceptronProject
{
    public class Perceptron
    {
        private double[] weights;
        private double learningRate;

        public Perceptron(int inputSize, double learningRate = 0.0000025)
        {
            this.weights = new double[inputSize + 1]; // +1 for the bias weight
            this.learningRate = learningRate;
            InitializeWeights();
        }

        private void InitializeWeights()
        {
            var random = new Random();
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = random.NextDouble() - 0.5; // Initialize weights to small random values
            }
        }

        public int Predict(double[] inputs)
        {
            double sum = weights[0]; // Bias weight
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * weights[i + 1];
            }
            return ActivationFunction(sum);
        }

        private int ActivationFunction(double sum)
        {
            return sum > 0 ? 1 : -1;
        }

        public List<double> Train(double[][] trainingInputs, int[] trainingLabels, int epochs)
        {
            var epochErrors = new List<double>();

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalError = 0.0;

                for (int i = 0; i < trainingInputs.Length; i++)
                {
                    // Make a prediction
                    int prediction = Predict(trainingInputs[i]);

                    // Calculate the error
                    int error = trainingLabels[i] - prediction;

                    // Update the weights
                    UpdateWeights(trainingInputs[i], error);

                    // Accumulate the squared error for this example
                    totalError += Math.Pow(error, 2);
                }

                // Calculate mean squared error for this epoch and add to the list
                epochErrors.Add(totalError / trainingInputs.Length);
            }

            return epochErrors;
        }

        private void UpdateWeights(double[] inputs, int error)
        {
            weights[0] += learningRate * error; // Update bias weight
            for (int i = 0; i < inputs.Length; i++)
            {
                weights[i + 1] += learningRate * error * inputs[i]; // Update each weight
            }
        }

        public void SaveWeights(string filePath)
        {
            File.WriteAllLines(filePath, weights.Select(w => w.ToString()));
        }

        public void LoadWeights(string filePath)
        {
            if (File.Exists(filePath))
            {
                weights = File.ReadAllLines(filePath).Select(double.Parse).ToArray();
            }
            else
            {
                throw new FileNotFoundException("Weights file not found.");
            }
        }

        public int InputSize
        {
            get { return weights.Length - 1; } // Exclude bias weight
        }
    }
}
