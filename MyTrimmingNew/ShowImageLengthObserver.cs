using System.Windows.Controls;

namespace MyTrimmingNew
{
    class ShowImageLengthObserver : IVisualComponentObserver
    {
        private ContentControl CC { get; set; }

        private ImageController IC { get; set; }

        public ShowImageLengthObserver(ContentControl cc, ImageController ic)
        {
            CC = cc;
            IC = ic;
            IC.Attach(this);
            Draw(IC);
        }

        public void Update(object o)
        {
            ImageController ic = (ImageController)o;
            if (ic == IC)
            {
                Draw(ic);
            }
        }

        private void Draw(ImageController ic)
        {
            CC.Content = "オリジナル画像: 横" + ic.OriginalImageWidth.ToString() + "x縦" + ic.OriginalImageHeight.ToString();
        }
    }
}
