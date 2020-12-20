using System.Windows.Controls;

namespace MyTrimmingNew.AuxiliaryLine
{
    class TrimmingImageLengthObserver : IVisualComponentObserver
    {
        private ContentControl CC { get; set; }

        private ImageController IC { get; set; }

        private AuxiliaryController AC { get; set; }

        public TrimmingImageLengthObserver(ContentControl cc, ImageController ic, AuxiliaryController ac)
        {
            CC = cc;
            IC = ic;
            AC = ac;
            AC.Attach(this);
            Draw(AC);
        }

        public void Update(object o)
        {
            AuxiliaryController ac = (AuxiliaryController)o;
            if(ac == AC)
            {
                Draw(ac);
            }
        }

        private void Draw(AuxiliaryController ac)
        {
            string width = GetWillTrimmingWidth(ac);
            string height = GetWillTrimmingHeight(ac);
            string length = "トリム領域: (横 " + width + ", 縦 " + height + ")";
            CC.Content = length;
        }

        private string GetWillTrimmingWidth(AuxiliaryController ac)
        {
            // 1/1スケール画像上の切り抜き範囲: 横の長さ
            double ratio = IC.ImageFitRatio;
            int left = (int)((double)ac.AuxiliaryLeft / ratio);
            int right = (int)((double)ac.AuxiliaryRight / ratio);
            return (right - left).ToString();
        }

        private string GetWillTrimmingHeight(AuxiliaryController ac)
        {
            // 1/1スケール画像上の切り抜き範囲: 縦の長さ
            double ratio = IC.ImageFitRatio;
            int top = (int)((double)ac.AuxiliaryTop / ratio);
            int bottom = (int)((double)ac.AuxiliaryBottom / ratio);
            return (bottom - top).ToString();
        }
    }
}
