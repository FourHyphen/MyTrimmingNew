using System;
using System.Drawing;

namespace MyTrimmingNew
{
    public class DisplayImage
    {
        private System.IO.FileInfo _imageInfo;
        private WindowSize _windowSize;

        /// <summary>
        /// パス指定された画像を開く
        /// </summary>
        /// <param name="imagePath"></param>
        public DisplayImage(String imagePath, int windowWidth, int windowHeight)
        {
            BitmapImage = new Bitmap(imagePath);
            _imageInfo = new System.IO.FileInfo(imagePath);
            _windowSize = new WindowSize(windowWidth, windowHeight, OriginalImageWidth, OriginalImageHeight);
        }

        public Bitmap BitmapImage { get; private set; }

        public int OriginalImageWidth { get { return BitmapImage.Width; } }

        public int OriginalImageHeight { get { return BitmapImage.Height; } }

        /// <summary>
        /// 表示用画像
        /// </summary>
        /// <returns></returns>
        public System.Windows.Media.Imaging.BitmapSource GetImage()
        {
            // 要求タイミングはWindow新規表示 or リサイズ時のみなのでインスタンス保持する必要なし、都度作成する
            Bitmap windowFitImage = common.Image.CreateBitmap(BitmapImage,
                                                              _windowSize.ImageFitWindowWidth,
                                                              _windowSize.ImageFitWindowHeight);
            return common.Image.CreateBitmapSourceImage(windowFitImage);
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

        public bool Save(string filePath, IAuxiliaryLine auxiliaryLine)
        {
            return true;
        }
    }
}
