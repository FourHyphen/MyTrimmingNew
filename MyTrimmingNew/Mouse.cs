using System.Drawing;
using MyTrimmingNew.common;

namespace MyTrimmingNew
{
    class Mouse
    {
        public enum KindMouseDownAuxiliaryLineArea
        {
            LeftTop,
            RightTop,
            RightBottom,
            LeftBottom,
            Inside,
            Else
        }

        public static KindMouseDownAuxiliaryLineArea GetKindMouseDownAuxiliaryLineArea(AuxiliaryController ac,
                                                                                       Point mouseDownCoordinateRelativeAuxiliaryLine)
        {
            int mouseX = (int)mouseDownCoordinateRelativeAuxiliaryLine.X;
            int mouseY = (int)mouseDownCoordinateRelativeAuxiliaryLine.Y;
            bool isNearLeftTop = IsInRangeMouseDownArea(mouseX, mouseY, ac.AuxiliaryLeftTop);
            bool isNearLeftBottom = IsInRangeMouseDownArea(mouseX, mouseY, ac.AuxiliaryLeftBottom);
            bool isNearRightTop = IsInRangeMouseDownArea(mouseX, mouseY, ac.AuxiliaryRightTop);
            bool isNearRightBottom = IsInRangeMouseDownArea(mouseX, mouseY, ac.AuxiliaryRightBottom);

            // 補助線の4隅の点のいずれか
            if (isNearLeftTop)
            {
                return KindMouseDownAuxiliaryLineArea.LeftTop;
            }
            else if (isNearLeftBottom)
            {
                return KindMouseDownAuxiliaryLineArea.LeftBottom;
            }
            if (isNearRightTop)
            {
                return KindMouseDownAuxiliaryLineArea.RightTop;
            }
            if (isNearRightBottom)
            {
                return KindMouseDownAuxiliaryLineArea.RightBottom;
            }

            // 4隅の点ではないが、補助線の内側
            if (IsMouseDownInsideAuxiliaryLine(ac, mouseX, mouseY))
            {
                return KindMouseDownAuxiliaryLineArea.Inside;
            }

            return KindMouseDownAuxiliaryLineArea.Else;
        }

        private static bool IsInRangeMouseDownArea(int mouseDownX, int mouseDownY, Point p)
        {
            if (!IsInRangeMouseDownArea(mouseDownX, p.X))
            {
                return false;
            }

            if (!IsInRangeMouseDownArea(mouseDownY, p.Y))
            {
                return false;
            }

            return true;
        }

        private static bool IsInRangeMouseDownArea(int mouseDownCoordinate, int baseCoordinate)
        {
            int minusBasePoint = baseCoordinate - Constant.MouseDownPointMargin;
            int plusBasePoint = baseCoordinate + Constant.MouseDownPointMargin;
            return (minusBasePoint < mouseDownCoordinate && mouseDownCoordinate < plusBasePoint);
        }

        private static bool IsMouseDownInsideAuxiliaryLine(AuxiliaryController ac, int mouseX, int mouseY)
        {
            TrimRect trimRect = new TrimRect(ac.AuxiliaryLeftTop,
                                             ac.AuxiliaryRightTop,
                                             ac.AuxiliaryRightBottom,
                                             ac.AuxiliaryLeftBottom);
            return (trimRect.IsInside(mouseX, mouseY));
        }
    }
}
