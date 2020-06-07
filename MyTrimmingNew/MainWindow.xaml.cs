using System;
using System.Windows;
using System.Windows.Input;
using MyTrimmingNew.AuxiliaryLine;
using MyTrimmingNew.common;

namespace MyTrimmingNew
{
    public partial class MainWindow : Window
    {
        private ImageController _imageController;
        private ShowSupportMessageObserver _showImageMessage;
        private ShowImageObserver _showImageObserver;
        private ShowImageLengthObserver _showImageLength;

        private AuxiliaryController _auxiliaryController;
        private AuxiliaryLineRectangleObserver _auxiliaryLineRectangle;
        private AuxiliaryLineLengthObserver _auxiliaryLineLength;

        // TODO: オプションか外部ファイルか何かで可変にする
        private int _rotateByKey = 1;
        private int _rotateByKeyWithCtrl = 10;

        public MainWindow()
        {
            InitializeComponent();
            _showImageMessage = new ShowSupportMessageObserver(xSupportMessage);
        }

        #region "ユーザー操作時処理: 画像ファイルオープン"

        private void menuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenImage();
        }

        private void OpenImage()
        {
            string filePath = ImageFileOpenDialog.GetInstance().Show();
            if (filePath != "")
            {
                DisplayInfo(filePath);
            }
        }

        /// <summary>
        /// 各種データを表示する
        /// </summary>
        /// <param name="filePath"></param>
        private void DisplayInfo(string filePath)
        {
            DisplayImageInfo(filePath);
            DisplayAuxiliaryLine(_imageController);
        }

        private void DisplayImageInfo(string imageFilePath)
        {
            if (_imageController != null)
            {
                _imageController.Dispose();
            }

            try
            {
                _imageController = new ImageController(imageFilePath,
                                                       (int)Width - Constant.FixCanvasWidth,
                                                       (int)Height - Constant.FixCanvasHeight);
                _showImageMessage.Attach(_imageController);
                _showImageObserver = new ShowImageObserver(xShowImage, _imageController);
                _showImageLength = new ShowImageLengthObserver(xOriginalImageLength, _imageController);
            }
            catch
            {
                ImageProcessResultMessageBox.Show(ImageProcessResultMessageBox.Result.FailureImageOpen);
            }
        }

        /// <summary>
        /// TODO: 比率の初期値どうする？config.ini管理？
        /// </summary>
        private void DisplayAuxiliaryLine(ImageController ic, AuxiliaryLineParameter.RatioType ratioType = AuxiliaryLineParameter.RatioType.W16H9)
        {
            _auxiliaryController = new AuxiliaryController(ic, ratioType:ratioType);
            _auxiliaryLineRectangle = new AuxiliaryLineRectangleObserver(xAuxiliaryLine, _auxiliaryController);
            _auxiliaryLineLength = new AuxiliaryLineLengthObserver(xAuxiliaryLineLength, _auxiliaryController);
        }

        #endregion

        #region "ユーザー操作時処理: 設定変更"

        private void menuSettingsRatioW16H9_Click(object sender, RoutedEventArgs e)
        {
            DisplayAuxiliaryLine(_imageController, ratioType: AuxiliaryLineParameter.RatioType.W16H9);
        }

        private void menuSettingsRatioW4H3_Click(object sender, RoutedEventArgs e)
        {
            DisplayAuxiliaryLine(_imageController, ratioType: AuxiliaryLineParameter.RatioType.W4H3);
        }

        private void menuSettingsRatioW9H16_Click(object sender, RoutedEventArgs e)
        {
            DisplayAuxiliaryLine(_imageController, ratioType: AuxiliaryLineParameter.RatioType.W9H16);
        }

        private void menuSettingsRatioNoDefined_Click(object sender, RoutedEventArgs e)
        {
            DisplayAuxiliaryLine(_imageController, ratioType: AuxiliaryLineParameter.RatioType.NoDefined);
        }

        #endregion

        #region "ユーザー操作時処理: キー操作時の処理"

