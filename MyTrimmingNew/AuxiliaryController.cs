﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTrimmingNew
{
    public class AuxiliaryController
    {
        public AuxiliaryController(ImageController ic,
                                   int widthRatio = 16,
                                   int heightRatio = 9,
                                   int auxiliaryLineThickness = 1)
        {
            AuxiliaryRatio = (double)widthRatio / (double)heightRatio;

            if (ic.DisplayImageWidth > ic.DisplayImageHeight)
            {
                AuxiliaryWidth = ic.DisplayImageWidth;
                AuxiliaryHeight = (int)((double)ic.DisplayImageWidth / AuxiliaryRatio);
            }
            else
            {
                AuxiliaryWidth = (int)((double)ic.DisplayImageHeight / AuxiliaryRatio);
                AuxiliaryHeight = ic.DisplayImageHeight;
            }

            DisplayImageWidth = ic.DisplayImageWidth;
            DisplayImageHeight = ic.DisplayImageHeight;

            // 初期値は画像の原点に合わせる
            AuxiliaryLeftRelativeImage = 0;
            AuxiliaryTopRelativeImage = 0;

            // TODO: 線の太さを変更できるようにする
            AuxiliaryLineThickness = 1;
        }

        private int _auxiliaryLeftRelativeImage;

        public int AuxiliaryLeftRelativeImage
        {
            get
            {
                return _auxiliaryLeftRelativeImage;
            }
            set
            {
                _auxiliaryLeftRelativeImage = FitInRangeAuxiliaryLeftRelativeImage(value);
            }
        }

        private int _auxiliaryTopRelativeImage;

        public int AuxiliaryTopRelativeImage
        {
            get
            {
                return _auxiliaryTopRelativeImage;
            }
            set
            {
                _auxiliaryTopRelativeImage = FitInRangeAuxiliaryTopRelativeImage(value);
            }
        }

        private int AuxiliaryLineThickness { get; set; }

        public int AuxiliaryWidth { get; private set; }

        public int AuxiliaryHeight { get; private set; }

        public double AuxiliaryRatio { get; private set; }

        public int DisplayImageWidth { get; private set; }

        public int DisplayImageHeight { get; private set; }

        public string GetLineSizeString()
        {
            return "矩形: 横" + AuxiliaryWidth.ToString() + "x縦" + AuxiliaryHeight.ToString();
        }

        private int FitInRangeAuxiliaryTopRelativeImage(int toMoveTop)
        {
            if (toMoveTop < 0)
            {
                return 0;
            }

            int maxOriginTop = DisplayImageHeight - AuxiliaryHeight - AuxiliaryLineThickness;
            if (toMoveTop > maxOriginTop)
            {
                return AuxiliaryTopRelativeImage;
            }

            return toMoveTop;
        }

        private int FitInRangeAuxiliaryLeftRelativeImage(int toMoveLeft)
        {
            if (toMoveLeft < 0)
            {
                return 0;
            }

            int maxOriginLeft = DisplayImageWidth - AuxiliaryWidth - AuxiliaryLineThickness;
            if (toMoveLeft > maxOriginLeft)
            {
                return AuxiliaryLeftRelativeImage;
            }

            return toMoveLeft;
        }

        public void ChangeSizeAuxiliaryLineWhereOperationBottomRight(int changeWidth, int changeHeight)
        {
            // widthとheight、基準にする変更サイズに合わせてRatioの通りにサイズを変更する
            int newWidth = AuxiliaryWidth + changeWidth;
            int newHeight = AuxiliaryHeight + changeHeight;
            if (BaseWidthWhenChangeSize(changeWidth, changeHeight))
            {
                newHeight = CalcAuxiliaryLineHeightWithFitRatio(newWidth);
            }
            else
            {
                newWidth = CalcAuxiliaryLineWidthWithFitRatio(newHeight);
            }

            // 右下点を思いっきり左や上に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if (newWidth < 0 || newHeight < 0)
            {
                return;
            }

            AuxiliaryWidth = newWidth;
            AuxiliaryHeight = newHeight;
        }

        private bool BaseWidthWhenChangeSize(int willChangeWidth, int willChangeHeight)
        {
            int baseWidthChangeHeight = CalcAuxiliaryLineHeightWithFitRatio(willChangeWidth);
            return (Math.Abs(baseWidthChangeHeight) > Math.Abs(willChangeHeight));
        }

        private int CalcAuxiliaryLineWidthWithFitRatio(int newAuxiliaryHeight)
        {
            double newWidth = (double)newAuxiliaryHeight * AuxiliaryRatio;
            return (int)Math.Round(newWidth, 0, MidpointRounding.AwayFromZero);
        }

        private int CalcAuxiliaryLineHeightWithFitRatio(int newAuxiliaryWidth)
        {
            double newHeight = (double)newAuxiliaryWidth / AuxiliaryRatio;
            return (int)Math.Round(newHeight, 0, MidpointRounding.AwayFromZero);
        }

        private IAuxiliaryLineOperation AuxiliaryLineOperation { get; set; }

        public void SetEvent()
        {
            AuxiliaryLineOperation = new AuxiliaryLineOperationFactory().Create(this);
        }

        public void SetEvent(Point pointRelatedAuxiliaryLine)
        {
            AuxiliaryLineOperation = new AuxiliaryLineOperationFactory().Create(this, pointRelatedAuxiliaryLine);
        }

        public void PublishEvent(object operation)
        {
            if (AuxiliaryLineOperation == null)
            {
                return;
            }
            AuxiliaryLineOperation.Execute(operation);
        }
    }
}
