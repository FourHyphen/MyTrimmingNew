using System.Windows;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineChangeSizeBottomLeft : AuxiliaryLineChangeSizeTemplate
    {
        public override double GetMouseUpX(AuxiliaryController ac,
                                           int mouseMoveWidthPixel)
        {
            return ac.AuxiliaryLeftRelativeImage - (double)mouseMoveWidthPixel;
        }

        public override double GetMouseUpY(AuxiliaryController ac,
                                           int mouseMoveHeightPixel)
        {
            return ac.AuxiliaryHeight + (double)mouseMoveHeightPixel;
        }

        public override Point GetMouseDownPoint(AuxiliaryController ac)
        {
            return new Point(0, (double)ac.AuxiliaryHeight);
        }

        public override int GetMaxChangeSizeWidth(AuxiliaryController ac,
                                                  int beforeWidth,
                                                  int beforeLeftRelativeImage)
        {
            return beforeLeftRelativeImage - Common.AuxiliaryLineThickness + 1;
        }

        public override int GetMaxChangeSizeHeight(AuxiliaryController ac,
                                                   int beforeHeight,
                                                   int beforeTopRelativeImage)
        {
            return ac.DisplayImageHeight - beforeHeight - beforeTopRelativeImage - Common.AuxiliaryLineThickness + 1;
        }

        public override bool WillChangeAuxiliaryLineOrigin(int beforeWidth,
                                                           int beforeHeight,
                                                           int changeSizeWidth,
                                                           int changeSizeHeight)
        {
            if ((-changeSizeWidth > beforeWidth) || (-changeSizeHeight > beforeHeight))
            {
                return true;
            }
            return false;
        }

        public override AuxiliaryLineTestData GetAuxiliaryTestData(int beforeLeftRelativeImage,
                                                                   int beforeTopRelativeImage,
                                                                   int beforeWidth,
                                                                   int beforeHeight,
                                                                   int changeSizeWidth,
                                                                   int changeSizeHeight)
        {
            return new AuxiliaryLineTestData(beforeLeftRelativeImage - changeSizeWidth,
                                             beforeTopRelativeImage,
                                             beforeWidth + changeSizeWidth,
                                             beforeHeight + changeSizeHeight);
        }
    }
}
