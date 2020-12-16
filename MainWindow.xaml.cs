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
        WriteableBitmap w_wbm = null;    // working writeable bitmap
        WriteableBitmap bh_wbm = null;   // brightness histogram bitmap

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog reply = FileDialogHandler();
            if (reply != null)
            {
                BitmapImage bm = new BitmapImage();
                bm.BeginInit();
                bm.UriSource = new Uri(reply.FileName);
                bm.DecodePixelWidth = (int)ImageArea.Width;
                bm.DecodePixelHeight = (int)ImageArea.Height;
                bm.EndInit();
                w_wbm = new WriteableBitmap(bm);
                ImageArea.Source = w_wbm;
                // drawing brightness histogram
                BrightnessHistogramСalculator bhc = new BrightnessHistogramСalculator(w_wbm);
                bh_wbm = bhc.CalculateHistogram((int)HistogramArea.Width, (int)HistogramArea.Height);
                HistogramArea.Source = bh_wbm;
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
