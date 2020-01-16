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
        /// TODO: 例外処理の実装、ユーザーから見て、保存失敗したときはシステムにどうして欲しい？
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="seemingOriginX"></param>
        /// <param name="seemingOriginY"></param>
        /// <param name="seemingWidth"></param>
        /// <param name="seemingHeight"></param>
        /// <returns></returns>
        public static void SaveTrimImage(Bitmap image,
                                         String filePath,
                                         int seemingOriginX,
                                         int seemingOriginY,
                                         int seemingWidth,
                                         int seemingHeight,
                                         double ratio)
        {
            // 1/1スケール画像上の切り抜き範囲
            int originX = (int)((double)seemingOriginX / ratio);
            int originY = (int)((double)seemingOriginY / ratio);
            int trimWidth = (int)((double)seemingWidth / ratio);
            int trimHeight = (int)((double)seemingHeight / ratio);

            // 切り抜き画像作成
            Bitmap trimImage = new Bitmap(trimWidth, trimHeight);
            Graphics g = Graphics.FromImage(trimImage);
            Rectangle trim = new Rectangle(originX, originY, trimWidth, trimHeight);
            Rectangle draw = new Rectangle(0, 0, trim.Width, trim.Height);
            g.DrawImage(image, draw, trim, GraphicsUnit.Pixel);
            g.Dispose();

            // 保存
            trimImage.Save(filePath);
        }
    }
}
