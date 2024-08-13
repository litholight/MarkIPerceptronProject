using System.Collections.Generic;
using System.Linq;
using Gtk;
using SkiaSharp;

namespace MarkIPerceptronProject
{
    public class SkiaPlotView : DrawingArea
    {
        private readonly List<double> epochErrors;

        public SkiaPlotView(List<double> errors)
        {
            epochErrors = errors;
            this.Drawn += OnDrawn;
        }

        private void OnDrawn(object sender, DrawnArgs args)
        {
            var context = args.Cr;
            var width = this.Allocation.Width;
            var height = this.Allocation.Height;

            using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
            {
                DrawPlot(surface.Canvas);
                using (var image = surface.Snapshot())
                using (var data = image.Encode())
                {
                    using (var pixbuf = new Gdk.Pixbuf(data.ToArray()))
                    {
                        Gdk.CairoHelper.SetSourcePixbuf(context, pixbuf, 0, 0);
                        context.Paint();
                    }
                }
            }
        }

        private void DrawPlot(SKCanvas canvas)
        {
            canvas.Clear(SKColors.White);

            var paint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 2,
                IsAntialias = true
            };

            var labelPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 20,
                IsAntialias = true
            };

            float width = this.Allocation.Width;
            float height = this.Allocation.Height;
            float margin = 50;
            float plotWidth = width - 2 * margin;
            float plotHeight = height - 2 * margin;

            // Normalize error values to start from zero
            double minError = epochErrors.Min();
            var normalizedErrors = epochErrors.Select(e => e - minError).ToList();
            double maxError = normalizedErrors.Max();
            double errorRange = maxError;

            // Draw axes
            canvas.DrawLine(margin, margin, margin, height - margin, paint);
            canvas.DrawLine(margin, height - margin, width - margin, height - margin, paint);

            // Draw axis labels
            canvas.DrawText("Epochs", width / 2, height - 10, labelPaint);
            canvas.RotateDegrees(-90);
            canvas.DrawText("Normalized Error", -height / 2, 20, labelPaint);
            canvas.RotateDegrees(90);

            // Plot the errors
            if (normalizedErrors.Count > 1)
            {
                for (int i = 1; i < normalizedErrors.Count; i++)
                {
                    float x1 = margin + plotWidth * (i - 1) / (normalizedErrors.Count - 1);
                    float y1 =
                        height
                        - margin
                        - plotHeight * ((float)normalizedErrors[i - 1] / (float)errorRange);
                    float x2 = margin + plotWidth * i / (normalizedErrors.Count - 1);
                    float y2 =
                        height
                        - margin
                        - plotHeight * ((float)normalizedErrors[i] / (float)errorRange);

                    canvas.DrawLine(x1, y1, x2, y2, paint);
                }

                // Draw ticks and labels
                for (int i = 0; i <= 10; i++)
                {
                    float x = margin + plotWidth * i / 10;
                    float y = height - margin + 10;
                    canvas.DrawLine(x, height - margin, x, y, paint);
                    canvas.DrawText(
                        (normalizedErrors.Count * i / 10).ToString(),
                        x - 10,
                        y + 20,
                        labelPaint
                    );
                }

                for (int i = 0; i <= 10; i++)
                {
                    float x = margin - 10;
                    float y = height - margin - plotHeight * i / 10;
                    canvas.DrawLine(margin, y, x, y, paint);
                    canvas.DrawText(
                        (maxError * i / 10).ToString("0.00"),
                        x - 40,
                        y + 5,
                        labelPaint
                    );
                }
            }
        }
    }
}
