using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

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
        /// 切り抜き画像保存
        /// 切り抜き範囲の原点指定は画像を親とした相対値としてください
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="seemingOriginX"></param>
        /// <param name="seemingOriginY"></param>
        /// <param name="seemingWidth"></param>
        /// <param name="seemingHeight"></param>
        /// <returns></returns>
        public static void SaveTrimImage(Bitmap image,
                                         String filePath,
                                         int seemingLeft,
                                         int seemingTop,
                                         int seemingRight,
                                         int seemingBottom,
                                         double ratio)
        {
            // 1/1スケール画像上の切り抜き範囲
            int left = (int)((double)seemingLeft / ratio);
            int top = (int)((double)seemingTop / ratio);
            int right = (int)((double)seemingRight / ratio);
            int bottom = (int)((double)seemingBottom / ratio);

            ImageTrim imageTrim = new ImageTrim();
            imageTrim.Execute(image, filePath, left, top, right, bottom);
        }

        /// <summary>
        /// 回転後の見た目の矩形4隅および回転角度から切り抜き画像を保存
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filePath"></param>
        /// <param name="seemingLeftTop"></param>
        /// <param name="seemingLeftBottom"></param>
        /// <param name="seemingRightTop"></param>
        /// <param name="seemingRightBottom"></param>
        /// <param name="ratio"></param>
        /// <param name="degree"></param>
        public static void SaveTrimImage(Bitmap image,
                                         String filePath,
                                         Point seemingLeftTop,
                                         Point seemingLeftBottom,
                                         Point seemingRightTop,
                                         Point seemingRightBottom,
                                         double ratio,
                                         int degree)
        {
            // 1/1スケール画像上のパラメーターに変換
            Point leftTop = PointRatio(seemingLeftTop, ratio);
            Point leftBottom = PointRatio(seemingLeftBottom, ratio);
            Point rightTop = PointRatio(seemingRightTop, ratio);
            Point rightBottom = PointRatio(seemingRightBottom, ratio);

            ImageTrim imageTrim = new ImageTrim();
            imageTrim.Execute(image, filePath, leftTop, leftBottom, rightTop, rightBottom, degree);
        }

        private static Point PointRatio(Point p, double ratio)
        {
            return new Point((int)(p.X / ratio), (int)(p.Y / ratio));
        }
    }
}
