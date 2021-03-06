﻿using System.Drawing;
using MyTrimmingNew;
using MyTrimmingNew.AuxiliaryLine;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineChangeSizeTopRight : AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineChangeSizeTopRight(AuxiliaryController ac) : base(ac) { }

        public override int GetMouseUpX(AuxiliaryController ac,
                                        int changeSizeWidthPixel)
        {
            return ac.AuxiliaryRightTop.X + changeSizeWidthPixel;
        }

        public override int GetMouseUpY(AuxiliaryController ac,
                                        int changeSizeHeightPixel)
        {
            // 矩形拡大のために動かす方向 = Top値が減る方向
            return ac.AuxiliaryRightTop.Y - changeSizeHeightPixel;
        }

        public override System.Windows.Point GetMouseDownPoint(AuxiliaryController ac)
        {
            return new System.Windows.Point(ac.AuxiliaryRightTop.X, ac.AuxiliaryRightTop.Y);
        }

        public override AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseWidth(int changeSizeWidth, int changeSizeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 右上点の操作なら左側や下側は変わらない
            int changeWidth = changeSizeWidth;
            int changeHeight = changeSizeHeight;
            int newTop = AC.AuxiliaryTop - CalcHeightChangeSize(changeSizeWidth, changeSizeHeight);
            int newRight = AC.AuxiliaryRight + changeSizeWidth;
            int maxRight = GetMaxRight();
            int minTop = GetMinTop();

            if (newRight > maxRight)
            {
                newTop = AC.AuxiliaryTop - CalcHeightChangeSize(newRight - maxRight, changeHeight);
                newRight = maxRight;
            }
            if (newTop < minTop)
            {
                changeHeight = AC.AuxiliaryTop - minTop;
                newRight = AC.AuxiliaryRight + CalcWidthChangeSize(changeWidth, changeHeight);
                newTop = minTop;
            }

            newParameter.ReplacePoint(new Point(AC.AuxiliaryLeft, newTop),
                                      new Point(AC.AuxiliaryLeft, AC.AuxiliaryBottom),
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
            int newTop = AC.AuxiliaryTop - changeSizeHeight;
            int newRight = AC.AuxiliaryRight + CalcWidthChangeSize(changeSizeWidth, changeSizeHeight);
            int maxRight = GetMaxRight();
            int minTop = GetMinTop();

            if (newTop < minTop)
            {
                changeHeight = AC.AuxiliaryTop - minTop;
                newRight = AC.AuxiliaryRight + CalcWidthChangeSize(changeWidth, changeHeight);
                newTop = minTop;
            }
            if (newRight > maxRight)
            {
                newRight = maxRight;
                newTop = AC.AuxiliaryTop - CalcHeightChangeSize(newRight - AC.AuxiliaryRight, changeHeight);
            }

            newParameter.ReplacePoint(new Point(AC.AuxiliaryLeft, newTop),
                                      new Point(AC.AuxiliaryLeft, AC.AuxiliaryBottom),
                                      new Point(newRight, newTop),
                                      new Point(newRight, AC.AuxiliaryBottom));

            return newParameter;
        }

        public override bool WillChangeAuxiliaryOrigin(int newLeft, int newTop, int newRight, int newBottom)
        {
            // 右上点を思いっきり左や下に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if (AC.AuxiliaryLeft > newRight || AC.AuxiliaryBottom < newTop)
            {
                return true;
            }
            return false;
        }
    }
}