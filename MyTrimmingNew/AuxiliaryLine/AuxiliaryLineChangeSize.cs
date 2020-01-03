using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineChangeSize : IAuxiliaryLineOperation
    {
        private AuxiliaryController AC { get; set; }

        private Point MouseDownRelatedAuxiliaryLine { get; set; }

        public AuxiliaryLineChangeSize(AuxiliaryController ac, Point CoordinateRelatedAuxiliaryLine)
        {
            AC = ac;
            MouseDownRelatedAuxiliaryLine = CoordinateRelatedAuxiliaryLine;
        }

        public void Execute(object operation)
        {
            Point mouseUpCoordinateRelatedAuxiliaryLine = (Point)operation;
            int changeSizeWidth = (int)mouseUpCoordinateRelatedAuxiliaryLine.X - (int)MouseDownRelatedAuxiliaryLine.X;
            int changeSizeHeight = (int)mouseUpCoordinateRelatedAuxiliaryLine.Y - (int)MouseDownRelatedAuxiliaryLine.Y;
            Mouse.KindMouseDownAuxiliaryLineArea mouseDownArea = Mouse.GetKindMouseDownAuxiliaryLineArea(AC, MouseDownRelatedAuxiliaryLine);

            // 拡大を意図しているなら正に、縮小を意図しているなら負になるよう符号を合わせてから処理実行
            AuxiliaryLineChangeSizeTemplate changeSizeLogic = null;
            if (mouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.RightBottom)
            {
                changeSizeLogic = new AuxiliaryLineChangeSizeBottomRight(AC);
            }
            else if(mouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.LeftBottom)
            {
                changeSizeWidth = -changeSizeWidth;
                changeSizeLogic = new AuxiliaryLineChangeSizeBottomLeft(AC);
            }
            else if (mouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.RightTop)
            {
                changeSizeHeight = -changeSizeHeight;
                changeSizeLogic = new AuxiliaryLineChangeSizeTopRight(AC);
            }

            changeSizeLogic.Execute(changeSizeWidth, changeSizeHeight);
        }
    }
}
