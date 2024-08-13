using System;
using System.Drawing;

namespace MarkIPerceptronProject
{
    public class ImageGenerator
    {
        public static Bitmap GenerateRandomImage(int width, int height)
        {
            var random = new Random();
            var bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                    bitmap.SetPixel(x, y, randomColor);
                }
            }

            return bitmap;
        }

        public static double[] ExtractFeatures(Bitmap image)
        {
            var features = new double[image.Width * image.Height * 3]; // RGB values
            int index = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    features[index++] = pixel.R;
                    features[index++] = pixel.G;
                    features[index++] = pixel.B;
                }
            }

            return features;
        }
    }
}
