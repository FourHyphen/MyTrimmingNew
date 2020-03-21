using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineLengthObserver : IVisualComponentObserver
    {
        private ContentControl CC { get; set; }

        private AuxiliaryController AC { get; set; }

        public AuxiliaryLineLengthObserver(ContentControl cc, AuxiliaryController ac)
        {
            CC = cc;
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
            string leftTop = GetDrawParameter(ac.AuxiliaryLeftTop, "左上");
            string rightTop = GetDrawParameter(ac.AuxiliaryRightTop, "右上");
            string rightBottom = GetDrawParameter(ac.AuxiliaryRightBottom, "右下");
            string leftBottom = GetDrawParameter(ac.AuxiliaryLeftBottom, "左下");
            string rect = "矩形: " + leftTop + " / " + rightTop + " / " + rightBottom + " / " + leftBottom;
            CC.Content = rect;
        }

        private string GetDrawParameter(System.Drawing.Point p, string point)
        {
            return point + "(" + p.X.ToString() + ", " + p.Y.ToString() + ")";
        }
    }
}
