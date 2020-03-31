using System;
using System.Drawing;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineRotate
    {
        public AuxiliaryLineTestData Execute(AuxiliaryController ac, int degree)
        {
            int left = ac.AuxiliaryLeft;
            int top = ac.AuxiliaryTop;

            Point newLeftTop = CalcRotatePoint(ac.AuxiliaryLeftTop, degree);
            Point newLeftBottom = CalcRotatePoint(ac.AuxiliaryLeftBottom, degree);
            Point newRightTop = CalcRotatePoint(ac.AuxiliaryRightTop, degree);
            Point newRightBottom = CalcRotatePoint(ac.AuxiliaryRightBottom, degree);

            return new AuxiliaryLineTestData(newLeftTop, newLeftBottom, newRightTop, newRightBottom, degree);
        }

        private Point CalcRotatePoint(Point p, int degree)
        {
            double rad = ToRadian(degree);
            double rotateX = p.X * Math.Cos(rad) - p.Y * Math.Sin(rad);
            double rotateY = p.Y * Math.Cos(rad) + p.X * Math.Sin(rad);
            return new Point((int)rotateX, (int)rotateY);
        }

        private double ToRadian(int degree)
        {
            return (double)degree * Math.PI / 180.0;
        }
    }
}
