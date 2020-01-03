using System;
using System.Windows;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    abstract class AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineTestData ChangeSize(AuxiliaryController ac,
                                                int mouseMoveWidthPixel,
                                                int mouseMoveHeightPixel,
                                                bool isWidthMuchLongerThanHeight)
        {
            // 操作前の値を保持
            int beforeLeftRelativeImage = ac.AuxiliaryLeftRelativeImage;
            int beforeTopRelativeImage = ac.AuxiliaryTopRelativeImage;
            int beforeWidth = ac.AuxiliaryWidth;
            int beforeHeight = ac.AuxiliaryHeight;

            // 操作
            double mouseUpX = GetMouseUpX(ac, mouseMoveWidthPixel);
            double mouseUpY = GetMouseUpY(ac, mouseMoveHeightPixel);
            Point mouseDown = GetMouseDownPoint(ac);
            Point mouseUp = new Point(mouseUpX, mouseUpY);
            ac.SetEvent(mouseDown);
            ac.PublishEvent(mouseUp);

            // X方向操作距離とY方向操作距離を、矩形の縦横比率に合わせる
            int changeSizeWidth = GetInitChangeSizeWidth(mouseMoveWidthPixel);
            int changeSizeHeight = GetInitChangeSizeHeight(mouseMoveHeightPixel);
            if (isWidthMuchLongerThanHeight)
            {
                changeSizeHeight = (int)Math.Round((double)changeSizeWidth / ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                changeSizeWidth = (int)Math.Round((double)changeSizeHeight * ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }

            int maxChangeSizeWidth = GetMaxChangeSizeWidth(ac, beforeWidth, beforeLeftRelativeImage);
            int maxChangeHeight = GetMaxChangeSizeHeight(ac, beforeHeight, beforeTopRelativeImage);
            if (WillChangeAuxiliaryLineOrigin(beforeWidth, beforeHeight, changeSizeWidth, changeSizeHeight))
            {
                // 原点が変わるようなサイズ変更が要求されても、サイズ変更しない
                changeSizeWidth = 0;
                changeSizeHeight = 0;
            }
            else if (changeSizeWidth > maxChangeSizeWidth || changeSizeHeight > maxChangeHeight)
            {
                // 画像からはみ出るようなサイズ変更が要求された場合、代わりに画像一杯まで広げる
                if (isWidthMuchLongerThanHeight)
                {
                    changeSizeWidth = maxChangeSizeWidth;
                    changeSizeHeight = (int)Math.Round((double)changeSizeWidth / ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    changeSizeHeight = maxChangeHeight;
                    changeSizeWidth = (int)Math.Round((double)changeSizeHeight * ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
                }
            }

            return GetAuxiliaryTestData(beforeLeftRelativeImage,
                                        beforeTopRelativeImage,
                                        beforeWidth,
                                        beforeHeight,
                                        changeSizeWidth,
                                        changeSizeHeight);
        }

        public abstract double GetMouseUpX(AuxiliaryController ac,
                                           int mouseMoveWidthPixel);

        public abstract double GetMouseUpY(AuxiliaryController ac,
                                           int mouseMoveHeightPixel);

        public abstract Point GetMouseDownPoint(AuxiliaryController ac);

        public abstract int GetInitChangeSizeWidth(int changeSizeWidth);

        public abstract int GetInitChangeSizeHeight(int changeSizeHeight);

        public abstract int GetMaxChangeSizeWidth(AuxiliaryController ac,
                                                  int beforeWidth,
                                                  int beforeLeftRelativeImage);

        public abstract int GetMaxChangeSizeHeight(AuxiliaryController ac,
                                                   int beforeHeight,
                                                   int beforeTopRelativeImage);

        public abstract bool WillChangeAuxiliaryLineOrigin(int beforeWidth,
                                                           int beforeHeight,
                                                           int changeSizeWidth,
                                                           int changeSizeHeight);

        public abstract AuxiliaryLineTestData GetAuxiliaryTestData(int beforeLeftRelativeImage,
                                                                   int beforeTopRelativeImage,
                                                                   int beforeWidth,
                                                                   int beforeHeight,
                                                                   int changeSizeWidth,
                                                                   int changeSizeHeight);

    }
}
