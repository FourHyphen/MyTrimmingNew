using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    class AuxiliaryLineOperationFactory
    {
        public IAuxiliaryLineOperation Create(AuxiliaryController ac)
        {
            return new AuxiliaryLineMove(ac);
        }

        public IAuxiliaryLineOperation Create(AuxiliaryController ac, 
                                              System.Windows.Point pointRelatedAuxiliaryLine)
        {
            // マウス押下場所によって補助線の操作内容を決定
            //KindMouseDownPoint mdp = GetKindMouseDownPoint(ac, pointRelatedAuxiliaryLine);
            //return new AuxiliaryLineChangeSize(ac);
            return null;
        }

        private enum KindMouseDownPoint
        {
            AuxiliaryLineLeftTop,
            AuxiliaryLineRightTop,
            AuxiliaryLineRightBottom,
            AuxiliaryLineLeftBottom,
            AuxiliaryLineInside,
            Else
        }

        private KindMouseDownPoint GetKindMouseDownPoint(AuxiliaryController ac, 
                                                         System.Windows.Point mouse)
        {
            int mouseX = (int)mouse.X;
            int mouseY = (int)mouse.Y;
            bool isInRangeLeft = IsInRangeMouseDownPoint(mouseX, 0);
            bool isInRangeRight = IsInRangeMouseDownPoint(mouseX, ac.AuxiliaryWidth);
            bool isInRangeTop = IsInRangeMouseDownPoint(mouseY, 0);
            bool isInRangeBottom = IsInRangeMouseDownPoint(mouseY, ac.AuxiliaryHeight);

            // 補助線の4隅の点のいずれか
            if (isInRangeLeft)
            {
                if (isInRangeTop)
                {
                    return KindMouseDownPoint.AuxiliaryLineLeftTop;
                }
                else if (isInRangeBottom)
                {
                    return KindMouseDownPoint.AuxiliaryLineLeftBottom;
                }
            }
            else if (isInRangeRight)
            {
                if (isInRangeTop)
                {
                    return KindMouseDownPoint.AuxiliaryLineRightTop;
                }
                else if (isInRangeBottom)
                {
                    return KindMouseDownPoint.AuxiliaryLineRightBottom;
                }
            }

            // 4隅の点ではないが、補助線の内側
            if ((0 < mouseX && mouseX < ac.AuxiliaryWidth) &&
                (0 < mouseY && mouseY < ac.AuxiliaryHeight))
            {
                return KindMouseDownPoint.AuxiliaryLineInside;
            }

            return KindMouseDownPoint.Else;
        }

        private bool IsInRangeMouseDownPoint(int mouseDownPoint, int basePoint)
        {
            int minusBasePoint = basePoint - Constant.MouseDownPointMargin;
            int plusBasePoint = basePoint + Constant.MouseDownPointMargin;
            return (minusBasePoint < mouseDownPoint && mouseDownPoint < plusBasePoint);
        }
    }
}
