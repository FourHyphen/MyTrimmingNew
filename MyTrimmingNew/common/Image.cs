using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyTrimmingNew.common
{
    public class Image
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject(IntPtr hObject);

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
    }
}
