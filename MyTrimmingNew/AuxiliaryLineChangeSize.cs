using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTrimmingNew
{
    class AuxiliaryLineChangeSize : IAuxiliaryLineOperation
    {
        private enum KindMouseDownArea
        {
            AuxiliaryLineLeftTop,
            AuxiliaryLineRightTop,
            AuxiliaryLineRightBottom,
            AuxiliaryLineLeftBottom,
            AuxiliaryLineInside,
            Else
        }

        private AuxiliaryController AC { get; set; }

        private Point MouseDownCoordinate { get; set; }

        private KindMouseDownArea MouseDownArea { get; set; }

        public AuxiliaryLineChangeSize(AuxiliaryController ac, Point mouseDownCoordinate)
        {
            AC = ac;
            MouseDownCoordinate = mouseDownCoordinate;
            MouseDownArea = GetKindMouseDownArea();
        }

        private KindMouseDownArea GetKindMouseDownArea()
        {
            int mouseX = (int)MouseDownCoordinate.X;
            int mouseY = (int)MouseDownCoordinate.Y;
            bool isInRangeLeft = IsInRangeMouseDownArea(mouseX, 0);
            bool isInRangeRight = IsInRangeMouseDownArea(mouseX, AC.AuxiliaryWidth);
            bool isInRangeTop = IsInRangeMouseDownArea(mouseY, 0);
            bool isInRangeBottom = IsInRangeMouseDownArea(mouseY, AC.AuxiliaryHeight);

            // 補助線の4隅の点のいずれか
            if (isInRangeLeft)
            {
                if (isInRangeTop)
                {
                    return KindMouseDownArea.AuxiliaryLineLeftTop;
                }
                else if (isInRangeBottom)
                {
                    return KindMouseDownArea.AuxiliaryLineLeftBottom;
                }
            }
            else if (isInRangeRight)
            {
                if (isInRangeTop)
                {
                    return KindMouseDownArea.AuxiliaryLineRightTop;
                }
                else if (isInRangeBottom)
                {
                    return KindMouseDownArea.AuxiliaryLineRightBottom;
                }
            }

            // 4隅の点ではないが、補助線の内側
            if ((0 < mouseX && mouseX < AC.AuxiliaryWidth) &&
                (0 < mouseY && mouseY < AC.AuxiliaryHeight))
            {
                return KindMouseDownArea.AuxiliaryLineInside;
            }

            return KindMouseDownArea.Else;
        }

        private bool IsInRangeMouseDownArea(int mouseDownCoordinate, int baseCoordinate)
        {
            int minusBasePoint = baseCoordinate - Constant.MouseDownPointMargin;
            int plusBasePoint = baseCoordinate + Constant.MouseDownPointMargin;
            return (minusBasePoint < mouseDownCoordinate && mouseDownCoordinate < plusBasePoint);
        }

        public void Execute(object operation)
        {
            Point mouseUpCoordinate = (Point)operation;
            int mouseMoveX = (int)mouseUpCoordinate.X - (int)MouseDownCoordinate.X;
            int mouseMoveY = (int)mouseUpCoordinate.Y - (int)MouseDownCoordinate.Y;

            if (MouseDownArea == KindMouseDownArea.AuxiliaryLineRightBottom)
            {
                ExecuteWhereOperationBottomRight(mouseMoveX, mouseMoveY);
            }
        }

        public void ExecuteWhereOperationBottomRight(int changeWidth, int changeHeight)
        {
            // widthとheight、基準にする変更サイズに合わせてRatioの通りにサイズを変更する
            int newWidth = AC.AuxiliaryWidth + changeWidth;
            int newHeight = AC.AuxiliaryHeight + changeHeight;
            if (BaseWidthWhenChangeSize(changeWidth, changeHeight))
            {
                newHeight = CalcAuxiliaryLineHeightWithFitRatio(newWidth);
            }
            else
            {
                newWidth = CalcAuxiliaryLineWidthWithFitRatio(newHeight);
            }

            // 右下点を思いっきり左や上に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if (newWidth < 0 || newHeight < 0)
            {
                return;
            }

            AC.AuxiliaryWidth = newWidth;
            AC.AuxiliaryHeight = newHeight;
        }

        private bool BaseWidthWhenChangeSize(int willChangeWidth, int willChangeHeight)
        {
            int baseWidthChangeHeight = CalcAuxiliaryLineHeightWithFitRatio(willChangeWidth);
            return (Math.Abs(baseWidthChangeHeight) > Math.Abs(willChangeHeight));
        }

        private int CalcAuxiliaryLineWidthWithFitRatio(int newAuxiliaryHeight)
        {
            double newWidth = (double)newAuxiliaryHeight * AC.AuxiliaryRatio;
            return (int)Math.Round(newWidth, 0, MidpointRounding.AwayFromZero);
        }

        private int CalcAuxiliaryLineHeightWithFitRatio(int newAuxiliaryWidth)
        {
            double newHeight = (double)newAuxiliaryWidth / AC.AuxiliaryRatio;
            return (int)Math.Round(newHeight, 0, MidpointRounding.AwayFromZero);
        }
    }
}