        private void mainWindowKeyDown(object sender, KeyEventArgs e)
        {
            Keys.EnableKeys keyKind = Keys.ToEnableKeys(e.Key, e.KeyboardDevice);

            if (Keys.IsKeyCursor(keyKind))
            {
                SetAuxiliaryLineEvent();
                PublishAuxiliaryLineEvent(keyKind);
            }
            else if (keyKind == Keys.EnableKeys.RotatePlus)
            {
                RotateAuxiliaryLine(_rotateByKey);
            }
            else if (keyKind == Keys.EnableKeys.RotateMinus)
            {
                RotateAuxiliaryLine(-_rotateByKey);
            }
            else if (keyKind == Keys.EnableKeys.RotatePlusWithCtrl)
            {
                RotateAuxiliaryLine(_rotateByKeyWithCtrl);
            }
            else if (keyKind == Keys.EnableKeys.RotateMinusWithCtrl)
            {
                RotateAuxiliaryLine(-_rotateByKeyWithCtrl);
            }
            else if (keyKind == Keys.EnableKeys.Cancel)
            {
                CancelAuxiliaryLineEvent();
            }
            else if (keyKind == Keys.EnableKeys.Redo)
            {
                RedoAuxiliaryLineEvent();
            }
            else if (keyKind == Keys.EnableKeys.FileOpen)
            {
                OpenImage();
            }
            else if (keyKind == Keys.EnableKeys.FileSave)
            {
                SaveImage();
            }
            else if (keyKind == Keys.EnableKeys.PreviewWindowOpen)
            {
                OpenPreviewWindow();
            }
        }

        #endregion

        #region "ユーザー操作時処理: マウス操作時の処理"

        private void xShowImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            // MouseUp時に発行するイベントをセット
            SetAuxiliaryLineEvent(e.GetPosition(xAuxiliaryLine));
        }

        private void xShowImageMouseUp(object sender, MouseButtonEventArgs e)
        {
            PublishAuxiliaryLineEvent(e.GetPosition(xAuxiliaryLine));
        }

        private Point GetMousePositionRelativeAuxiliaryLine(Point mousePoint)
        {
            int x = (int)mousePoint.X - _auxiliaryController.AuxiliaryLeft;
            int y = (int)mousePoint.Y - _auxiliaryController.AuxiliaryTop;
            return new Point(x, y);
        }

        #endregion

        #region ユーザー操作時処理: 回転時の処理

        private void MenuEditRotatePlus10_Click(object sender, RoutedEventArgs e)
        {
            RotateAuxiliaryLine(10);
        }

        private void MenuEditRotateMinus10_Click(object sender, RoutedEventArgs e)
        {
            RotateAuxiliaryLine(-10);
        }

        private void RotateAuxiliaryLine(int degree)
        {
            SetAuxiliaryLineEvent(degree);
            PublishAuxiliaryLineEvent();
        }

        #endregion

        #region "ユーザー操作時処理: 画像ファイル保存"

        private void menuFileSave_Click(object sender, RoutedEventArgs e)
        {
            SaveImage();
        }

        private void SaveImage()
        {
            if (_imageController == null)
            {
                return;
            }

            ImageController.SaveResult result = _imageController.Save(_auxiliaryController);
            if (result == ImageController.SaveResult.Success)
            {
                ImageProcessResultMessageBox.Show(ImageProcessResultMessageBox.Result.SuccessImageSave);
            }
            else if (result == ImageController.SaveResult.Failure)
            {
                ImageProcessResultMessageBox.Show(ImageProcessResultMessageBox.Result.FailureImageSave);
            }
        }

        #endregion

        #region "ユーザー操作時処理: プレビューWindow"

        private void menuPreviewWindowOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenPreviewWindow();
        }

        private void OpenPreviewWindow()
        {
            if (_imageController == null)
            {
                return;
            }

            Window preview = new Preview(_imageController, _auxiliaryController);
            preview.Show();
        }

        #endregion

        #region "内部処理: 補助線操作処理"

        private void SetAuxiliaryLineEvent()
        {
            if (_auxiliaryController == null)
            {
                return;
            }
            _auxiliaryController.SetEvent();
        }

        private void SetAuxiliaryLineEvent(int degree)
        {
            if (_auxiliaryController == null)
            {
                return;
            }
            _auxiliaryController.SetEvent(degree);
        }

        private void SetAuxiliaryLineEvent(Point mouseDownCoordinate)
        {
            if (_auxiliaryController == null)
            {
                return;
            }
            _auxiliaryController.SetEvent(mouseDownCoordinate);
        }

        private void PublishAuxiliaryLineEvent(object operation)
        {
            if (_auxiliaryController == null)
            {
                return;
            }
            _auxiliaryController.PublishEvent(operation);
        }

        private void PublishAuxiliaryLineEvent()
        {
            if (_auxiliaryController == null)
            {
                return;
            }
            _auxiliaryController.PublishEvent(null);
        }

        private void CancelAuxiliaryLineEvent()
        {
            if (_auxiliaryController == null)
            {
                return;
            }
            _auxiliaryController.CancelEvent();
        }

        private void RedoAuxiliaryLineEvent()
        {
            if (_auxiliaryController == null)
            {
                return;
            }
            _auxiliaryController.RedoEvent();
        }

        #endregion
    }
}
