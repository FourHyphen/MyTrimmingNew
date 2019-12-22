using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace MyTrimmingNew
{
    public partial class MainWindow : Window
    {
        private ImageController _imageController;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void menuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            string filePath = ImageFileOpenDialog.GetInstance().Show();
            if(filePath != "")
            {
                DisplayImageInfo(filePath);
            }
        }

        private void DisplayImageInfo(string imageFilePath)
        {
            try
            {
                // TODO: 実装をプログラミングしてないか？
                _imageController = new ImageController(imageFilePath,
                                                       (int)Width - Constant.FixCanvasWidth,
                                                       (int)Height - Constant.FixCanvasHeight);
                xShowImage.Source = _imageController.GetImage();
                xOriginalImageLength.Content = _imageController.GetImageSizeString();
            }
            catch (Exception ex)
            {
                // TODO: 画像オープン失敗時の例外処理
            }
        }

        private void mainWindowKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void menuFileSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _imageController.Save(null, xSaveResult);

            }
            catch (Exception ex)
            {
                // TODO: 画像保存失敗時の例外処理
                throw ex;
            }
        }
    }
}
