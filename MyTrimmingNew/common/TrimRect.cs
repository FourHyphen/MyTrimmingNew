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
            if (!IsYInsideRectLineLeft(x, y))
            {
                return false;
            }

            if (!IsYInsideRectLineRight(x, y))
            {
                return false;
            }

            if (!IsYInsideRectLineTop(x, y))
            {
                return false;
            }

            if (!IsYInsideRectLineBottom(x, y))
            {
                return false;
            }

            return true;
        }

        private bool IsYInsideRectLineLeft(int x, int y)
        {
            int lineY = (int)(LineLeftA * x + LineLeftB);
            if (LineLeftA > 0.0)
            {
                return (lineY > y);
            }
            else
            {
                return (lineY < y);
            }
        }

        private bool IsYInsideRectLineRight(int x, int y)
        {
            int lineY = (int)(LineRightA * x + LineRightB);
            if (LineRightA > 0.0)
            {
                return (lineY < y);
            }
            else
            {
                return (lineY > y);
            }
        }

        private bool IsYInsideRectLineTop(int x, int y)
        {
            int lineY = (int)(LineTopA * x + LineTopB);
            return (lineY < y);
        }

        private bool IsYInsideRectLineBottom(int x, int y)
        {
            int lineY = (int)(LineBottomA * x + LineBottomB);
            return (lineY > y);
        }

        private double CalcLineA(Point p1, Point p2)
        {
            double x = (double)(p2.X - p1.X);
            double y = (double)(p2.Y - p1.Y);

            if (x == 0.0)
            {
                // 直線が x = 定数 となる場合、aを十分大きな値にした直線で近似する
                return (y / 1e-4);
            }
            else
            {
                return (y / x);
            }
        }

        private double CalcLineB(int x, int y, double a)
        {
            return y - a * x;
        }
    }
}
