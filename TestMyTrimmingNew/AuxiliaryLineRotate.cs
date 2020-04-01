using System;
using System.Drawing;
using MyTrimmingNew;
using MyTrimmingNew.AuxiliaryLine;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineRotate
    {
        public AuxiliaryLineTestData Execute(AuxiliaryController ac, int degree)
        {
            Point leftTop = ac.AuxiliaryLeftTop;
            Point leftBottom = ac.AuxiliaryLeftBottom;
            Point rightTop = ac.AuxiliaryRightTop;
            Point rightBottom = ac.AuxiliaryRightBottom;
            int centerX = CalcCenterX(leftTop, rightBottom);
            int centerY = CalcCenterY(leftBottom, rightTop);

            // テストデータ作成
            Point newLeftTop = CalcRotatePoint(leftTop, centerX, centerY, degree);
            Point newLeftBottom = CalcRotatePoint(leftBottom, centerX, centerY, degree);
            Point newRightTop = CalcRotatePoint(rightTop, centerX, centerY, degree);
            Point newRightBottom = CalcRotatePoint(rightBottom, centerX, centerY, degree);

            AuxiliaryLineParameter before = ac.CloneParameter();
            AuxiliaryLineTestData testData =  new AuxiliaryLineTestData(newLeftTop, newLeftBottom, newRightTop, newRightBottom, degree);

            if (IsOutOfRangeImagePoint(newLeftTop, ac) ||
                IsOutOfRangeImagePoint(newLeftBottom, ac) ||
                IsOutOfRangeImagePoint(newRightTop, ac) ||
                IsOutOfRangeImagePoint(newRightBottom, ac))
            {
                testData = new AuxiliaryLineTestData(before);
            }

            // 回転実施
            ac.SetEvent(degree);
            ac.PublishEvent(null);

            return testData;
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

        private bool IsOutOfRangeImagePoint(Point p, AuxiliaryController ac)
        {
            if (p.X < 0 || ac.DisplayImageWidth < p.X)
            {
                return true;
            }

            if (p.Y < 0 || ac.DisplayImageHeight < p.Y)
            {
                return true;
            }

            return false;
        }
    }
}
