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
        private AuxiliaryController _auxiliaryController;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void menuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            string filePath = ImageFileOpenDialog.GetInstance().Show();
            if(filePath != "")
            {
                DisplayInfo(filePath);
            }
        }

        /// <summary>
        /// 各種データを表示する
        /// 表示順は画像情報 → 補助線情報とすること
        /// </summary>
        /// <param name="filePath"></param>
        private void DisplayInfo(string filePath)
        {
            DisplayImageInfo(filePath);
            DisplayAuxiliaryLine();
        }

        /// <summary>
        /// TODO: 実装をプログラミングしてないか？
        /// </summary>
        /// <param name="imageFilePath"></param>
        private void DisplayImageInfo(string imageFilePath)
        {
            try
            {
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

        /// <summary>
        /// 切り抜き補助矩形初期化
        /// TODO: 比率を16:9固定ではなく4:3とかの任意比率に対応する
        /// TODO: 比率の初期値どうする？config.ini管理？
        /// </summary>
        private void DisplayAuxiliaryLine()
        {
            _auxiliaryController = new AuxiliaryController(_imageController);

            xAuxiliaryLine.Width = _auxiliaryController.AuxiliaryWidth;
            xAuxiliaryLine.Height = _auxiliaryController.AuxiliaryHeight;
            Canvas.SetLeft(xAuxiliaryLine, (double)_auxiliaryController.AuxiliaryLeftRelativeImage);
            Canvas.SetTop(xAuxiliaryLine, (double)_auxiliaryController.AuxiliaryTopRelativeImage);
            xAuxiliaryLineLength.Content = _auxiliaryController.GetLineSizeString();
        }

        private void mainWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.IsKeyCursor(e.Key))
            {
                MoveAuxiliaryLine(Keys.ToEnableKeys(e.Key)); ;
            }
        }

        /// <summary>
        /// 補助線をキー入力内容の通りに移動する
        /// </summary>
        /// <param name="key"></param>
        private void MoveAuxiliaryLine(Keys.EnableKeys key)
        {
            _auxiliaryController.MoveAuxiliaryLine(key);
            Canvas.SetLeft(xAuxiliaryLine, (double)_auxiliaryController.AuxiliaryLeftRelativeImage);
            Canvas.SetTop(xAuxiliaryLine, (double)_auxiliaryController.AuxiliaryTopRelativeImage);
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
