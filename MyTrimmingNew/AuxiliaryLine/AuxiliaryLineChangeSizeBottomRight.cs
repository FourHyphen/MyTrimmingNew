using System.Drawing;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineChangeSizeBottomRight : AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineChangeSizeBottomRight(AuxiliaryController ac) : base(ac) { }

        public override bool WillChangeAuxilirayOrigin(int newLeft, int newTop, int newRight, int newBottom)
        {
            // 右下点を思いっきり左や上に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if (AC.AuxiliaryLeft > newLeft || AC.AuxiliaryTop > newBottom)
            {
                return true;
            }
            return false;
        }

        public override AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseWidth(int changeSizeWidth, int changeSizeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 右下点の操作なら左側や上側は変わらない
            int changeWidth = changeSizeWidth;
            int changeHeight = changeSizeHeight;
            int newRight = AC.AuxiliaryRight + changeWidth;
            int newBottom = AC.AuxiliaryBottom + CalcHeightChangeSize(changeWidth, changeHeight);
            int maxRight = GetMaxRight();
            int maxBottom = GetMaxBottom();

            if (newRight > maxRight)
            {
                newBottom = AC.AuxiliaryBottom + CalcHeightChangeSize(newRight - maxRight, changeHeight);
                newRight = maxRight;
            }
            if (newBottom > maxBottom)
            {
                changeHeight = maxBottom - AC.AuxiliaryBottom;
                newRight = AC.AuxiliaryRight + CalcWidthChangeSize(changeWidth, changeHeight);
                newBottom = maxBottom;
            }

            newParameter.ReplacePoint(new Point(AC.AuxiliaryLeft, AC.AuxiliaryTop),
                                      new Point(AC.AuxiliaryLeft, newBottom),
                                      new Point(newRight, AC.AuxiliaryTop),
                                      new Point(newRight, newBottom));

            return newParameter;
        }

        public override AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseHeight(int changeSizeWidth, int changeSizeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 右下点の操作なら左側や上側は変わらない
            int changeWidth = changeSizeWidth;
            int changeHeight = changeSizeHeight;
            int newRight = AC.AuxiliaryRight + CalcWidthChangeSize(changeSizeWidth, changeSizeHeight);
            int newBottom = AC.AuxiliaryBottom + changeSizeHeight;
            int maxRight = GetMaxRight();
            int maxBottom = GetMaxBottom();

            if (newBottom > maxBottom)
            {
                changeHeight = maxBottom - AC.AuxiliaryBottom;
                newRight = AC.AuxiliaryRight + CalcWidthChangeSize(changeWidth, changeHeight);
                newBottom = maxBottom;
            }
            if (newRight > maxRight)
            {
                newRight = maxRight;
                newBottom = AC.AuxiliaryBottom + CalcHeightChangeSize(newRight - AC.AuxiliaryRight, changeHeight);
            }

            newParameter.ReplacePoint(new Point(AC.AuxiliaryLeft, AC.AuxiliaryTop),
                                      new Point(AC.AuxiliaryLeft, newBottom),
                                      new Point(newRight, AC.AuxiliaryTop),
                                      new Point(newRight, newBottom));

            return newParameter;
        }
    }
}
