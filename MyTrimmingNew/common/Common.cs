using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew.common
{
    class Common
    {
        public static System.Windows.Point ToWindowsPoint(System.Drawing.Point p)
        {
            return new System.Windows.Point(p.X, p.Y);
        }

        public static System.Drawing.Point ToDrawingPoint(System.Windows.Point p)
        {
            return new System.Drawing.Point((int)p.X, (int)p.Y);
        }

        public static double ToRadian(int degree)
        {
            return (double)degree * Math.PI / 180.0;
        }

        public static int CalcEuclideanDist(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            double tmp = Math.Pow(p1.X - p2.X, 2.0) + Math.Pow(p1.Y - p2.Y, 2.0);
            return (int)Math.Sqrt(tmp);
        }

        public static int CalcCenterX(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            return (p1.X + p2.X) / 2;
        }

        public static int CalcCenterY(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            return (p1.Y + p2.Y) / 2;
        }

        /// <summary>
        /// 2点を通る直線の傾きを返す
        /// 直線が x = a となる場合、y = 1e+6 * x + b で代用する
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double CalcSlope(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            double numer = (double)(p2.X - p1.X);
            if (numer == 0.0)
            {
                return 1e+6;
            }

            return (double)(p2.Y - p1.Y) / numer;
        }
    }
}
