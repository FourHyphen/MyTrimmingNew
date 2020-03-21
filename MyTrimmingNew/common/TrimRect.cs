using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyTrimmingNew.common
{
    class TrimRect
    {
        // y = a*x + b
        private double LineLeftA { get; set; }

        private double LineLeftB { get; set; }

        private double LineRightA { get; set; }

        private double LineRightB { get; set; }

        private double LineTopA { get; set; }

        private double LineTopB { get; set; }

        private double LineBottomA { get; set; }

        private double LineBottomB { get; set; }

        public TrimRect(Point leftTop,
                        Point rightTop,
                        Point rightBottom,
                        Point leftBottom)
        {
            LineLeftA = CalcLineA(leftTop, leftBottom);
            LineLeftB = CalcLineB(leftTop.X, leftTop.Y, LineLeftA);
            LineRightA = CalcLineA(rightTop, rightBottom);
            LineRightB = CalcLineB(rightTop.X, rightTop.Y, LineRightA);
            LineTopA = CalcLineA(rightTop, leftTop);
            LineTopB = CalcLineB(rightTop.X, rightTop.Y, LineTopA);
            LineBottomA = CalcLineA(rightBottom, leftBottom);
            LineBottomB = CalcLineB(rightBottom.X, rightBottom.Y, LineBottomA);
        }

        public bool IsInside(int x, int y)
        {
            if (!IsYBiggerThanLine(x, y, LineLeftA, LineLeftB))
            {
                return false;
            }

            if (IsYBiggerThanLine(x, y, LineRightA, LineRightB))
            {
                return false;
            }

            if (!IsYBiggerThanLine(x, y, LineTopA, LineTopB))
            {
                return false;
            }

            if (IsYBiggerThanLine(x, y, LineBottomA, LineBottomB))
            {
                return false;
            }

            return true;
        }

        private bool IsYBiggerThanLine(int x, int y, double a, double b)
        {
            int lineY = (int)(a * x + b);
            return (y > lineY);
        }

        private double CalcLineA(Point p1, Point p2)
        {
            double x = (double)(p2.X - p1.X);
            double y = (double)(p2.Y - p1.Y);

            return (y / x);
        }

        private double CalcLineB(int x, int y, double a)
        {
            return y - a * x;
        }
    }
}
