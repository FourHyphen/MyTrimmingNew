using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew.AuxiliaryLine
{
    abstract class AuxiliaryLineChangeSizeTemplate
    {
        protected AuxiliaryController AC { get; set; }

        public AuxiliaryLineChangeSizeTemplate(AuxiliaryController ac)
        {
            AC = ac;
        }

        public void Execute(int changeWidth, int changeHeight)
        {
            // 矩形のRatioに合わせたマウス移動距離を求め、その通りにサイズを変更する
            int changeSizeWidth = changeWidth;
            int changeSizeHeight = changeHeight;
            if (BaseWidthWhenChangeSize(changeSizeWidth, changeSizeHeight))
            {
                changeSizeHeight = CalcAuxiliaryLineHeightWithFitRatio(changeSizeWidth);
            }
            else
            {
                changeSizeWidth = CalcAuxiliaryLineWidthWithFitRatio(changeSizeHeight);
            }

            int maxChangeSizeWidth = GetMaxChangeSizeWidth();
            int maxChangeSizeHeight = GetMaxChangeSizeHeight();

            // 原点が変わるような操作の場合、サイズ変更しない
            if (WillChangeAuxilirayOrigin(changeSizeWidth, changeSizeHeight))
            {
                return;
            }
            // 画像からはみ出るような変形の場合、画像一杯までの変形に制限する
            else if (changeSizeWidth > maxChangeSizeWidth)
            {
                changeSizeWidth = maxChangeSizeWidth;
                changeSizeHeight = CalcAuxiliaryLineHeightWithFitRatio(changeSizeWidth);
            }
            else if (changeSizeHeight > maxChangeSizeHeight)
            {
                changeSizeHeight = maxChangeSizeHeight;
                changeSizeWidth = CalcAuxiliaryLineWidthWithFitRatio(changeSizeHeight);
            }

            // 新しいパラメーターをセット
            SetAuxiliaryLeft(changeSizeWidth);
            SetAuxiliaryTop(changeSizeHeight);
            SetAuxiliaryWidth(changeSizeWidth);
            SetAuxiliaryHeight(changeSizeHeight);
        }

        public abstract int GetMaxChangeSizeWidth();

        public abstract int GetMaxChangeSizeHeight();

        public abstract bool WillChangeAuxilirayOrigin(int changeSizeWidth, int changeSizeHeight);

        public abstract void SetAuxiliaryLeft(int changeSizeWidth);

        public abstract void SetAuxiliaryTop(int changeSizeHeight);

        public abstract void SetAuxiliaryWidth(int changeSizeWidth);

        public abstract void SetAuxiliaryHeight(int changeSizeHeight);

        private bool BaseWidthWhenChangeSize(int willChangeWidth, int willChangeHeight)
        {
            int baseWidthChangeHeight = CalcAuxiliaryLineHeightWithFitRatio(willChangeWidth);
            return (Math.Abs(baseWidthChangeHeight) > Math.Abs(willChangeHeight));
        }

        private int CalcAuxiliaryLineWidthWithFitRatio(int newAuxiliaryHeight)
        {
            double newWidth = (double)newAuxiliaryHeight * AC.AuxiliaryRatio;
            return (int)Math.Round(newWidth, 0, MidpointRounding.AwayFromZero);
        }

        private int CalcAuxiliaryLineHeightWithFitRatio(int newAuxiliaryWidth)
        {
            double newHeight = (double)newAuxiliaryWidth / AC.AuxiliaryRatio;
            return (int)Math.Round(newHeight, 0, MidpointRounding.AwayFromZero);
        }
    }
}
