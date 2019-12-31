using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                                                                                       Point mouseDownCoordinate)
        {
            int mouseX = (int)mouseDownCoordinate.X;
            int mouseY = (int)mouseDownCoordinate.Y;
            bool isInRangeLeft = IsInRangeMouseDownArea(mouseX, 0);
            bool isInRangeRight = IsInRangeMouseDownArea(mouseX, ac.AuxiliaryWidth);
            bool isInRangeTop = IsInRangeMouseDownArea(mouseY, 0);
            bool isInRangeBottom = IsInRangeMouseDownArea(mouseY, ac.AuxiliaryHeight);

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
            if ((0 < mouseX && mouseX < ac.AuxiliaryWidth) &&
                (0 < mouseY && mouseY < ac.AuxiliaryHeight))
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
    }
}
