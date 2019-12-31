﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTrimmingNew
{
    /// <summary>
    /// TODO: ちょっといろいろPublicにし過ぎでは？
    /// </summary>
    public class AuxiliaryController : Subject
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

        public int AuxiliaryWidth { get; set; }

        public int AuxiliaryHeight { get; set; }

        public double AuxiliaryRatio { get; private set; }

        public int DisplayImageWidth { get; private set; }

        public int DisplayImageHeight { get; private set; }

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

        private IAuxiliaryLineOperation AuxiliaryLineOperation { get; set; }

        public void SetEvent()
        {
            AuxiliaryLineOperation = new AuxiliaryLineOperationFactory().Create(this);
        }

        public void SetEvent(Point coordinateRelatedAuxiliaryLine)
        {
            AuxiliaryLineOperation = new AuxiliaryLineOperationFactory().Create(this, coordinateRelatedAuxiliaryLine);
        }

        public void PublishEvent(object operation)
        {
            if (AuxiliaryLineOperation == null)
            {
                return;
            }
            AuxiliaryLineOperation.Execute(operation);

            // 登録Observerに変更を通知
            Notify();
        }
    }
}
