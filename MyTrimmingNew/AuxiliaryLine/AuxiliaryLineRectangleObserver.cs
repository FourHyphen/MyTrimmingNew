using System.Windows.Shapes;
using MyTrimmingNew.common;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineRectangleObserver : IVisualComponentObserver
    {
        private Polyline Polyline { get; set; }

        private AuxiliaryController AC { get; set; }

        public AuxiliaryLineRectangleObserver(Polyline polyline, AuxiliaryController ac)
        {
            Polyline = polyline;
            AC = ac;
            AC.Attach(this);
            Draw(AC);
        }

        public void Update(object o)
        {
            AuxiliaryController ac = (AuxiliaryController)o;
            if (ac == AC)
            {
                Draw(ac);
            }
        }

        private void Draw(AuxiliaryController ac)
        {
            Polyline.Points[0] = Common.ToWindowsPoint(AC.AuxiliaryLeftTop);
            Polyline.Points[1] = Common.ToWindowsPoint(AC.AuxiliaryRightTop);
            Polyline.Points[2] = Common.ToWindowsPoint(AC.AuxiliaryRightBottom);
            Polyline.Points[3] = Common.ToWindowsPoint(AC.AuxiliaryLeftBottom);
            Polyline.Points[4] = Common.ToWindowsPoint(AC.AuxiliaryLeftTop);  // 長方形として閉じる
        }
    }
}
