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
    }
}
