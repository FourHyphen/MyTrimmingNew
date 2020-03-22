using System.Drawing;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineChangeSizeBottomLeft : AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineChangeSizeBottomLeft(AuxiliaryController ac) : base(ac) { }

        public override bool WillChangeAuxilirayOrigin(int newLeft, int newTop, int newRight, int newBottom)
        {
            // 左下点を思いっきり右や上に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if (AC.AuxiliaryRight < newLeft || AC.AuxiliaryTop > newBottom)
            {
                return true;
            }
            return false;
        }

        public override AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseWidth(int changeSizeWidth, int changeSizeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 左下点の操作なら右側と上側は変わらない
            int changeWidth = changeSizeWidth;
            int changeHeight = changeSizeHeight;
            int newLeft = AC.AuxiliaryLeft - changeSizeWidth;
            int newBottom = AC.AuxiliaryBottom + CalcHeightChangeSize(changeSizeWidth, changeSizeHeight);
            int minLeft = GetMinLeft();
            int maxBottom = GetMaxBottom();

            if (newLeft < minLeft)
            {
                newBottom = AC.AuxiliaryBottom + CalcHeightChangeSize(minLeft - newLeft, changeHeight);
                newLeft = minLeft;
            }
            if (newBottom > maxBottom)
            {
                changeHeight = maxBottom - AC.AuxiliaryBottom;
                newLeft = AC.AuxiliaryLeft - CalcWidthChangeSize(changeWidth, changeHeight);
                newBottom = maxBottom;
            }

            newParameter.ReplacePoint(new Point(newLeft, AC.AuxiliaryTop),
                                      new Point(newLeft, newBottom),
                                      new Point(AC.AuxiliaryRight, AC.AuxiliaryTop),
                                      new Point(AC.AuxiliaryRight, newBottom));

            return newParameter;
        }

        public override AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseHeight(int changeSizeWidth, int changeSizeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 左下点の操作なら右側と上側は変わらない
            int changeWidth = changeSizeWidth;
            int changeHeight = changeSizeHeight;
            int newLeft = AC.AuxiliaryLeft - CalcWidthChangeSize(changeSizeWidth, changeSizeHeight);
            int newBottom = AC.AuxiliaryBottom + changeSizeHeight;
            int minLeft = GetMinLeft();
            int maxBottom = GetMaxBottom();

            if (newBottom > maxBottom)
            {
                changeHeight = maxBottom - AC.AuxiliaryBottom;
                newLeft = AC.AuxiliaryLeft - CalcWidthChangeSize(changeWidth, changeHeight);
                newBottom = maxBottom;
            }
            if (newLeft < minLeft)
            {
                newLeft = minLeft;
                newBottom = AC.AuxiliaryBottom + CalcHeightChangeSize(AC.AuxiliaryLeft - newLeft, changeHeight);
            }

            newParameter.ReplacePoint(new Point(newLeft, AC.AuxiliaryTop),
                                      new Point(newLeft, newBottom),
                                      new Point(AC.AuxiliaryRight, AC.AuxiliaryTop),
                                      new Point(AC.AuxiliaryRight, newBottom));

            return newParameter;
        }
    }
}
