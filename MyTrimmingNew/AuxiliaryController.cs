using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    public class AuxiliaryController
    {
        public AuxiliaryController(ImageController ic,
                                   int widthRatio = 16,
                                   int heightRatio = 9,
                                   int auxiliaryLineThickness = 1)
        {
            AuxiliaryWidthRatio = widthRatio;
            AuxiliaryHeightRatio = heightRatio;

            if (ic.DisplayImageWidth > ic.DisplayImageHeight)
            {
                AuxiliaryWidth = ic.DisplayImageWidth;
                AuxiliaryHeight = (int)((double)ic.DisplayImageWidth / AuxiliaryRatio);
            }
            else
            {
                AuxiliaryWidth = (int)((double)ic.DisplayImageHeight / AuxiliaryRatio);
                AuxiliaryHeight = ic.DisplayImageHeight;
            }

            DisplayImageWidth = ic.DisplayImageWidth;
            DisplayImageHeight = ic.DisplayImageHeight;

            // 初期値は画像の原点に合わせる
            AuxiliaryLeftRelativeImage = 0;
            AuxiliaryTopRelativeImage = 0;

            // TODO: 線の太さを変更できるようにする
            AuxiliaryLineThickness = 1;
        }

        private int _auxiliaryLeftRelativeImage;

        public int AuxiliaryLeftRelativeImage
        {
            get
            {
                return _auxiliaryLeftRelativeImage;
            }
            private set
            {
                _auxiliaryLeftRelativeImage = value;
            }
        }

        private int _auxiliaryTopRelativeImage;

        public int AuxiliaryTopRelativeImage
        {
            get
            {
                return _auxiliaryTopRelativeImage;
            }
            private set
            {
                _auxiliaryTopRelativeImage = FitInRangeAuxiliaryTopRelativeImage(value);
            }
        }

        private int AuxiliaryLineThickness { get; set; }

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

        public int DisplayImageWidth { get; private set; }

        public int DisplayImageHeight { get; private set; }

        public string GetLineSizeString()
        {
            return "矩形: 横" + AuxiliaryWidth.ToString() + "x縦" + AuxiliaryHeight.ToString();
        }

        public void MoveAuxiliaryLine(Keys.EnableKeys key)
        {
            if (key == Keys.EnableKeys.Up)
            {
                AuxiliaryTopRelativeImage--;
            }
            else if (key == Keys.EnableKeys.Down)
            {
                AuxiliaryTopRelativeImage++;
            }
            else if (key == Keys.EnableKeys.Right)
            {
                AuxiliaryLeftRelativeImage++;
            }
            else if (key == Keys.EnableKeys.Left)
            {
                AuxiliaryLeftRelativeImage--;
            }
        }

        private int FitInRangeAuxiliaryTopRelativeImage(int toMoveTop)
        {
            if (toMoveTop < 0)
            {
                return 0;
            }

            int maxOriginTop = DisplayImageHeight - AuxiliaryHeight - AuxiliaryLineThickness;
            if (toMoveTop > maxOriginTop)
            {
                return AuxiliaryTopRelativeImage;
            }

            return toMoveTop;
        }
    }
}
