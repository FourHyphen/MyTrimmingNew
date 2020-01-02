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
            int mouseMoveX = (int)mouseUpCoordinateRelatedAuxiliaryLine.X - (int)MouseDownRelatedAuxiliaryLine.X;
            int mouseMoveY = (int)mouseUpCoordinateRelatedAuxiliaryLine.Y - (int)MouseDownRelatedAuxiliaryLine.Y;
            Mouse.KindMouseDownAuxiliaryLineArea mouseDownArea = Mouse.GetKindMouseDownAuxiliaryLineArea(AC, MouseDownRelatedAuxiliaryLine);

            // 意図した拡大/縮小のサイズになるよう符号を合わせてから処理実行
            // 例) 右下点のX方向操作 -> 正なら拡大、負なら縮小
            // 例) 左下点のX方向操作 -> 負なら拡大、正なら縮小
            //                           -> 符号反転し、拡大を意図しているなら正になるようにする
            AuxiliaryLineChangeSizeTemplate changeSizeLogic = null;
            if (mouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.RightBottom)
            {
                changeSizeLogic = new AuxiliaryLineChangeSizeBottomRight(AC);
            }
            else if(mouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.LeftBottom)
            {
                mouseMoveX = -mouseMoveX;
                changeSizeLogic = new AuxiliaryLineChangeSizeBottomLeft(AC);
            }

            changeSizeLogic.Execute(mouseMoveX, mouseMoveY);
        }
    }
}
