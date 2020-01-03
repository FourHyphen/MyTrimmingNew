using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMyTrimmingNew
{
    class AuxiliaryLineTestData
    {
        public AuxiliaryLineTestData(int expectLeft,
                                     int expectTop,
                                     int expectWidth,
                                     int expectHeight)
        {
            ExpectLeft = expectLeft;
            ExpectTop = expectTop;
            ExpectWidth = expectWidth;
            ExpectHeight = expectHeight;
        }

        public int ExpectLeft { get; private set; }

        public int ExpectTop { get; private set; }

        public int ExpectWidth { get; private set; }

        public int ExpectHeight { get; private set; }
    }
}
