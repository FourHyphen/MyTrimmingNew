﻿using System.Drawing;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineChangeSize : AuxiliaryLineCommand
    {
        private Point MouseDownRelatedAuxiliaryLine { get; set; }

        public AuxiliaryLineChangeSize(AuxiliaryController ac, Point CoordinateRelatedAuxiliaryLine) : base(ac)
        {
            MouseDownRelatedAuxiliaryLine = CoordinateRelatedAuxiliaryLine;
        }

        public override AuxiliaryLineParameter ExecuteCore(object operation)
        {
            System.Windows.Point mouseUpCoordinateRelatedAuxiliaryLine = (System.Windows.Point)operation;
            int changeSizeWidth = (int)mouseUpCoordinateRelatedAuxiliaryLine.X - (int)MouseDownRelatedAuxiliaryLine.X;
            int changeSizeHeight = (int)mouseUpCoordinateRelatedAuxiliaryLine.Y - (int)MouseDownRelatedAuxiliaryLine.Y;
            Mouse.KindMouseDownAuxiliaryLineArea mouseDownArea = Mouse.GetKindMouseDownAuxiliaryLineArea(AC, MouseDownRelatedAuxiliaryLine);

            // 拡大を意図しているなら正に、縮小を意図しているなら負になるよう符号を合わせてから処理実行
            AuxiliaryLineChangeSizeTemplate changeSizeLogic = null;
            if (mouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.RightBottom)
            {
                changeSizeLogic = new AuxiliaryLineChangeSizeBottomRight(AC);
            }
            else if (mouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.LeftBottom)
            {
                changeSizeWidth = -changeSizeWidth;
                changeSizeLogic = new AuxiliaryLineChangeSizeBottomLeft(AC);
            }
            else if (mouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.RightTop)
            {
                changeSizeHeight = -changeSizeHeight;
                changeSizeLogic = new AuxiliaryLineChangeSizeTopRight(AC);
            }
            else if (mouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.LeftTop)
            {
                changeSizeWidth = -changeSizeWidth;
                changeSizeHeight = -changeSizeHeight;
                changeSizeLogic = new AuxiliaryLineChangeSizeTopLeft(AC);
            }

            return changeSizeLogic.Execute(changeSizeWidth, changeSizeHeight);
        }
    }
}
