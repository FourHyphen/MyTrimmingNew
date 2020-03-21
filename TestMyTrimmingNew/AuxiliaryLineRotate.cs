using System;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineRotate
    {
        public AuxiliaryLineTestData Execute(AuxiliaryController ac, int degree)
        {
            int left = ac.AuxiliaryLeftRelativeImage;
            int top = ac.AuxiliaryTopRelativeImage;

            int newLeft = CalcRotateX(left, top, degree);
            int newTop = CalcRotateY(left, top, degree);
            int width = ac.AuxiliaryRight - ac.AuxiliaryLeftRelativeImage;
            int height = ac.AuxiliaryBottom - ac.AuxiliaryTopRelativeImage;

            return new AuxiliaryLineTestData(newLeft, newTop, width, height);
        }

        private int CalcRotateX(int x, int y, int degree)
        {
            double rad = ToRadian(degree);
            double rotateX = x * Math.Cos(rad) - y * Math.Sin(rad);
            return (int)rotateX;
        }

        private int CalcRotateY(int x, int y, int degree)
        {
            double rad = ToRadian(degree);
            double rotateY = y * Math.Cos(rad) + x * Math.Sin(rad);
            return (int)rotateY;
        }

        private double ToRadian(int degree)
        {
            return (double)degree * Math.PI / 180.0;
        }
    }
}
