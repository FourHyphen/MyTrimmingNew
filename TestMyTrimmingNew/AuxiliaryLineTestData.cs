using System.Drawing;
using MyTrimmingNew.AuxiliaryLine;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineTestData
    {
        public AuxiliaryLineTestData(Point expectLeftTop,
                                     Point expectLeftBottom,
                                     Point expectRightTop,
                                     Point expectRightBottom,
                                     int expectDegree)
        {
            ExpectLeftTop = expectLeftTop;
            ExpectLeftBottom = expectLeftBottom;
            ExpectRightTop = expectRightTop;
            ExpectRightBottom = expectRightBottom;
            ExpectDegree = expectDegree;
        }

        public AuxiliaryLineTestData(AuxiliaryLineParameter param)
        {
            ExpectLeftTop = param.LeftTop;
            ExpectLeftBottom = param.LeftBottom;
            ExpectRightTop = param.RightTop;
            ExpectRightBottom = param.RightBottom;
            ExpectDegree = param.Degree;
        }

        public Point ExpectLeftTop { get; private set; }

        public Point ExpectLeftBottom { get; private set; }

        public Point ExpectRightTop { get; private set; }

        public Point ExpectRightBottom { get; private set; }

        public int ExpectDegree { get; private set; }
    }
}
