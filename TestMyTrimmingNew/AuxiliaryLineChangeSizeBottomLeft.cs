using System.Windows;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineChangeSizeBottomLeft : AuxiliaryLineChangeSizeTemplate
    {
        public override double GetMouseUpX(AuxiliaryController ac,
                                           int changeSizeWidthPixel)
        {
            // 矩形拡大のために動かす方向 = Left値が減る方向
            return ac.AuxiliaryLeftRelativeImage - (double)changeSizeWidthPixel;
        }

        public override double GetMouseUpY(AuxiliaryController ac,
                                           int changeSizeHeightPixel)
        {
            return GetAuxiliaryHeight(ac) + (double)changeSizeHeightPixel;
        }

        public override Point GetMouseDownPoint(AuxiliaryController ac)
        {
            return new Point(0, (double)GetAuxiliaryHeight(ac));
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
            // 左下点操作の場合、原点はLeftだけ変わる
            return new AuxiliaryLineTestData(beforeLeftRelativeImage - changeSizeWidth,
                                             beforeTopRelativeImage,
                                             beforeWidth + changeSizeWidth,
                                             beforeHeight + changeSizeHeight);
        }
    }
}
