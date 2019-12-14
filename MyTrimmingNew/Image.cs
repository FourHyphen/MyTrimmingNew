using System;
using System.Drawing;

namespace MyTrimmingNew
{
    class Image
    {
        private System.IO.FileInfo _imageInfo;

        /// <summary>
        /// パス指定された画像を開く
        /// </summary>
        /// <param name="imagePath"></param>
        public Image(String imagePath)
        {
            BitmapImage = new Bitmap(imagePath);
            _imageInfo = new System.IO.FileInfo(imagePath);
        }

        /// <summary>
        /// Bitmap画像
        /// </summary>
        /// <returns></returns>
        public Bitmap BitmapImage { get; private set; }

        /// <summary>
        /// 画像の横幅
        /// </summary>
        /// <returns></returns>
        public int Width { get { return BitmapImage.Width; } }

        /// <summary>
        /// 画像の縦幅
        /// </summary>
        /// <returns></returns>
        public int Height { get { return BitmapImage.Height; } }

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
            get
            {
                return _imageInfo.DirectoryName;
            }
        }

        public bool Save(string filePath, IAuxiliaryLine auxiliaryLine)
        {
            return true;
        }
    }
}
