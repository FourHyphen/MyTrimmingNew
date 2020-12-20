﻿using System;
using System.Drawing;

namespace MyTrimmingNew
{
    public class ImageController : Subject, IDisposable
    {
        public enum SaveResult
        {
            Success,
            Failure,
            Cancel
        }

        private System.IO.FileInfo _imageInfo;
        private WindowSize _windowSize;

        public double ImageFitRatio { get { return _windowSize.ImageFitWindowRatio; } }

        public ImageController(String imagePath, int windowWidth, int windowHeight)
        {
            BitmapImage = new Bitmap(imagePath);
            _imageInfo = new System.IO.FileInfo(imagePath);
            _windowSize = new WindowSize(windowWidth, windowHeight, OriginalImageWidth, OriginalImageHeight);
        }

        ~ImageController()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (BitmapImage != null)
            {
                BitmapImage.Dispose();
            }
        }

        public Bitmap BitmapImage { get; private set; }

        public int OriginalImageWidth { get { return BitmapImage.Width; } }

        public int OriginalImageHeight { get { return BitmapImage.Height; } }

        public int DisplayImageWidth { get { return _windowSize.ImageFitWindowWidth; } }

        public int DisplayImageHeight { get { return _windowSize.ImageFitWindowHeight; } }

        /// <summary>
        /// 表示用画像
        /// </summary>
        /// <returns></returns>
        public System.Windows.Media.Imaging.BitmapSource GetImage()
        {
            // 要求タイミングはWindow新規表示 or リサイズ時のみなのでインスタンス保持する必要なし、都度作成する
            return GetImage(BitmapImage, _windowSize.ImageFitWindowWidth, _windowSize.ImageFitWindowHeight);
        }

        public System.Windows.Media.Imaging.BitmapSource GetTrimImage(AuxiliaryController ac, int windowWidth)
        {
            int width = windowWidth;
            int height = (int)((double)width / ac.AuxiliaryRatio);
            return GetImage(Trim(ac), width, height);
        }

        /// <summary>
        /// 表示用画像
        /// </summary>
        /// <returns></returns>
        public System.Windows.Media.Imaging.BitmapSource GetImage(Bitmap bitmap, int width, int height)
        {
            return common.Image.CreateBitmapSourceImage(bitmap, width, height);
        }

        public string SaveNameExample
        {
            get
            {
                string ext = _imageInfo.Extension;
                string example = _imageInfo.Name.Replace(ext, "");
                return example + "_resize" + ext;
            }
        }

        public string DirPath
        {
            get { return _imageInfo.DirectoryName; }
        }

        public int LeftOriginalScale(AuxiliaryController ac)
        {
            return (int)((double)ac.AuxiliaryLeft / ImageFitRatio);
        }

        public int RightOriginalScale(AuxiliaryController ac)
        {
            return (int)((double)ac.AuxiliaryRight / ImageFitRatio);
        }

        public int TopOriginalScale(AuxiliaryController ac)
        {
            return (int)((double)ac.AuxiliaryTop / ImageFitRatio);
        }

        public int BottomOriginalScale(AuxiliaryController ac)
        {
            return (int)((double)ac.AuxiliaryBottom / ImageFitRatio);
        }

        // 1/1スケール画像上の切り抜き範囲: 横の長さ
        public int TrimmingWidth(AuxiliaryController ac)
        {
            return (RightOriginalScale(ac) - LeftOriginalScale(ac));
        }

        // 1/1スケール画像上の切り抜き範囲: 縦の長さ
        public int TrimmingHeight(AuxiliaryController ac)
        {
            return (BottomOriginalScale(ac) - TopOriginalScale(ac));
        }

        /// <summary>
        /// 画像をTrimする
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        public Bitmap Trim(AuxiliaryController ac)
        {
            Bitmap bitmap = null;
            if (ac.AuxiliaryDegree == 0)
            {
                bitmap = common.Image.CreateTrimImage(BitmapImage,
                                                      LeftOriginalScale(ac),
                                                      TopOriginalScale(ac),
                                                      TrimmingWidth(ac),
                                                      TrimmingHeight(ac));
            }
            else
            {
                bitmap = common.Image.CreateTrimImage(BitmapImage,
                                                      PointRatio(ac.AuxiliaryLeftTop, ImageFitRatio),
                                                      PointRatio(ac.AuxiliaryLeftBottom, ImageFitRatio),
                                                      PointRatio(ac.AuxiliaryRightTop, ImageFitRatio),
                                                      PointRatio(ac.AuxiliaryRightBottom, ImageFitRatio),
                                                      ac.AuxiliaryDegree);
            }

            return bitmap;
        }

        private Point PointRatio(Point p, double ratio)
        {
            // 1/1スケール画像上のパラメーターに変換
            return new Point((int)(p.X / ratio), (int)(p.Y / ratio));
        }

        /// <summary>
        /// 画像を保存する一連の処理を実行する
        /// </summary>
        public SaveResult Save(AuxiliaryController ac)
        {
            string filePath = ImageFileSaveDialog.GetInstance(SaveNameExample, DirPath).Show();
            if (filePath == "")
            {
                return SaveResult.Cancel;
            }

            SaveResult result;
            try
            {
                Bitmap bitmap = Trim(ac);
                bitmap.Save(filePath);
                result = SaveResult.Success;
            }
            catch
            {
                result = SaveResult.Failure;
            }

            Notify();
            return result;
        }
    }
}
