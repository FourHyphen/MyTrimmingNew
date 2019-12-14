﻿using System;
using System.Drawing;

namespace MyTrimmingNew
{
    public class Image
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

        public Bitmap BitmapImage { get; private set; }

        public int Width { get { return BitmapImage.Width; } }

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
