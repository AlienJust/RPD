using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Abt.Controls.SciChart.Visuals;

namespace RPD.SciChartControl
{
    internal  static class SciChartSurfaceExtensions
    {
        public static void ExportBitmapToClipboard(this SciChartSurface element)
        {
            var rtb = ExportToBitmap(element);
            Clipboard.SetImage(rtb);
        }

        public static void ExportBitmapToFile(this SciChartSurface element, string filename)
        {
            using (var filestream = new FileStream(filename, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                var bitmap = ExportToBitmap(element);
                
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(filestream);
            }
        }

        public static BitmapSource ExportToBitmap(this SciChartSurface element)
        {
            if (element == null)
                return null;

            // Store the Frameworks current layout transform, as this will be restored later
            var storedTransform = element.LayoutTransform;

            // Set the layout transform to unity to get the nominal width and height
            element.LayoutTransform = new ScaleTransform(1, 1);

            // если раскоментировать, то график будет смещатьсяи и искажаться
            //element.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            //element.Arrange(new Rect(new Point(0, 0), element.DesiredSize));


            var height = element.ActualHeight + element.Margin.Top + element.Margin.Bottom;
            var width = element.ActualWidth + element.Margin.Left + element.Margin.Right;

            // Render to a Bitmap Source, note that the DPI is
            // changed for the render target as a way of scaling the FrameworkElement
            var rtb = new RenderTargetBitmap(
                   (int)width,
                   (int)height,
                   96d,
                   96d,
                   PixelFormats.Default);

            // Render a white background in Clipboard
            var vRect = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = Brushes.White
            };
            vRect.Measure(element.RenderSize);
            vRect.Arrange(new Rect(element.RenderSize));
            rtb.Render(vRect);
            rtb.Render(element);

            /*
             * // если раскоментировать, то график будет смещатьсяи и искажаться
            // Restore the Framework Element to it’s previous state
            element.LayoutTransform = storedTransform;
            element.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            element.Arrange(new Rect(new Point(0, 0), element.DesiredSize));*/

            return rtb;
        }
    }
}
