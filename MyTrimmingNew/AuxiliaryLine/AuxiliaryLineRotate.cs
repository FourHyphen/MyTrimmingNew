using System;
using System.Drawing;
using MyTrimmingNew.common;

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
            int centerX = Common.CalcCenterX(leftTop, rightBottom);
            int centerY = Common.CalcCenterY(leftBottom, rightTop);

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

        private Point CalcRotatePoint(Point p, int centerX, int centerY, int degree)
        {
            double rad = Common.ToRadian(degree);
            int x = p.X - centerX;
            int y = p.Y - centerY;
            double rotateX = x * Math.Cos(rad) - y * Math.Sin(rad);
            double rotateY = y * Math.Cos(rad) + x * Math.Sin(rad);
            return new Point((int)rotateX + centerX, (int)rotateY + centerY);
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
