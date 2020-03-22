using System.Windows.Controls;

namespace MyTrimmingNew
{
    class ShowSupportMessageObserver : IVisualComponentObserver
    {
        private ContentControl CC { get; set; }

        private ImageController IC { get; set; }

        public ShowSupportMessageObserver(ContentControl cc)
        {
            CC = cc;
            DrawInit();
        }

        public void Attach(ImageController ic)
        {
            IC = ic;
            IC.Attach(this);
            Draw(ic);
        }

        public void Update(object o)
        {
            ImageController ic = (ImageController)o;
            if (ic == IC)
            {
                Draw(ic);
            }
        }

        private void DrawInit()
        {
            CC.Content = "画像ファイルを開いてください";
        }

        private void Draw(ImageController ic)
        {
            CC.Content = "";
        }
    }
}
