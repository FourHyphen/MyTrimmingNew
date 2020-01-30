using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyTrimmingNew.AuxiliaryLine;

namespace MyTrimmingNew
{
    /// <summary>
    /// TODO: ちょっといろいろPublicにし過ぎでは？
    /// </summary>
    public class AuxiliaryController : Subject
    {
        public enum RatioType
        {
            W16H9,
            W4H3,
            W9H16,
            W1H1,
            NoDefined
        }

        public int? WidthRatio(RatioType type)
        {
            if (type == RatioType.W16H9)
            {
                return 16;
            }
            else if (type == RatioType.W4H3)
            {
                return 4;
            }
            else if (type == RatioType.W1H1)
            {
                return 1;
            }
            else if (type == RatioType.W9H16)
            {
                return 9;
            }
            else
            {
                return null;
            }
        }

        public int? HeightRatio(RatioType type)
        {
            if (type == RatioType.W16H9)
            {
                return 9;
            }
            else if (type == RatioType.W4H3)
            {
                return 3;
            }
            else if (type == RatioType.W1H1)
            {
                return 1;
            }
            else if (type == RatioType.W9H16)
            {
                return 16;
            }
            else
            {
                return null;
            }
        }

        public AuxiliaryController(ImageController ic,
                                   RatioType ratioType = RatioType.W16H9,
                                   int auxiliaryLineThickness = 1)
        {
            ImageController = ic;
            int? widthRatio = WidthRatio(ratioType);
            int? heightRatio = HeightRatio(ratioType);

            if (widthRatio == null || heightRatio == null){
                AuxiliaryRatio = null;
                AuxiliaryWidth = ic.DisplayImageWidth;
                AuxiliaryHeight = ic.DisplayImageHeight;
            }
            else
            {
                AuxiliaryRatio = (double)WidthRatio(ratioType) / (double)HeightRatio(ratioType);
                AuxiliaryWidth = ic.DisplayImageWidth;
                AuxiliaryHeight = (int)((double)ic.DisplayImageWidth / AuxiliaryRatio);

                // 矩形比率に合わせてサイズ算出後、画像からはみ出た場合に補正
                if (AuxiliaryHeight > ic.DisplayImageHeight)
                {
                    AuxiliaryHeight = ic.DisplayImageHeight;
                    AuxiliaryWidth = (int)((double)ic.DisplayImageHeight * AuxiliaryRatio);
                }
            }

            // 初期値は画像の原点に合わせる
            AuxiliaryLeftRelativeImage = 0;
            AuxiliaryTopRelativeImage = 0;

            AuxiliaryLineThickness = auxiliaryLineThickness;
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

        public int AuxiliaryLineThickness { get; private set; }

        public int AuxiliaryWidth { get; set; }

        public int AuxiliaryHeight { get; set; }

        public double? AuxiliaryRatio { get; private set; }

        private ImageController ImageController { get; set; }

        public int DisplayImageWidth { get { return ImageController.DisplayImageWidth; } }

        public int DisplayImageHeight { get { return ImageController.DisplayImageHeight; } }

        private int FitInRangeAuxiliaryTopRelativeImage(int toMoveTop)
        {
            if (toMoveTop < 0)
            {
                return 0;
            }

            int maxOriginTop = DisplayImageHeight - AuxiliaryHeight - AuxiliaryLineThickness + 1;
            if (toMoveTop > maxOriginTop)
            {
                return maxOriginTop;
            }

            return toMoveTop;
        }

        private int FitInRangeAuxiliaryLeftRelativeImage(int toMoveLeft)
        {
            if (toMoveLeft < 0)
            {
                return 0;
            }

            int maxOriginLeft = DisplayImageWidth - AuxiliaryWidth - AuxiliaryLineThickness + 1;
            if (toMoveLeft > maxOriginLeft)
            {
                return maxOriginLeft;
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
