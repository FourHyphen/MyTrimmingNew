using System;
using System.Windows;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    abstract class AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineTestData ChangeSize(AuxiliaryController ac,
                                                int changeSizeWidthPixel,
                                                int changeSizeHeightPixel,
                                                bool isChangeSizeWidthMuchLongerThanChangeSizeHeight)
        {
            // 操作前の値を保持
            int beforeLeftRelativeImage = ac.AuxiliaryLeftRelativeImage;
            int beforeTopRelativeImage = ac.AuxiliaryTopRelativeImage;
            int beforeWidth = GetAuxiliaryWidth(ac);
            int beforeHeight = GetAuxiliaryHeight(ac);

            // 操作
            double mouseUpX = GetMouseUpX(ac, changeSizeWidthPixel);
            double mouseUpY = GetMouseUpY(ac, changeSizeHeightPixel);
            Point mouseDown = GetMouseDownPoint(ac);
            Point mouseUp = new Point(mouseUpX, mouseUpY);
            ac.SetEvent(mouseDown);
            ac.PublishEvent(mouseUp);

            // X方向操作距離とY方向操作距離を、矩形の縦横比率に合わせる
            int changeSizeWidth = changeSizeWidthPixel;
            int changeSizeHeight = changeSizeHeightPixel;
            if (isChangeSizeWidthMuchLongerThanChangeSizeHeight)
            {
                changeSizeHeight = CalcAuxiliaryHeight(changeSizeHeight, changeSizeWidth, ac);
            }
            else
            {
                changeSizeWidth = CalcAuxiliaryWidth(changeSizeWidth, changeSizeHeight, ac);
            }

            // 画像からはみ出るようなサイズ変更が要求された場合、代わりに画像一杯まで広げる
            int maxChangeSizeWidth = GetMaxChangeSizeWidth(ac, beforeWidth, beforeLeftRelativeImage);
            int maxChangeHeight = GetMaxChangeSizeHeight(ac, beforeHeight, beforeTopRelativeImage);
            if (changeSizeWidth > maxChangeSizeWidth)
            {
                if (isChangeSizeWidthMuchLongerThanChangeSizeHeight)
                {
                    changeSizeWidth = maxChangeSizeWidth;
                    changeSizeHeight = CalcAuxiliaryHeight(changeSizeHeight, changeSizeWidth, ac);
                }
                else
                {
                    changeSizeHeight = maxChangeHeight;
                    changeSizeWidth = CalcAuxiliaryWidth(changeSizeWidth, changeSizeHeight, ac);
                }
            }
            if (changeSizeHeight > maxChangeHeight)
            {
                if (isChangeSizeWidthMuchLongerThanChangeSizeHeight)
                {
                    changeSizeHeight = maxChangeHeight;
                    changeSizeWidth = CalcAuxiliaryWidth(changeSizeWidth, changeSizeHeight, ac);
                }
                else
                {
                    changeSizeWidth = maxChangeSizeWidth;
                    changeSizeHeight = CalcAuxiliaryHeight(changeSizeWidth, changeSizeHeight, ac);
                }
            }
            // 補正してもなお画像からはみ出る場合あり、それを再度補正する
            if (changeSizeWidth > maxChangeSizeWidth)
            {
                changeSizeWidth = maxChangeSizeWidth;
                changeSizeHeight = CalcAuxiliaryHeight(changeSizeHeight, changeSizeWidth, ac);
            }

            if (WillChangeAuxiliaryLineOrigin(beforeWidth, beforeHeight, changeSizeWidth, changeSizeHeight))
            {
                // 原点が変わるようなサイズ変更が要求されても、サイズ変更しない
                changeSizeWidth = 0;
                changeSizeHeight = 0;
            }

            return GetAuxiliaryTestData(beforeLeftRelativeImage,
                                        beforeTopRelativeImage,
                                        beforeWidth,
                                        beforeHeight,
                                        changeSizeWidth,
                                        changeSizeHeight);
        }

        protected int GetAuxiliaryWidth(AuxiliaryController ac)
        {
            return ac.AuxiliaryRight - ac.AuxiliaryLeftRelativeImage - ac.AuxiliaryLineThickness + 1;
        }

        protected int GetAuxiliaryHeight(AuxiliaryController ac)
        {
            return ac.AuxiliaryBottom - ac.AuxiliaryTopRelativeImage - ac.AuxiliaryLineThickness + 1;
        }

        public abstract double GetMouseUpX(AuxiliaryController ac,
                                           int changeSizeWidthPixel);

        public abstract double GetMouseUpY(AuxiliaryController ac,
                                           int changeSizeHeightPixel);

        public abstract Point GetMouseDownPoint(AuxiliaryController ac);

        private int CalcAuxiliaryHeight(int nowHeight, int newWidth, AuxiliaryController ac)
        {
            if (ac.AuxiliaryRatio == null)
            {
                return nowHeight;
            }
            else
            {
                return (int)Math.Round((double)newWidth / (double)ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }
        }

        private int CalcAuxiliaryWidth(int nowWidth, int newHeight, AuxiliaryController ac)
        {
            if (ac.AuxiliaryRatio == null)
            {
                return nowWidth;
            }
            else
            {
                return (int)Math.Round((double)newHeight * (double)ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }
        }

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
