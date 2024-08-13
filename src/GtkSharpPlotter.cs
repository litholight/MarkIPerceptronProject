using System.Collections.Generic;
using Gtk;

namespace MarkIPerceptronProject
{
    public class GtkSharpPlotter
    {
        public static void PlotErrors(List<double> epochErrors)
        {
            Application.Init();

            var window = new Window("Training Error Plot")
            {
                DefaultWidth = 800,
                DefaultHeight = 600
            };

            var plotView = new SkiaPlotView(epochErrors);
            window.Add(plotView);

            window.ShowAll();

            window.DeleteEvent += (sender, e) =>
            {
                Application.Quit();
            };

            Application.Run();
        }
    }
}
