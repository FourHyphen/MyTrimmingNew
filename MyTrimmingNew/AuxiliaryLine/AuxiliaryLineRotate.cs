using System;
using System.Drawing;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineRotate : AuxiliaryLineCommand
    {
        private int Degree { get; set; }

        public AuxiliaryLineRotate(AuxiliaryController ac, int degree): base(ac)
        {
            Degree = degree;
        }

        public override AuxiliaryLineParameter ExecuteCore(object operation = null)
        {
            Point leftTop = AC.AuxiliaryLeftTop;
            Point leftBottom = AC.AuxiliaryLeftBottom;
            Point rightTop = AC.AuxiliaryRightTop;
            Point rightBottom = AC.AuxiliaryRightBottom;
            int centerX = CalcCenterX(leftTop, rightBottom);
            int centerY = CalcCenterY(leftBottom, rightTop);

            Point newLeftTop = CalcRotatePoint(leftTop, centerX, centerY, Degree);
            Point newLeftBottom = CalcRotatePoint(leftBottom, centerX, centerY, Degree);
            Point newRightTop = CalcRotatePoint(rightTop, centerX, centerY, Degree);
            Point newRightBottom = CalcRotatePoint(rightBottom, centerX, centerY, Degree);

            // degreeの通りに回転したら画像からはみ出る場合に回転しない
            if (IsOutOfRangeDisplayImage(newLeftTop) ||
                IsOutOfRangeDisplayImage(newLeftBottom) ||
                IsOutOfRangeDisplayImage(newRightTop) ||
                IsOutOfRangeDisplayImage(newRightBottom))
            {
                return AC.CloneParameter();
            }

            AuxiliaryLineParameter newParameter = AC.CloneParameter();
            newParameter.ReplaceParameter(newLeftTop,
                                          newLeftBottom,
                                          newRightTop,
                                          newRightBottom,
                                          newParameter.Degree + Degree);

            return newParameter;
        }

        private int CalcCenterX(Point p1, Point p2)
        {
            return (p1.X + p2.X) / 2;
        }

        private int CalcCenterY(Point p1, Point p2)
        {
            return (p1.Y + p2.Y) / 2;
        }

        private Point CalcRotatePoint(Point p, int centerX, int centerY, int degree)
        {
            double rad = ToRadian(degree);
            int x = p.X - centerX;
            int y = p.Y - centerY;
            double rotateX = x * Math.Cos(rad) - y * Math.Sin(rad);
            double rotateY = y * Math.Cos(rad) + x * Math.Sin(rad);
            return new Point((int)rotateX + centerX, (int)rotateY + centerY);
        }

        private double ToRadian(int degree)
        {
            return (double)degree * Math.PI / 180.0;
        }

        private bool IsOutOfRangeDisplayImage(Point p)
        {
            if (p.X < 0 || AC.DisplayImageWidth < p.X)
            {
                return true;
            }

            if (p.Y < 0 || AC.DisplayImageHeight < p.Y)
            {
                return true;
            }

            return false;
        }
    }
}
