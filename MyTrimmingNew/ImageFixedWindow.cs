using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyTrimmingNew
{
    public class ImageFixedWindow
    {
        private Image _image;
        private Bitmap _fixedWindowImage;
        private System.Windows.Media.Imaging.BitmapSource _showFixedWindowImage;

        /// <summary>
        /// 画像全体をwindowに表示するための縮小率
        /// </summary>
        /// <returns></returns>
        private double _reductionRatio;

        public ImageFixedWindow(Image image, int windowWidth, int windowHeight)
        {
            _image = image;
            _fixedWindowImage = null;
            _showFixedWindowImage = null;
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
        }

        private int _windowWidth = 0;

        public int WindowWidth
        {
            // TODO: windowサイズは縦横両方とも一度に変わることもある
            //       このままだとWidth変更とHeight変更で画像作成処理がそれぞれ走ってしまう
            private get
            {
                return _windowWidth;
            }
            set
            {
                if (value != _windowWidth)
                {
                    _windowWidth = value;
                    CreateBitmapSourceImage();
                }
            }
        }

        private int _windowHeight = 0;

        public int WindowHeight
        {
            private get
            {
                return _windowHeight;
            }
            set
            {
                if (value != _windowHeight)
                {
                    WindowHeight = value;
                    CreateBitmapSourceImage();
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 表示用画像
        /// 参考: http://qiita.com/KaoruHeart/items/dc130d5fc00629c1b6ea
        /// </summary>
        /// <returns></returns>
        public System.Windows.Media.Imaging.BitmapSource GetImage()
        {
            if (_showFixedWindowImage == null)
            {
                CreateBitmapSourceImage();
            }

            return _showFixedWindowImage;
        }

        private void CreateBitmapSourceImage()
        {
            CreateImageFixedWindow();

            IntPtr handle = _fixedWindowImage.GetHbitmap();
            try
            {
                _showFixedWindowImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                            handle,
                                            IntPtr.Zero,
                                            System.Windows.Int32Rect.Empty,
                                            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()
                                        );
            }
            finally
            {
                DeleteObject(handle);
            }
        }

        public int Width
        {
            get
            {
                if(_showFixedWindowImage == null)
                {
                    CreateBitmapSourceImage();
                }
                return (int)_showFixedWindowImage.Width;
            }
        }

        public int Height
        {
            get
            {
                if (_showFixedWindowImage == null)
                {
                    CreateBitmapSourceImage();
                }
                return (int)_showFixedWindowImage.Height;
            }
        }

        /// <summary>
        /// windowサイズに合わせた画像を作成
        /// </summary>
        private void CreateImageFixedWindow()
        {
            // windowサイズに合わせて縮小するサイズを計算
            int reductionImageWidth = 0;
            int reductionImageHeight = 0;
            CalcReductionImageSize(ref reductionImageWidth, ref reductionImageHeight);

            // 縮小画像作成(TODO: 最も画像劣化の少ない縮小アルゴリズムの選定)
            _fixedWindowImage = new Bitmap(reductionImageWidth, reductionImageHeight);
            Graphics g = Graphics.FromImage(_fixedWindowImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(_image.BitmapImage, 0, 0, reductionImageWidth, reductionImageHeight);

            g.Dispose();
        }

        /// <summary>
        /// windowに画像全部が表示されるよう、画像の縦横比率を維持しつつ縮小するサイズを求める
        /// ex) windowサイズ横1000 * 縦800に横2160 * 縦3000の画像を表示する場合、
        ///     ・横縮小率: 2160 / 1000 = 2.16 | 画像サイズ 横1000 * 縦1388
        ///     ・縦縮小率: 3000 /  800 = 3.75 | 画像サイズ 横 576 * 縦 800
        ///     となり、横576 * 縦800に縮小すれば画像全部が表示される
        /// </summary>
        /// <param name="reductionImageWidth"></param>
        /// <param name="reductionImageHeight"></param>
        private void CalcReductionImageSize(ref int reductionImageWidth,
                                            ref int reductionImageHeight)
        {
            double widthReductionRatio = (double)_image.Width / (double)WindowWidth;
            double heightReductionRatio = (double)_image.Height / (double)WindowHeight;

            if (widthReductionRatio < heightReductionRatio)
            {
                _reductionRatio = heightReductionRatio;
                reductionImageWidth = (int)((double)_image.Width / _reductionRatio);
                reductionImageHeight = WindowHeight;
            }
            else
            {
                _reductionRatio = widthReductionRatio;
                reductionImageWidth = WindowWidth;
                reductionImageHeight = (int)((double)_image.Height / _reductionRatio);
            }
        }
    }
}
