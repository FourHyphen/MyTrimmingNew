using System.Drawing;
using MyTrimmingNew;
using MyTrimmingNew.AuxiliaryLine;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineChangeSizeTopLeft : AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineChangeSizeTopLeft(AuxiliaryController ac) : base(ac) { }

        public override int GetMouseUpX(AuxiliaryController ac,
                                        int changeSizeWidthPixel)
        {
            // 矩形拡大のために動かす方向 = Left値が減る方向
            return ac.AuxiliaryLeftTop.X - changeSizeWidthPixel;
        }

        public override int GetMouseUpY(AuxiliaryController ac,
                                        int changeSizeHeightPixel)
        {
            // 矩形拡大のために動かす方向 = Top値が減る方向
            return ac.AuxiliaryLeftTop.Y - changeSizeHeightPixel;
        }

        public override System.Windows.Point GetMouseDownPoint(AuxiliaryController ac)
        {
            return new System.Windows.Point(ac.AuxiliaryLeftTop.X, ac.AuxiliaryLeftTop.Y);
        }

        public override AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseWidth(int changeSizeWidth, int changeSizeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 左上点の操作なら右側や下側は変わらない
            int changeWidth = changeSizeWidth;
            int changeHeight = changeSizeHeight;
            int newLeft = AC.AuxiliaryLeft - changeSizeWidth;
            int newTop = AC.AuxiliaryTop - CalcHeightChangeSize(changeSizeWidth, changeSizeHeight);
            int minLeft = GetMinLeft();
            int minTop = GetMinTop();

            if (newLeft < minLeft)
            {
                newTop = AC.AuxiliaryTop - CalcHeightChangeSize(minLeft - newLeft, changeHeight);
                newLeft = minLeft;
            }
            if (newTop < minTop)
            {
                changeHeight = AC.AuxiliaryTop - minTop;
                newLeft = AC.AuxiliaryLeft - CalcWidthChangeSize(changeWidth, changeHeight);
                newTop = minTop;
            }

            newParameter.ReplacePoint(new Point(newLeft, newTop),
                                      new Point(newLeft, AC.AuxiliaryBottom),
                                      new Point(AC.AuxiliaryRight, newTop),
                                      new Point(AC.AuxiliaryRight, AC.AuxiliaryBottom));
            return newParameter;
        }

        public override AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseHeight(int changeSizeWidth, int changeSizeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 左上点の操作なら右側や下側は変わらない
            int changeWidth = changeSizeWidth;
            int changeHeight = changeSizeHeight;
            int newLeft = AC.AuxiliaryLeft - CalcWidthChangeSize(changeSizeWidth, changeSizeHeight);
            int newTop = AC.AuxiliaryTop - changeSizeHeight;
            int minLeft = GetMinLeft();
            int minTop = GetMinTop();

            if (newTop < minTop)
            {
                changeHeight = AC.AuxiliaryTop - minTop;
                newLeft = AC.AuxiliaryLeft - CalcWidthChangeSize(changeWidth, changeHeight);
                newTop = minTop;
            }
            if (newLeft < minLeft)
            {
                newLeft = minLeft;
                newTop = AC.AuxiliaryTop - CalcHeightChangeSize(AC.AuxiliaryLeft - newLeft, changeHeight);
            }

            newParameter.ReplacePoint(new Point(newLeft, newTop),
                                      new Point(newLeft, AC.AuxiliaryBottom),
                                      new Point(AC.AuxiliaryRight, newTop),
                                      new Point(AC.AuxiliaryRight, AC.AuxiliaryBottom));
            return newParameter;
        }

        public override bool WillChangeAuxiliaryOrigin(int newLeft, int newTop, int newRight, int newBottom)
        {
            // 左上点を思いっきり右や下に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if (AC.AuxiliaryRight < newLeft || AC.AuxiliaryBottom < newTop)
            {
                return true;
            }
            return false;
        }
    }
}
