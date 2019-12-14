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
        private Image _image;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void menuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            ImageOpenDirector iod = new ImageOpenDirector();
            try
            {
                _image = iod.ImageOpen(xShowImage,
                                       xAuxiliaryLine,
                                       xOriginalImageLength,
                                       (int)Width - Constant.FixCanvasWidth,
                                       (int)Height - Constant.FixCanvasHeight);
            }
            catch(Exception ex)
            {
                // TODO: 画像オープン失敗時の例外処理
            }
        }

        private void mainWindowKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void menuFileSave_Click(object sender, RoutedEventArgs e)
        {
            ImageSaveDirector isd = new ImageSaveDirector();
            try
            {
                isd.ImageSave(_image,
                              null,
                              xSaveResult);

            }
            catch (Exception ex)
            {
                // TODO: 画像保存失敗時の例外処理
                throw ex;
            }
        }
    }
}
