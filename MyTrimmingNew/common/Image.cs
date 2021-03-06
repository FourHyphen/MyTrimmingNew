﻿using System;
using System.Drawing;

namespace MyTrimmingNew.common
{
    public class Image
    {
        /// <summary>
        /// サイズを変更したBitmapSourceImageを作成する
        /// </summary>
        /// <param name="image"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        public static System.Windows.Media.Imaging.BitmapSource CreateBitmapSourceImage(Bitmap image,
                                                                                        int newWidth,
                                                                                        int newHeight)
        {
            Bitmap resizeImage = CreateBitmap(image, newWidth, newHeight);
            return CreateBitmapSourceImage(resizeImage);
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// BitmapからBitmapSourceを作成する
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapSource CreateBitmapSourceImage(Bitmap bitmapImage)
        {
            // 参考: http://qiita.com/KaoruHeart/items/dc130d5fc00629c1b6ea
            IntPtr handle = bitmapImage.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.
                    CreateBitmapSourceFromHBitmap(handle,
                                                  IntPtr.Zero,
                                                  System.Windows.Int32Rect.Empty,
                                                  System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(handle);
            }
        }

        /// <summary>
        /// 縦横幅を変更したBitmap画像を作成する
        /// </summary>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        public static Bitmap CreateBitmap(Bitmap image, int newWidth, int newHeight)
        {
            // 縮小画像作成(TODO: 最も画像劣化の少ない縮小アルゴリズムの選定)
            Bitmap reductionImage = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(reductionImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(image, 0, 0, newWidth, newHeight);

            g.Dispose();
            return reductionImage;
        }

        /// <summary>
        /// imageの(left, top)からwidth, heightの範囲を切り抜いたBitmapを返す
        /// </summary>
        public static System.Drawing.Bitmap CreateTrimImage(Bitmap image,
                                                            int left,
                                                            int top,
                                                            int width,
                                                            int height)
        {
            ImageTrim imageTrim = new ImageTrim();
            return imageTrim.Execute(image, left, top, width, height);
        }

        /// <summary>
        /// 回転後の矩形4隅および回転角度から切り抜き画像を作成
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap CreateTrimImage(Bitmap image,
                                                            Point leftTop,
                                                            Point leftBottom,
                                                            Point rightTop,
                                                            Point rightBottom,
                                                            int degree)
        {
            ImageTrim imageTrim = new ImageTrim();
            return imageTrim.Execute(image, leftTop, leftBottom, rightTop, rightBottom, degree);
        }
    }
}
