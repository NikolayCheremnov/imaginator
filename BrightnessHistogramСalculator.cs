using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace imaginator
{
    class BrightnessHistogramСalculator
    {
        private BitmapImage srs;
 
        public BrightnessHistogramСalculator(BitmapImage srs)
        {
            this.srs = srs;
        }

        public BitmapImage CalculateHistogram(int width, int height)
        {
            Bitmap bmp = FormatConverter.BitmapFromBitmapImage(srs); 
            // array data
            int[] Y = new int[256];

            // to collect statistics
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    System.Drawing.Color color = bmp.GetPixel(i, j);
                    int brightness = (int)(0.299 * color.R + 0.5876 * color.G + 0.114 * color.B);
                    Y[brightness]++;         
                }

            // scaling
            int max = 0;
            for (int i = 0; i < 256; i++)
                if (Y[i] > max)
                    max = Y[i];

            // drawing wia scaling
            Bitmap barChart = new Bitmap(256, max);
            // do white background
            using (Graphics graph = Graphics.FromImage(barChart))
            {
                Rectangle ImageSize = new Rectangle(0, 0, width, height);
                graph.FillRectangle(System.Drawing.Brushes.White, ImageSize);
            }
            for (int i = 0; i < 256; i++)
                for (int j = max-1; j > max - Y[i]; j--)
                    barChart.SetPixel(i, j, System.Drawing.Color.Black);


            return FormatConverter.BitmapImageFromBitmap(barChart, width, height);
        }
    }
}
