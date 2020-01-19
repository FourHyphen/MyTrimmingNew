using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace MyTrimmingNew
{
    class ImageSaveResultFieldObserver : IVisualComponentObserver
    {
        private StatusBarItem StatusBarItem { get; set; }

        private ImageController IC { get; set; }

        public ImageSaveResultFieldObserver(StatusBarItem sbi, ImageController ic)
        {
            StatusBarItem = sbi;
            IC = ic;
            IC.Attach(this);
            DrawInitStr();
        }

        public void Update(object o)
        {
            ImageController ic = (ImageController)o;
            if (ic == IC)
            {
                Draw(ic);
            }
        }

        private void DrawInitStr()
        {
            StatusBarItem.Content = "ここに保存結果が表示されます";
        }

        private void Draw(ImageController ic)
        {
            StatusBarItem.Content = (ic.SaveResult) ? "保存に成功しました" : "保存に失敗しました";
        }
    }
}
