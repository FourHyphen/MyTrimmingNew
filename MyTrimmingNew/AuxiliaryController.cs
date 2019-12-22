using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    public class AuxiliaryController
    {
        public AuxiliaryController(int displayImageWidth,
                                   int displayImageHeight,
                                   int widthRatio = 16,
                                   int heightRatio = 9)
        {
            Init(displayImageWidth, displayImageHeight, widthRatio, heightRatio);
        }

        private void Init(int displayImageWidth,
                          int displayImageHeight,
                          int widthRatio,
                          int heightRatio)
        {
            AuxiliaryWidthRatio = widthRatio;
            AuxiliaryHeightRatio = heightRatio;

            if (displayImageWidth > displayImageHeight)
            {
                AuxiliaryWidth = displayImageWidth;
                AuxiliaryHeight = (int)((double)displayImageWidth / AuxiliaryRatio);
            }
            else
            {
                AuxiliaryWidth = (int)((double)displayImageHeight / AuxiliaryRatio);
                AuxiliaryHeight = displayImageHeight;
            }
        }

        public int AuxiliaryWidth { get; private set; }

        public int AuxiliaryHeight { get; private set; }

        private int AuxiliaryWidthRatio { get; set; }

        private int AuxiliaryHeightRatio { get; set; }

        private double AuxiliaryRatio
        {
            get
            {
                return (double)AuxiliaryWidthRatio / (double)AuxiliaryHeightRatio;
            }
        }

        public string GetLineSizeString()
        {
            return "矩形: 横" + AuxiliaryWidth.ToString() + "x縦" + AuxiliaryHeight.ToString();
        }
    }
}
