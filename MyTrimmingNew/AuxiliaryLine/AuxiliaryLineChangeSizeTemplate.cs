﻿using System;
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

        public AuxiliaryLineParameter Execute(int changeWidth, int changeHeight)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // 矩形のRatioに合わせたマウス移動距離を求め、その通りにサイズを変更する
            int changeSizeWidth = changeWidth;
            int changeSizeHeight = changeHeight;
            if (BaseWidthWhenChangeSize(changeSizeWidth, changeSizeHeight))
            {
                changeSizeHeight = CalcAuxiliaryLineHeight(changeSizeHeight, changeSizeWidth);
            }
            else
            {
                changeSizeWidth = CalcAuxiliaryLineWidth(changeSizeWidth, changeSizeHeight);
            }

            int maxChangeSizeWidth = GetMaxChangeSizeWidth();
            int maxChangeSizeHeight = GetMaxChangeSizeHeight();

            // 原点が変わるような操作の場合、サイズ変更しない
            if (WillChangeAuxilirayOrigin(changeSizeWidth, changeSizeHeight))
            {
                return newParameter;
            }
            // 画像からはみ出るような変形の場合、画像一杯までの変形に制限する
            else if (changeSizeWidth > maxChangeSizeWidth)
            {
                changeSizeWidth = maxChangeSizeWidth;
                changeSizeHeight = CalcAuxiliaryLineHeight(changeSizeHeight, changeSizeWidth);
            }
            else if (changeSizeHeight > maxChangeSizeHeight)
            {
                changeSizeHeight = maxChangeSizeHeight;
                changeSizeWidth = CalcAuxiliaryLineWidth(changeSizeWidth, changeSizeHeight);
            }

            // 新しいパラメーターを返す
            int newWidth = GetNewAuxiliaryWidth(changeSizeWidth);
            int newHeight = GetNewAuxiliaryHeight(changeSizeHeight);
            int newLeft = GetNewAuxiliaryLeft(changeSizeWidth);
            int newTop = GetNewAuxiliaryTop(changeSizeHeight);
            newParameter.Width = newWidth;
            newParameter.Height = newHeight;
            newParameter.Top = newTop;
            newParameter.Left = newLeft;

            return newParameter;
        }

        public abstract int GetMaxChangeSizeWidth();

        public abstract int GetMaxChangeSizeHeight();

        public abstract bool WillChangeAuxilirayOrigin(int changeSizeWidth, int changeSizeHeight);

        public abstract int GetNewAuxiliaryLeft(int changeSizeWidth);

        public abstract int GetNewAuxiliaryTop(int changeSizeHeight);

        public abstract int GetNewAuxiliaryWidth(int changeSizeWidth);

        public abstract int GetNewAuxiliaryHeight(int changeSizeHeight);

        private bool BaseWidthWhenChangeSize(int willChangeWidth, int willChangeHeight)
        {
            int baseWidthChangeHeight = CalcAuxiliaryLineHeight(willChangeHeight, willChangeWidth);
            return (Math.Abs(baseWidthChangeHeight) > Math.Abs(willChangeHeight));
        }

        private int CalcAuxiliaryLineWidth(int nowAuxiliaryWidth, int newAuxiliaryHeight)
        {
            if(AC.AuxiliaryRatio == null)
            {
                return nowAuxiliaryWidth;
            }
            else
            {
                return CalcAuxiliaryLineWidthWithFitRatio(newAuxiliaryHeight, (double)AC.AuxiliaryRatio);
            }
        }

        private int CalcAuxiliaryLineWidthWithFitRatio(int newAuxiliaryHeight, double auxiliaryRatio)
        {
            double newWidth = (double)newAuxiliaryHeight * auxiliaryRatio;
            return (int)Math.Round(newWidth, 0, MidpointRounding.AwayFromZero);
        }

        private int CalcAuxiliaryLineHeight(int nowAuxiliaryHeight, int newAuxiliaryWidth)
        {
            if (AC.AuxiliaryRatio == null)
            {
                return nowAuxiliaryHeight;
            }
            else
            {
                return CalcAuxiliaryLineHeightWithFitRatio(newAuxiliaryWidth, (double)AC.AuxiliaryRatio);
            }
        }

        private int CalcAuxiliaryLineHeightWithFitRatio(int newAuxiliaryWidth, double auxiliaryRatio)
        {
            double newHeight = (double)newAuxiliaryWidth / auxiliaryRatio;
            return (int)Math.Round(newHeight, 0, MidpointRounding.AwayFromZero);
        }
    }
}
