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
        private WriteableBitmap srs;
        private int width;
        private int height;
        public BrightnessHistogramСalculator(WriteableBitmap srs)
        {
            this.srs = srs;
            // standart width and height for histogram
            width = 256;
            height = 300;
        }

        public WriteableBitmap CalculateHistogram(int width, int height)
        {
            Bitmap bmp = BitmapFromWriteableBitmap(); 
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


            return new WriteableBitmap(BitmapImageFromBitmap(barChart, width, height));
        }

        // internal converter
        private Bitmap BitmapFromWriteableBitmap()
        {
            Bitmap bmp;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create((BitmapSource)srs));
                enc.Save(outStream);
                bmp = new Bitmap(outStream);
            }
            return bmp;
        }

        private BitmapImage BitmapImageFromBitmap(Bitmap bitmap, int width, int height)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.DecodePixelWidth = width;
                bitmapImage.DecodePixelHeight = height;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
