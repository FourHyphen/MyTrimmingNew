using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
            int mouseXRelative = (int)mouseDownCoordinateRelativeAuxiliaryLine.X;
            int mouseYRelative = (int)mouseDownCoordinateRelativeAuxiliaryLine.Y;
            bool isInRangeLeft = IsInRangeMouseDownArea(mouseXRelative, 0);
            bool isInRangeRight = IsInRangeMouseDownArea(mouseXRelative, ac.AuxiliaryRight - ac.AuxiliaryLeftRelativeImage);
            bool isInRangeTop = IsInRangeMouseDownArea(mouseYRelative, 0);
            bool isInRangeBottom = IsInRangeMouseDownArea(mouseYRelative, ac.AuxiliaryBottom - ac.AuxiliaryTopRelativeImage);

            // 補助線の4隅の点のいずれか
            if (isInRangeLeft)
            {
                if (isInRangeTop)
                {
                    return KindMouseDownAuxiliaryLineArea.LeftTop;
                }
                else if (isInRangeBottom)
                {
                    return KindMouseDownAuxiliaryLineArea.LeftBottom;
                }
            }
            else if (isInRangeRight)
            {
                if (isInRangeTop)
                {
                    return KindMouseDownAuxiliaryLineArea.RightTop;
                }
                else if (isInRangeBottom)
                {
                    return KindMouseDownAuxiliaryLineArea.RightBottom;
                }
            }

            // 4隅の点ではないが、補助線の内側
            if (IsMouseDownInsideAuxiliaryLine(ac, mouseXRelative, mouseYRelative))
            {
                return KindMouseDownAuxiliaryLineArea.Inside;
            }

            return KindMouseDownAuxiliaryLineArea.Else;
        }

        private static bool IsInRangeMouseDownArea(int mouseDownCoordinate, int baseCoordinate)
        {
            int minusBasePoint = baseCoordinate - Constant.MouseDownPointMargin;
            int plusBasePoint = baseCoordinate + Constant.MouseDownPointMargin;
            return (minusBasePoint < mouseDownCoordinate && mouseDownCoordinate < plusBasePoint);
        }

        private static bool IsMouseDownInsideAuxiliaryLine(AuxiliaryController ac,
                                                           int mouseXRelativeAuxiliaryLine,
                                                           int mouseYRelativeAuxiliaryLine)
        {
            int mouseX = mouseXRelativeAuxiliaryLine + ac.AuxiliaryLeftRelativeImage;
            int mouseY = mouseYRelativeAuxiliaryLine + ac.AuxiliaryTopRelativeImage;
            if ((ac.AuxiliaryLeftRelativeImage < mouseX && mouseX < ac.AuxiliaryRight) &&
                (ac.AuxiliaryTopRelativeImage < mouseY && mouseY < ac.AuxiliaryBottom))
            {
                return true;
            }
            return false;
        }
    }
}
