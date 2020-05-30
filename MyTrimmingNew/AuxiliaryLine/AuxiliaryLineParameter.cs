using System;
using System.Drawing;
using MyTrimmingNew.common;

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

        public int Degree { get; private set; }

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
            FitRatio(LeftTop, LeftBottom, RightTop, RightBottom);
        }

        public void ReplaceParameter(Point newLeftTop,
                                     Point newLeftBottom,
                                     Point newRightTop,
                                     Point newRightBottom,
                                     int degree)
        {
            Degree = degree;
            ReplacePoint(newLeftTop, newLeftBottom, newRightTop, newRightBottom);
        }

        private void FitRatio(Point leftTop,
                              Point leftBottom,
                              Point rightTop,
                              Point rightBottom)
        {
            if (Ratio == null)
            {
                return;
            }

            FitRatioCore(leftTop, leftBottom, rightTop, rightBottom);
        }

        private void FitRatioCore(Point leftTop,
                                  Point leftBottom,
                                  Point rightTop,
                                  Point rightBottom)
        {
            if (Degree == 0)
            {
                FitRatioDegree0(leftTop, leftBottom, rightTop, rightBottom);
            }
            else
            {
                FitRatioDegreeNot0(leftTop, leftBottom, rightTop, rightBottom);
            }
        }

        private void FitRatioDegree0(Point leftTop,
                                     Point leftBottom,
                                     Point rightTop,
                                     Point rightBottom)
        {
            // 左上点を基準にする
            if (rightTop.Y != leftTop.Y)
            {
                rightTop.Y = leftTop.Y;
            }
            if (leftBottom.X != leftTop.X)
            {
                leftBottom.X = leftTop.X;
            }

            int width = rightTop.X - leftTop.X;
            int height = leftBottom.Y - leftTop.Y;
            if (rightBottom.X != leftBottom.X + width)
            {
                rightBottom.X = leftBottom.X + width;
            }
            if (rightBottom.Y != rightTop.Y + height)
            {
                rightBottom.Y = rightTop.Y + height;
            }
        }

        private void FitRatioDegreeNot0(Point leftTop,
                                        Point leftBottom,
                                        Point rightTop,
                                        Point rightBottom)
        {
            // 左上点を基準にする
            int width = CalcBaseWidth(leftTop, rightTop);
            FitLeftBottom(leftTop, rightTop, leftBottom, width);
        }

        private int CalcBaseWidth(Point leftTop, Point rightTop)
        {
            return Common.CalcEuclideanDist(leftTop, rightTop);
        }

        private void FitLeftBottom(Point leftTop, Point rightTop, Point leftBottom, int baseWidth)
        {
            // 角度が90度か？
            bool orthogonal = Common.Are2LinesOrthogonal(leftTop, rightTop, leftBottom);
            if (orthogonal)
            {
                // 角度は問題ないのであとはheight
                int baseHeight = CalcBaseHeight(leftTop, leftBottom, baseWidth);
                if (leftTop.Y - leftBottom.Y != 0)
                {
                    // TODO
                }
            }
            else
            {
                // 角度もズレてる、TODO
            }
        }

        private int CalcBaseHeight(Point leftTop, Point leftBottom, int baseWidth)
        {
            int baseHeight = Common.CalcEuclideanDist(leftTop, leftBottom);
            int ratio = baseWidth / baseHeight;
            if (ratio != Ratio)
            {
                baseHeight = (int)((double)baseWidth / Ratio);
            }

            return baseHeight;
        }

        private int FitInRangeImageMovingX(int moveXPixel)
        {
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
    }
}
