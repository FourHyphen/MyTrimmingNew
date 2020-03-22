using System;

namespace MyTrimmingNew.AuxiliaryLine
{
    abstract class AuxiliaryLineChangeSizeTemplate
    {
        protected AuxiliaryController AC { get; set; }

        public AuxiliaryLineChangeSizeTemplate(AuxiliaryController ac)
        {
            AC = ac;
        }

        public AuxiliaryLineParameter Execute(int changeWidth, int changeHeight)
        {
            AuxiliaryLineParameter nowParameter = AC.CloneParameter();
            AuxiliaryLineParameter newParameter = null;

            // 矩形のRatioに合わせたマウス移動距離を求め、その通りにサイズを変更する
            int changeSizeWidth = changeWidth;
            int changeSizeHeight = changeHeight;
            if (BaseWidthWhenChangeSize(changeSizeWidth, changeSizeHeight))
            {
                newParameter = GetNewAuxiliaryLineParameterBaseWidth(changeSizeWidth, changeSizeHeight);
            }
            else
            {
                newParameter = GetNewAuxiliaryLineParameterBaseHeight(changeSizeWidth, changeSizeHeight);
            }

            // 原点が変わるような操作の場合、サイズ変更しない
            if (WillChangeAuxilirayOrigin(newParameter.Left, newParameter.Top, newParameter.Right, newParameter.Bottom))
            {
                return nowParameter;
            }

            return newParameter;
        }

        public abstract bool WillChangeAuxilirayOrigin(int newLeft, int newTop, int newRight, int newBottom);

        public abstract AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseWidth(int changeSizeWidth, int changeSizeHeight);

        public abstract AuxiliaryLineParameter GetNewAuxiliaryLineParameterBaseHeight(int changeSizeWidth, int changeSizeHeight);

        private bool BaseWidthWhenChangeSize(int willChangeWidth, int willChangeHeight)
        {
            int baseWidthChangeHeight = CalcHeightChangeSize(willChangeWidth, willChangeHeight);
            return (Math.Abs(baseWidthChangeHeight) > Math.Abs(willChangeHeight));
        }

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
