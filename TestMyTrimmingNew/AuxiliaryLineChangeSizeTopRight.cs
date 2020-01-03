using System.Windows;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineChangeSizeTopRight : AuxiliaryLineChangeSizeTemplate
    {
        public override double GetMouseUpX(AuxiliaryController ac,
                                           int changeSizeWidthPixel)
        {
            return ac.AuxiliaryWidth + (double)changeSizeWidthPixel;
        }

        public override double GetMouseUpY(AuxiliaryController ac,
                                           int changeSizeHeightPixel)
        {
            // 矩形拡大のために動かす方向 = Top値が減る方向
            return -(double)changeSizeHeightPixel;
        }

        public override Point GetMouseDownPoint(AuxiliaryController ac)
        {
            return new Point(ac.AuxiliaryWidth, 0);
        }

        public override int GetMaxChangeSizeWidth(AuxiliaryController ac,
                                                  int beforeWidth,
                                                  int beforeLeftRelativeImage)
        {
            return ac.DisplayImageWidth - beforeWidth - beforeLeftRelativeImage - Common.AuxiliaryLineThickness + 1;
        }

        public override int GetMaxChangeSizeHeight(AuxiliaryController ac,
                                                   int beforeHeight,
                                                   int beforeTopRelativeImage)
        {
            return beforeTopRelativeImage - Common.AuxiliaryLineThickness + 1;
        }

        public override bool WillChangeAuxiliaryLineOrigin(int beforeWidth,
                                                           int beforeHeight,
                                                           int changeSizeWidth,
                                                           int changeSizeHeight)
        {
            if ((changeSizeWidth < -beforeWidth) || (changeSizeHeight < -beforeHeight))
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
            // 右上点操作の場合、原点はTopだけ変わる
            return new AuxiliaryLineTestData(beforeLeftRelativeImage,
                                             beforeTopRelativeImage - changeSizeHeight,
                                             beforeWidth + changeSizeWidth,
                                             beforeHeight + changeSizeHeight);
        }
    }
}