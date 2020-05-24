using System;
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

        public ImageController(String imagePath, int windowWidth, int windowHeight)
        {
            BitmapImage = new Bitmap(imagePath);
            _imageInfo = new System.IO.FileInfo(imagePath);
            _windowSize = new WindowSize(windowWidth, windowHeight, OriginalImageWidth, OriginalImageHeight);
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
            return common.Image.CreateBitmapSourceImage(BitmapImage,
                                                        _windowSize.ImageFitWindowWidth,
                                                        _windowSize.ImageFitWindowHeight);
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
                if (ac.AuxiliaryDegree == 0)
                {
                    common.Image.SaveTrimImage(BitmapImage,
                                               filePath,
                                               ac.AuxiliaryLeft,
                                               ac.AuxiliaryTop,
                                               ac.AuxiliaryRight,
                                               ac.AuxiliaryBottom,
                                               _windowSize.ImageFitWindowRatio);
                }
                else
                {
                    common.Image.SaveTrimImage(BitmapImage,
                                               filePath,
                                               ac.AuxiliaryLeftTop,
                                               ac.AuxiliaryLeftBottom,
                                               ac.AuxiliaryRightTop,
                                               ac.AuxiliaryRightBottom,
                                               _windowSize.ImageFitWindowRatio,
                                               ac.AuxiliaryDegree);
                }

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
