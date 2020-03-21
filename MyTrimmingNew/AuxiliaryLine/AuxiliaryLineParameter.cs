using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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

            LeftTop = new Point(0, 0);
            LeftBottom = new Point(0, Height);
            RightTop = new Point(Width, 0);
            RightBottom = new Point(Width, Height);
            Thickness = thickness;
            Degree = 0;
        }

        private AuxiliaryLineParameter(AuxiliaryLineParameter copy)
        {
            ImageWidth = copy.ImageWidth;
            ImageHeight = copy.ImageHeight;
            Width = copy.Width;
            Height = copy.Height;
            LeftTop = copy.LeftTop;
            LeftBottom = copy.LeftBottom;
            RightTop = copy.RightTop;
            RightBottom = copy.RightBottom;
            Ratio = copy.Ratio;
            Thickness = copy.Thickness;
            Degree = copy.Degree;
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

        private int Width { get; set; }

        private int Height { get; set; }

        private Point _leftTop = new Point();

        public Point LeftTop
        {
            get
            {
                return _leftTop;
            }
            private set
            {
                _leftTop = value;
            }
        }

        private Point _leftBottom = new Point();

        public Point LeftBottom
        {
            get
            {
                return _leftBottom;
            }
            private set
            {
                _leftBottom = value;
            }
        }

        private Point _rightTop = new Point();

        public Point RightTop
        {
            get
            {
                return _rightTop;
            }
            private set
            {
                _rightTop = value;
            }
        }

        private Point _rightBottom = new Point();

        public Point RightBottom
        {
            get
            {
                return _rightBottom;
            }
            private set
            {
                _rightBottom = value;
            }
        }

        public int Top
        {
            get
            {
                if (LeftTop.Y < RightTop.Y)
                {
                    return (int)LeftTop.Y;
                }
                else
                {
                    return (int)RightTop.Y;
                }
            }
        }

        public int Left
        {
            get
            {
                if (LeftTop.X < LeftBottom.X)
                {
                    return (int)LeftTop.X;
                }
                else
                {
                    return (int)LeftBottom.X;
                }
            }
        }


        public int Bottom
        {
            get
            {
                if (LeftBottom.Y < RightBottom.Y)
                {
                    return (int)RightBottom.Y;
                }
                else
                {
                    return (int)LeftBottom.Y;
                }
            }
        }

        public int Right
        {
            get
            {
                if (RightTop.X < RightBottom.X)
                {
                    return (int)RightBottom.X;
                }
                else
                {
                    return (int)RightTop.X;
                }
            }
        }

        public double? Ratio { get; private set; }

        public int Thickness { get; private set; }

        private int _degree;

        public int Degree
        { 
            get
            {
                return _degree;
            }
            set
            {
                if (IsInRangeAuxiliaryLineAfterRotate(value))
                {
                    _degree = value;
                }
            }
        }

        public void Move(int moveXPixel, int moveYPixel)
        {
            MoveWidth(moveXPixel);
            MoveHeight(moveYPixel);
        }

        public void MoveWidth(int moveXPixel)
        {
            int moveX = FitInRangeImageMovingX(moveXPixel);
            _leftTop.X += moveX;
            _leftBottom.X += moveX;
            _rightTop.X += moveX;
            _rightBottom.X += moveX;
        }

        public void MoveHeight(int moveYPixel)
        {
            int moveY = FitInRangeImageMovingY(moveYPixel);
            _leftTop.Y += moveY;
            _leftBottom.Y += moveY;
            _rightTop.Y += moveY;
            _rightBottom.Y += moveY;
        }

        public void ReplacePoint(Point newLeftTop,
                                 Point newLeftBottom,
                                 Point newRightTop,
                                 Point newRightBottom)
        {
            LeftTop = newLeftTop;
            LeftBottom = newLeftBottom;
            RightTop = newRightTop;
            RightBottom = newRightBottom;
        }

        private int FitInRangeImageMovingX(int moveXPixel)
        {
            // TODO: degree対応
            int newLeft = Left + moveXPixel;
            int newRight = Right + moveXPixel;
            int minLeft = 0 - Thickness + 1;
            int maxRight = ImageWidth - Thickness + 1;
            if (newLeft < minLeft)
            {
                if (moveXPixel > 0)
                {
                    return Left - Thickness + 1;
                }
                else
                {
                    return -Left - Thickness + 1;
                }
            }
            else if (maxRight < newRight)
            {
                return (maxRight - Right - Thickness + 1);
            }

            return moveXPixel;
        }

        private int FitInRangeImageMovingY(int moveYPixel)
        {
            // TODO: degree対応
            int newTop = Top + moveYPixel;
            int newBottom = Bottom + moveYPixel;
            int minTop = 0 - Thickness + 1;
            int maxBottom = ImageHeight - Thickness + 1;
            if (newTop < minTop)
            {
                if (moveYPixel > 0)
                {
                    return Top - Thickness + 1;
                }
                else
                {
                    return -Top - Thickness + 1;
                }
            }
            else if (maxBottom < newBottom)
            {
                return (maxBottom - Bottom - Thickness + 1);
            }

            return moveYPixel;
        }

        private bool IsInRangeAuxiliaryLineAfterRotate(int newDegree)
        {
            // TODO: 実装
            // degreeの通りに回転したら画像からはみ出る場合、falseを返す
            // 回転中心は矩形中心とする

            return true;
        }
    }
}
