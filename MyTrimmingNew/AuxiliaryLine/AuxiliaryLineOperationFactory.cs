using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineOperationFactory
    {
        public AuxiliaryLineCommand Create(AuxiliaryController ac)
        {
            // キー操作を想定(始点がない)
            return new AuxiliaryLineMove(ac);
        }

        public AuxiliaryLineCommand Create(AuxiliaryController ac,
                                           int degree)
        {
            return new AuxiliaryLineRotate(ac, degree);
        }

        public AuxiliaryLineCommand Create(AuxiliaryController ac, 
                                           Point coordinateRelatedAuxiliaryLine)
        {
            Mouse.KindMouseDownAuxiliaryLineArea area = Mouse.GetKindMouseDownAuxiliaryLineArea(ac,
                                                                                                coordinateRelatedAuxiliaryLine);

            if (area == Mouse.KindMouseDownAuxiliaryLineArea.Else)
            {
                return new AuxiliaryLineNoneOperation(ac);
            }
            else if (area == Mouse.KindMouseDownAuxiliaryLineArea.Inside)
            {
                return new AuxiliaryLineMove(ac, coordinateRelatedAuxiliaryLine);
            }
            else
            {
                return new AuxiliaryLineChangeSize(ac, coordinateRelatedAuxiliaryLine);
            }
        }
    }
}
