using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    class ImageOpenDirector
    {
        private ImageFileOpenDialog _ifod;

        public ImageOpenDirector() { }

        /// <summary>
        /// 画像を開く一連の処理を実行する
        /// </summary>
        /// <returns>
        /// True: 画像を開いた
        /// False: 画像を開かなかった(ユーザーがキャンセルした)
        /// </returns>
        /// <exception>
        /// 画像オープンエラー
        /// </exception>
        public DisplayImage ImageOpen(System.Windows.Controls.Image imageField,
                               System.Windows.Shapes.Rectangle auxiliaryLineField,
                               System.Windows.Controls.Primitives.StatusBarItem imageSizeField,
                               int windowWidth,
                               int windowHeight)
        {
            _ifod = ImageFileOpenDialog.GetInstance();
            string filePath = _ifod.Show();

            if (filePath == "")
            {
                return null;
            }

            // TODO: 実装をプログラミングしてないか？
            DisplayImage image = new DisplayImage(filePath, windowWidth, windowHeight);
            imageField.Source = image.GetImage();
            imageSizeField.Content = "オリジナル画像: 横" + image.OriginalImageWidth.ToString() + "x縦" + image.OriginalImageHeight.ToString();

            return image;
        }
    }
}
