using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace imaginator
{
    // this class accumulates image data for pixel-by-pixel  correct processing
    class PixelController
    {
        private int pixelWidth, pixelHeight;
        private int[,] R, G, B;                 // channels values wrapper

        // streams for bitmap and bitmap image

        private MemoryStream bmpStream = null;  // bitmap stream
        private MemoryStream bmpiStream = null; // bitmap image stream
        public PixelController(string path, int pixelWidth, int pixelHeight)
        {
            this.pixelWidth = pixelWidth;
            this.pixelHeight = pixelHeight;

            // streams initializing
            bmpStream = new MemoryStream();
            bmpiStream = new MemoryStream();

            // read bitmap image
            BitmapImage bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.UriSource = new Uri(path);
            bmi.DecodePixelWidth = pixelWidth;
            bmi.DecodePixelHeight = pixelHeight;
            bmi.EndInit();

            // get bitmap
            RewriteBitmapStream(bmi); // write bitmap stream
            Bitmap bm = GetBitmapFromStream();

            // get bitmapimage
            RewriteBitmapImageStream(bm);

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

        //  the conversion of brightness
        public void BConversion(int rValue, int gValue, int bValue) //
        {
            Bitmap bm = GetBitmapFromStream();  // the converted image
            for (int i = 0; i < pixelWidth; i++)
                for (int j = 0; j < pixelHeight; j++)
                {
                    Color newColor = Color.FromArgb(getNewBConversionValue(R[i, j], rValue)
                                                    , getNewBConversionValue(G[i, j], gValue),
                                                    getNewBConversionValue(B[i, j], bValue));
                    bm.SetPixel(i, j, newColor);
                }
            // update streams
            RewriteBitmapImageStream(bm);
            RewriteBitmapStream(GetBitmapImageFromStream());
        }
        private int getNewBConversionValue(int oldValue, int d)
        {
            int newValue = oldValue + d;
            if (newValue > 255)
                return 255;
            else if (newValue < 0)
                return 0;
            else
                return newValue;
        }
        // convert methods
        private void RewriteBitmapStream(BitmapImage srs)
        {
            bmpStream.Position = 0;
            Bitmap bmp;
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(srs));
            enc.Save(bmpStream);  
        }

        public Bitmap GetBitmapFromStream()
        {
            bmpStream.Position = 0;
            return new Bitmap(bmpStream);
        }

        private void RewriteBitmapImageStream(Bitmap bitmap)
        {
            bmpiStream.Position = 0;
            bitmap.Save(bmpiStream, ImageFormat.Png);
              
        }

        public BitmapImage GetBitmapImageFromStream()
        {
            bmpiStream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = bmpiStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.DecodePixelWidth = pixelWidth;
            bitmapImage.DecodePixelHeight = pixelHeight;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
