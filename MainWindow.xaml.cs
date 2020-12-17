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
        PixelController controller = null;
        BitmapImage bh_bm = null;   // brightness histogram bitmap

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog reply = FileDialogHandler();
            if (reply != null)
            {
                controller = new PixelController(reply.FileName, (int)ImageArea.Width, (int)ImageArea.Height);
                ImageRedrawing();
            } // add else branch with message box
        }
        
        private void ImageRedrawing()
        {
            ImageArea.Source = controller.GetBitmapImageFromStream();
            // drawing brightness histogram
            BrightnessHistogramСalculator bhc = new BrightnessHistogramСalculator(controller.GetBitmapImageFromStream());
            bh_bm = bhc.CalculateHistogram((int)HistogramArea.Width, (int)HistogramArea.Height);
            HistogramArea.Source = bh_bm;
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

        private void bConversion_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            controller.BConversion((int)rbConversion.Value, (int)gbConversion.Value, (int)bbConversion.Value);
            ImageRedrawing();
        }

        private void cSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double rValue = Convert.ToDouble(rcConversion.Text);
                double gValue = Convert.ToDouble(gcConversion.Text);
                double bValue = Convert.ToDouble(bcConversion.Text);
                controller.CConversion(rValue, gValue, bValue);
                ImageRedrawing();

            } catch (Exception ex)
            {
                MessageBox.Show("Incorrent data");
            }
        }

        private void makeBinary_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
