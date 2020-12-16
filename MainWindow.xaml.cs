using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace imaginator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // fields
        WriteableBitmap wbm = null; // working bitmap
        BitmapImage src = new BitmapImage();
        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            /*/ Create the bitmap, with the dimensions of the bm placeholder.
            WriteableBitmap wb = new WriteableBitmap((int)ImageArea.Width,
                (int)ImageArea.Height, 96, 96, PixelFormats.Bgra32, null);
            
            // Define the update square (which is as big as the entire bm).
            Int32Rect rect = new Int32Rect(0, 0, (int)ImageArea.Width, (int)ImageArea.Height);

            byte[] pixels = new byte[(int)ImageArea.Width * (int)ImageArea.Height * wb.Format.BitsPerPixel / 8];
            Random rand = new Random();
            for (int y = 0; y < wb.PixelHeight; y++) 
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    int alpha = 0;
                    int red = 0;
                    int green = 0;
                    int blue = 0;

                    // Determine the pixel's color.
                    if ((x % 5 == 0) || (y % 7 == 0))
                    {
                        red = (int)((double)y / wb.PixelHeight * 255);
                        green = rand.Next(100, 255);
                        blue = (int)((double)x / wb.PixelWidth * 255);
                        alpha = 255;
                    }
                    else
                    {
                        red = (int)((double)x / wb.PixelWidth * 255);
                        green = rand.Next(100, 255);
                        blue = (int)((double)y / wb.PixelHeight * 255);
                        alpha = 50;
                    }

                    int pixelOffset = (x + y * wb.PixelWidth) * wb.Format.BitsPerPixel/8;
                    pixels[pixelOffset] = (byte)blue;
                    pixels[pixelOffset + 1] = (byte)green;
                    pixels[pixelOffset + 2] = (byte)red;
                    pixels[pixelOffset + 3] = (byte)alpha;

                                       
                }

                int stride = (wb.PixelWidth * wb.Format.BitsPerPixel) / 8;

                wb.WritePixels(rect, pixels, stride, 0);
            }

            // Show the bitmap in an Image element.
            /*/
            OpenFileDialog reply = FileDialogHandler();
            if (reply != null)
            {
                BitmapImage bm = new BitmapImage();
                bm.BeginInit();
                bm.UriSource = new Uri(reply.FileName);
                bm.DecodePixelWidth = (int)ImageArea.Width;
                bm.DecodePixelHeight = (int)ImageArea.Height;
                bm.EndInit();
                wbm = new WriteableBitmap(bm);
                ImageArea.Source = wbm;
            } // add else branch with message box
        }
        
        private OpenFileDialog FileDialogHandler()
        {
            OpenFileDialog loadObjectFileDialog = new OpenFileDialog();
            loadObjectFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            loadObjectFileDialog.Filter = "all|*.*";
            loadObjectFileDialog.ShowDialog();
            if (loadObjectFileDialog.FileName == "")
                return null;
            return loadObjectFileDialog;
        }
    }
}
