﻿using Microsoft.Win32;
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
using MyTrimmingNew.AuxiliaryLine;

namespace MyTrimmingNew
{
    public partial class MainWindow : Window
    {
        private ImageController _imageController;
        private ShowImageObserver _showImageObserver;
        private ShowImageLengthObserver _showImageLength;

        private AuxiliaryController _auxiliaryController;
        private AuxiliaryLineRectangleObserver _auxiliaryLineRectangle;
        private AuxiliaryLineLengthObserver _auxiliaryLineLength;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region "ユーザー操作時処理: 画像ファイルオープン"

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
        /// </summary>
        /// <param name="filePath"></param>
        private void DisplayInfo(string filePath)
        {
            DisplayImageInfo(filePath);
            DisplayAuxiliaryLine(_imageController);
        }

        private void DisplayImageInfo(string imageFilePath)
        {
            try
            {
                _imageController = new ImageController(imageFilePath,
                                                       (int)Width - Constant.FixCanvasWidth,
                                                       (int)Height - Constant.FixCanvasHeight);
                _showImageObserver = new ShowImageObserver(xShowImage, _imageController);
                _showImageLength = new ShowImageLengthObserver(xOriginalImageLength, _imageController);
            }
            catch (Exception ex)
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
            if (Keys.IsKeyCursor(e.Key))
            {
                SetAuxiliaryLineEvent();
                PublishAuxiliaryLineEvent(Keys.ToEnableKeys(e.Key));
            }
        }

        #endregion

        #region "ユーザー操作時処理: マウス操作時の処理"

        private void xShowImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            // MouseUp時に発行するイベントをセット
            Point coordinateRelatedAuxiliaryLine = e.GetPosition(xAuxiliaryLine);
            SetAuxiliaryLineEvent(coordinateRelatedAuxiliaryLine);
        }

        private void xShowImageMouseUp(object sender, MouseButtonEventArgs e)
        {
            Point coordinateRelatedAuxiliaryLine = e.GetPosition(xAuxiliaryLine);
            PublishAuxiliaryLineEvent(coordinateRelatedAuxiliaryLine);
        }

        #endregion

        #region "ユーザー操作時処理: 画像ファイル保存"

        private void menuFileSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _imageController.Save(_auxiliaryController);
                ImageProcessResultMessageBox.Show(ImageProcessResultMessageBox.Result.SuccessImageSave);
            }
            catch
            {
                ImageProcessResultMessageBox.Show(ImageProcessResultMessageBox.Result.FailureImageSave);
            }
        }

        #endregion

        #region "内部処理: 補助線操作処理"

        private void SetAuxiliaryLineEvent()
        {
            _auxiliaryController.SetEvent();
        }

        private void SetAuxiliaryLineEvent(Point mouseDownCoordinate)
        {
            _auxiliaryController.SetEvent(mouseDownCoordinate);
        }

        private void PublishAuxiliaryLineEvent(object operation)
        {
            _auxiliaryController.PublishEvent(operation);
        }

        #endregion
    }
}
