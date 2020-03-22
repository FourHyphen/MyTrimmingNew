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
            // 回転を想定
            return new AuxiliaryLineRotate(ac, degree);
        }

        public AuxiliaryLineCommand Create(AuxiliaryController ac, 
                                           Point coordinateRelatedAuxiliaryLine)
        {
            // マウス操作を想定
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
