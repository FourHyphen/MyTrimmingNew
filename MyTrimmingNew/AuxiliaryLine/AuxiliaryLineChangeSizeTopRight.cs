using System.Drawing;

namespace MyTrimmingNew.AuxiliaryLine
{
    internal class AuxiliaryLineChangeSizeTopRight : AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineChangeSizeTopRight(AuxiliaryController ac) : base(ac) { }

        public override bool WillChangeAuxilirayOrigin(int newLeft, int newTop, int newRight, int newBottom)
        {
            // 右上点を思いっきり左や下に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if (AC.AuxiliaryLeftRelativeImage > newRight || AC.AuxiliaryBottom < newTop)
            {
                return true;
            }
            return false;
        }

        public override AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseWidth(int changeSizeWidth, int changeSizeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 右上点の操作なら左側や下側は変わらない
            int changeWidth = changeSizeWidth;
            int changeHeight = changeSizeHeight;
            int newTop = AC.AuxiliaryTopRelativeImage - CalcHeightChangeSize(changeSizeWidth, changeSizeHeight);
            int newRight = AC.AuxiliaryRight + changeSizeWidth;
            int maxRight = GetMaxRight();
            int minTop = GetMinTop();

            if (newRight > maxRight)
            {
                newTop = AC.AuxiliaryTopRelativeImage - CalcHeightChangeSize(newRight - maxRight, changeHeight);
                newRight = maxRight;
            }
            if (newTop < minTop)
            {
                changeHeight = AC.AuxiliaryTopRelativeImage - minTop;
                newRight = AC.AuxiliaryRight + CalcWidthChangeSize(changeWidth, changeHeight);
                newTop = minTop;
            }

            newParameter.ReplacePoint(new Point(AC.AuxiliaryLeftRelativeImage, newTop),
                                      new Point(AC.AuxiliaryLeftRelativeImage, AC.AuxiliaryBottom),
                                      new Point(newRight, newTop),
                                      new Point(newRight, AC.AuxiliaryBottom));

            return newParameter;
        }

        public override AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseHeight(int changeSizeWidth, int changeSizeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 右上点の操作なら左側や下側は変わらない
            int changeWidth = changeSizeWidth;
            int changeHeight = changeSizeHeight;
            int newTop = AC.AuxiliaryTopRelativeImage - changeSizeHeight;
            int newRight = AC.AuxiliaryRight + CalcWidthChangeSize(changeSizeWidth, changeSizeHeight);
            int maxRight = GetMaxRight();
            int minTop = GetMinTop();

            if (newTop < minTop)
            {
                changeHeight = AC.AuxiliaryTopRelativeImage - minTop;
                newRight = AC.AuxiliaryRight + CalcWidthChangeSize(changeWidth, changeHeight);
                newTop = minTop;
            }
            if (newRight > maxRight)
            {
                newRight = maxRight;
                newTop = AC.AuxiliaryTopRelativeImage - CalcHeightChangeSize(newRight - AC.AuxiliaryRight, changeHeight);
            }

            newParameter.ReplacePoint(new Point(AC.AuxiliaryLeftRelativeImage, newTop),
                                      new Point(AC.AuxiliaryLeftRelativeImage, AC.AuxiliaryBottom),
                                      new Point(newRight, newTop),
                                      new Point(newRight, AC.AuxiliaryBottom));

            return newParameter;
        }
    }
}