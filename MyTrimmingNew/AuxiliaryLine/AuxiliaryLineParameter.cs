using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew.AuxiliaryLine
{
    public class AuxiliaryLineParameter : ICloneable
    {
        public enum RatioType
        {
            W16H9,
            W4H3,
            W9H16,
            W1H1,
            NoDefined
        }

        public AuxiliaryLineParameter(int imageWidth,
                                      int imageHeight,
                                      RatioType ratioType,
                                      int thickness)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;

            int? widthRatio = WidthRatio(ratioType);
            int? heightRatio = HeightRatio(ratioType);

            if (widthRatio == null || heightRatio == null)
            {
                Ratio = null;
                Width = ImageWidth;
                Height = ImageHeight;
            }
            else
            {
                Ratio = (double)WidthRatio(ratioType) / (double)HeightRatio(ratioType);
                Width = ImageWidth;
                Height = (int)((double)ImageWidth / Ratio);

                // 矩形比率に合わせてサイズ算出後、画像からはみ出た場合に補正
                if (Height > ImageHeight)
                {
                    Height = ImageHeight;
                    Width = (int)((double)ImageHeight * Ratio);
                }
            }

            // 初期値は画像の原点に合わせる
            Left = 0;
            Top = 0;

            Thickness = thickness;
        }

        private AuxiliaryLineParameter(AuxiliaryLineParameter copy)
        {
            ImageWidth = copy.ImageWidth;
            ImageHeight = copy.ImageHeight;
            Width = copy.Width;
            Height = copy.Height;
            Top = copy.Top;
            Left = copy.Left;
            Ratio = copy.Ratio;
            Thickness = copy.Thickness;
        }

        public object Clone()
        {
            return new AuxiliaryLineParameter(this);
        }

        public static int? WidthRatio(RatioType type)
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

        public static int? HeightRatio(RatioType type)
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

        public int ImageWidth { get; private set; }

        public int ImageHeight { get; private set; }

        public int Width { get; set; }

        public int Height { get; set; }

        private int _top;

        public int Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = FitInRangeAuxiliaryTopRelativeImage(value);
            }
        }

        private int _left;

        public int Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = FitInRangeAuxiliaryLeftRelativeImage(value);
            }
        }

        public double? Ratio { get; private set; }

        public int Thickness { get; private set; }

        private int FitInRangeAuxiliaryTopRelativeImage(int toMoveTop)
        {
            if (toMoveTop < 0)
            {
                return 0;
            }

            int maxOriginTop = ImageHeight - Height - Thickness + 1;
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

            int maxOriginLeft = ImageWidth - Width - Thickness + 1;
            if (toMoveLeft > maxOriginLeft)
            {
                return maxOriginLeft;
            }

            return toMoveLeft;
        }
    }
}
