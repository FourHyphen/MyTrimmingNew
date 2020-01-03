using System;
using System.Windows;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineChangeSizeBottomRight : AuxiliaryLineChangeSizeTemplate
    {
        public override double GetMouseUpX(AuxiliaryController ac,
                                           int mouseMoveWidthPixel)
        {
            return (ac.AuxiliaryWidth + (double)mouseMoveWidthPixel);
        }

        public override double GetMouseUpY(AuxiliaryController ac, int mouseMoveHeightPixel)
        {
            return (ac.AuxiliaryHeight + (double)mouseMoveHeightPixel);
        }

        public override Point GetMouseDownPoint(AuxiliaryController ac)
        {
            return new Point((double)ac.AuxiliaryWidth, (double)ac.AuxiliaryHeight);
        }

        public override int GetInitChangeSizeWidth(int changeSizeWidth)
        {
            return changeSizeWidth;
        }

        public override int GetInitChangeSizeHeight(int changeSizeHeight)
        {
            return changeSizeHeight;
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
            return ac.DisplayImageHeight - beforeHeight - beforeTopRelativeImage - Common.AuxiliaryLineThickness + 1;
        }

        public override bool WillChangeAuxiliaryLineOrigin(int beforeWidth,
                                                           int beforeHeight,
                                                           int changeSizeWidth,
                                                           int changeSizeHeight)
        {
            if (((beforeWidth + changeSizeWidth) < 0) || ((beforeHeight + changeSizeHeight) < 0))
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
            // 右下点操作であれば、原点は変わらない
            return new AuxiliaryLineTestData(beforeLeftRelativeImage,
                                         beforeTopRelativeImage,
                                         beforeWidth + changeSizeWidth,
                                         beforeHeight + changeSizeHeight);
        }

    }
}
