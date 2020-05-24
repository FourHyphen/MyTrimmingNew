using System;
using System.Drawing;
using MyTrimmingNew.common;

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

            SetNewParameter(newParameter, newRight - AC.AuxiliaryRight, newBottom - AC.AuxiliaryBottom);
            return newParameter;
        }

        private void SetNewParameter(AuxiliaryLineParameter param, int changeSizeRight, int changeSizeBottom)
        {
            param.ReplacePoint(AC.AuxiliaryLeftTop,
                               new Point(AC.AuxiliaryLeftBottom.X, AC.AuxiliaryLeftBottom.Y + changeSizeBottom),
                               new Point(AC.AuxiliaryRightTop.X + changeSizeRight, AC.AuxiliaryRightTop.Y),
                               new Point(AC.AuxiliaryRightBottom.X + changeSizeRight, AC.AuxiliaryRightBottom.Y + changeSizeBottom));
            return;

            //if (AC.AuxiliaryDegree == 0)
            //{
            //    param.ReplacePoint(AC.AuxiliaryLeftTop,
            //                       new Point(AC.AuxiliaryLeftBottom.X, AC.AuxiliaryLeftBottom.Y + changeSizeBottom),
            //                       new Point(AC.AuxiliaryRightTop.X + changeSizeRight, AC.AuxiliaryRightTop.Y),
            //                       new Point(AC.AuxiliaryRightBottom.X + changeSizeRight, AC.AuxiliaryRightBottom.Y + changeSizeBottom));
            //    return;
            //}

            //// TODO: LeftTopを基準に矩形のWidthとHeight(比率保ったまま)を求めて、そのサイズと回転角から残り3点の座標を計算した方が良い
            //Point newRightBottom = GetNewRightBottom(changeSizeRight, changeSizeBottom);
            //Point newRightTop = GetNewRightTop(newRightBottom);
            //Point newLeftBottom = GetNewLeftBottom(newRightBottom);

            //param.ReplacePoint(AC.AuxiliaryLeftTop, newLeftBottom, newRightTop, newRightBottom);
            //return;
        }

        private Point GetNewRightBottom(int changeSizeRight, int changeSizeBottom)
        {
            int x = AC.AuxiliaryRightBottom.X + changeSizeRight;
            int y = AC.AuxiliaryRightBottom.Y + changeSizeBottom;
            return new Point(x, y);
        }

        private Point GetNewRightTop(Point newRightBottom)
        {
            // 傾きに関して連立方程式を作り、それを解く
            double aTop = Common.CalcSlope(AC.AuxiliaryLeftTop, AC.AuxiliaryRightTop);
            double aLeft = Common.CalcSlope(AC.AuxiliaryLeftTop, AC.AuxiliaryLeftBottom);
            double DenomX = aTop - aLeft;
            double NumerX = aTop * (double)AC.AuxiliaryLeftTop.X - aLeft * newRightBottom.X - (double)AC.AuxiliaryLeftTop.Y + newRightBottom.Y;
            double newRightTopX = NumerX / DenomX;
            double newRightTopY = aTop * (newRightTopX - (double)AC.AuxiliaryLeftTop.X) + (double)AC.AuxiliaryLeftTop.Y;

            return new Point((int)newRightTopX, (int)newRightTopY);
        }

        private Point GetNewLeftBottom(Point newRightBottom)
        {
            // 傾きに関して連立方程式を作り、それを解く
            double aBottom = Common.CalcSlope(AC.AuxiliaryLeftBottom, AC.AuxiliaryRightBottom);
            double aLeft = Common.CalcSlope(AC.AuxiliaryLeftTop, AC.AuxiliaryLeftBottom);
            double DenomX = aBottom - aLeft;
            double NumerX = aBottom * (double)newRightBottom.X - aLeft * AC.AuxiliaryLeftTop.X + (double)AC.AuxiliaryLeftTop.Y - newRightBottom.Y;
            double newRightBottomX = NumerX / DenomX;
            double newRightBottomY = aBottom * (newRightBottomX - (double)newRightBottom.X) + (double)newRightBottom.Y;

            return new Point((int)newRightBottomX, (int)newRightBottomY);
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
                newBottom = AC.AuxiliaryBottom + CalcHeightChangeSize(maxRight - AC.AuxiliaryRight, changeHeight);
            }

            SetNewParameter(newParameter, newRight - AC.AuxiliaryRight, newBottom - AC.AuxiliaryBottom);
            return newParameter;
        }
    }
}
