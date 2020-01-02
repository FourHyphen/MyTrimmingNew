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

        private Mouse.KindMouseDownAuxiliaryLineArea MouseDownArea { get; set; }

        public AuxiliaryLineChangeSize(AuxiliaryController ac, Point CoordinateRelatedAuxiliaryLine)
        {
            AC = ac;
            MouseDownRelatedAuxiliaryLine = CoordinateRelatedAuxiliaryLine;
            MouseDownArea = Mouse.GetKindMouseDownAuxiliaryLineArea(AC, MouseDownRelatedAuxiliaryLine);
        }

        public void Execute(object operation)
        {
            Point mouseUpCoordinateRelatedAuxiliaryLine = (Point)operation;
            int mouseMoveX = (int)mouseUpCoordinateRelatedAuxiliaryLine.X - (int)MouseDownRelatedAuxiliaryLine.X;
            int mouseMoveY = (int)mouseUpCoordinateRelatedAuxiliaryLine.Y - (int)MouseDownRelatedAuxiliaryLine.Y;

            // 意図した拡大/縮小のサイズになるよう符号を合わせてから処理実行
            // 例) 右下点のX方向操作 -> 正なら拡大、負なら縮小
            // 例) 左下点のX方向操作 -> 負なら拡大、正なら縮小
            //                           -> 符号反転し、拡大を意図しているなら正になるようにする
            AuxiliaryLineChangeSizeTemplate changeSizeLogic = null;
            if (MouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.RightBottom)
            {
                changeSizeLogic = new AuxiliaryLineChangeSizeBottomRight(AC);
                changeSizeLogic.Execute(mouseMoveX, mouseMoveY);
            }
            else if(MouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.LeftBottom)
            {
                mouseMoveX = -mouseMoveX;
                ExecuteWhereOperationBottomLeft(mouseMoveX, mouseMoveY);
            }
        }

        /// <param name="changeWidth">正 -> 拡大を意図</param>
        /// <param name="changeHeight">正 -> 拡大を意図</param>
        public void ExecuteWhereOperationBottomLeft(int changeWidth, int changeHeight)
        {
            // 矩形のRatioに合わせたマウス移動距離を求め、その通りにサイズを変更する
            int changeSizeWidth = changeWidth;
            int changeSizeHeight = changeHeight;
            if (BaseWidthWhenChangeSize(changeSizeWidth, changeSizeHeight))
            {
                changeSizeHeight = CalcAuxiliaryLineHeightWithFitRatio(changeSizeWidth);
            }
            else
            {
                changeSizeWidth = CalcAuxiliaryLineWidthWithFitRatio(changeSizeHeight);
            }

            int maxChangeSizeWidth = AC.AuxiliaryLeftRelativeImage - AC.AuxiliaryLineThickness + 1;
            int maxChangeSizeHeight = AC.DisplayImageHeight - AC.AuxiliaryHeight - AC.AuxiliaryTopRelativeImage - AC.AuxiliaryLineThickness + 1;

            // 左下点を思いっきり右や上に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if ((-changeSizeWidth > AC.AuxiliaryWidth) || (AC.AuxiliaryHeight + changeSizeHeight) < 0)
            {
                return;
            }
            // 画像からはみ出るような変形の場合、画像一杯までの変形に制限する
            else if (changeSizeWidth > maxChangeSizeWidth)
            {
                changeSizeWidth = maxChangeSizeWidth;
                changeSizeHeight = CalcAuxiliaryLineHeightWithFitRatio(changeSizeWidth);
            }
            else if (changeSizeHeight > maxChangeSizeHeight)
            {
                changeSizeHeight = maxChangeSizeHeight;
                changeSizeWidth = CalcAuxiliaryLineWidthWithFitRatio(changeSizeHeight);
            }

            AC.AuxiliaryWidth += changeSizeWidth;
            AC.AuxiliaryHeight += changeSizeHeight;
            AC.AuxiliaryLeftRelativeImage -= changeSizeWidth;
        }

        private bool BaseWidthWhenChangeSize(int willChangeWidth, int willChangeHeight)
        {
            int baseWidthChangeHeight = CalcAuxiliaryLineHeightWithFitRatio(willChangeWidth);
            return (Math.Abs(baseWidthChangeHeight) > Math.Abs(willChangeHeight));
        }

        private int CalcAuxiliaryLineWidthWithFitRatio(int newAuxiliaryHeight)
        {
            double newWidth = (double)newAuxiliaryHeight * AC.AuxiliaryRatio;
            return (int)Math.Round(newWidth, 0, MidpointRounding.AwayFromZero);
        }

        private int CalcAuxiliaryLineHeightWithFitRatio(int newAuxiliaryWidth)
        {
            double newHeight = (double)newAuxiliaryWidth / AC.AuxiliaryRatio;
            return (int)Math.Round(newHeight, 0, MidpointRounding.AwayFromZero);
        }
    }
}
