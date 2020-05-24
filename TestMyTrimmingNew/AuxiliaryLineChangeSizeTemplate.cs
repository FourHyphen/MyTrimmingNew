using System;
using System.Windows;
using MyTrimmingNew;
using MyTrimmingNew.AuxiliaryLine;

namespace TestMyTrimmingNew
{
    abstract class AuxiliaryLineChangeSizeTemplate
    {
        protected AuxiliaryController AC { get; set; }

        public AuxiliaryLineChangeSizeTemplate(AuxiliaryController ac)
        {
            AC = ac;
        }

        public AuxiliaryLineTestData ChangeSize(int changeSizeWidthPixel,
                                                int changeSizeHeightPixel,
                                                bool isChangeSizeWidthMuchLongerThanChangeSizeHeight)
        {
            // 操作前の値を保持
            AuxiliaryLineParameter now = AC.CloneParameter();
            AuxiliaryLineParameter afterParameter = null;

            // 矩形操作前にサイズ変更後の各種パラメーターを求め、テストデータとする
            if (isChangeSizeWidthMuchLongerThanChangeSizeHeight)
            {
                afterParameter = GetNewAuxiliaryLineParameterBaseWidth(changeSizeWidthPixel, changeSizeHeightPixel);
            }
            else
            {
                afterParameter = GetNewAuxiliaryLineParameterBaseHeight(changeSizeWidthPixel, changeSizeHeightPixel);
            }

            // 原点が変わるようなサイズ変更が要求されても、サイズ変更しない
            if (WillChangeAuxiliaryOrigin(afterParameter.Left, afterParameter.Top, afterParameter.Right, afterParameter.Bottom))
            {
                afterParameter = now;
            }

            // 操作
            double mouseUpX = (double)GetMouseUpX(AC, changeSizeWidthPixel);
            double mouseUpY = (double)GetMouseUpY(AC, changeSizeHeightPixel);
            Point mouseDown = GetMouseDownPoint(AC);
            Point mouseUp = new Point(mouseUpX, mouseUpY);
            AC.SetEvent(mouseDown);
            AC.PublishEvent(mouseUp);

            return new AuxiliaryLineTestData(afterParameter);
        }

        public abstract int GetMouseUpX(AuxiliaryController ac,
                                        int changeSizeWidthPixel);

        public abstract int GetMouseUpY(AuxiliaryController ac,
                                        int changeSizeHeightPixel);

        public abstract Point GetMouseDownPoint(AuxiliaryController ac);

        public abstract AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseWidth(int changeSizeWidth, int changeSizeHeight);

        public abstract AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseHeight(int changeSizeWidth, int changeSizeHeight);

        public abstract bool WillChangeAuxiliaryOrigin(int newLeft, int newTop, int newRight, int newBottom);

        protected int CalcWidthChangeSize(int willChangeSizeWidth, int willChangeSizeHeight)
        {
            if (AC.AuxiliaryRatio == null)
            {
                return willChangeSizeWidth;
            }
            else
            {
                return CalcWidthChangeSizeBaseHeight(willChangeSizeHeight, (double)AC.AuxiliaryRatio);
            }
        }

        private int CalcWidthChangeSizeBaseHeight(int willChangeSizeHeight, double auxiliaryRatio)
        {
            double newWidth = (double)willChangeSizeHeight * auxiliaryRatio;
            return (int)Math.Round(newWidth, 0, MidpointRounding.AwayFromZero);
        }

        protected int CalcHeightChangeSize(int willChangeSizeWidth, int willChangeSizeHeight)
        {
            if (AC.AuxiliaryRatio == null)
            {
                return willChangeSizeHeight;
            }
            else
            {
                return CalcHeightChangeSizeBaseWidth(willChangeSizeWidth, (double)AC.AuxiliaryRatio);
            }
        }

        private int CalcHeightChangeSizeBaseWidth(int willChangeSizeWidth, double auxiliaryRatio)
        {
            double newHeight = (double)willChangeSizeWidth / auxiliaryRatio;
            return (int)Math.Round(newHeight, 0, MidpointRounding.AwayFromZero);
        }

        protected int GetMinLeft()
        {
            return (0 - AC.AuxiliaryLineThickness + 1);
        }

        protected int GetMinTop()
        {
            return (0 - AC.AuxiliaryLineThickness + 1);
        }

        protected int GetMaxRight()
        {
            return (AC.DisplayImageWidth - AC.AuxiliaryLineThickness + 1);
        }

        protected int GetMaxBottom()
        {
            return (AC.DisplayImageHeight - AC.AuxiliaryLineThickness + 1);
        }
    }
}
