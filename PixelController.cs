using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace imaginator
{
    // this class accumulates image data for pixel-by-pixel  correct processing
    class PixelController
    {
        private Bitmap bm = null;               // image in bitmap
        
        private int pixelWidth, pixelHeight;
        private int[,] R, G, B;                 // channels values wrapper

        public PixelController(string path, int pixelWidth, int pixelHeight)
        {
            this.pixelWidth = pixelWidth;
            this.pixelHeight = pixelHeight;

            // read bitmap image
            BitmapImage bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.UriSource = new Uri(path);
            bmi.DecodePixelWidth = pixelWidth;
            bmi.DecodePixelHeight = pixelHeight;
            bmi.EndInit();

            // get bitmap
            bm = FormatConverter.BitmapFromBitmapImage(bmi);
            BitmapImage bmi2 = FormatConverter.BitmapImageFromBitmap(bm, pixelWidth, pixelHeight);
            // wrapper initializing
            R = new int[pixelWidth, pixelHeight];
            G = new int[pixelWidth, pixelHeight];
            B = new int[pixelWidth, pixelHeight];
            for (int i = 0; i < pixelWidth; i++)
                for (int j = 0; j < pixelHeight; j++)
                {
                    Color c_buf = bm.GetPixel(i, j);
                    R[i, j] = c_buf.R;
                    G[i, j] = c_buf.G;
                    B[i, j] = c_buf.B;
                }
        }

        // get image source for image <=> get BitmapImage
        public BitmapImage GetBitmapImage() {
            return FormatConverter.BitmapImageFromBitmap(bm, pixelWidth, pixelHeight);
        }


    }
}
