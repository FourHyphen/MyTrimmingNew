using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTrimmingNew
{
    class AuxiliaryLineChangeSize : IAuxiliaryLineOperation
    {
        private AuxiliaryController AC { get; set; }

        private Point MouseDownCoordinate { get; set; }

        private Mouse.KindMouseDownAuxiliaryLineArea MouseDownArea { get; set; }

        public AuxiliaryLineChangeSize(AuxiliaryController ac, Point mouseDownCoordinate)
        {
            AC = ac;
            MouseDownCoordinate = mouseDownCoordinate;
            MouseDownArea = Mouse.GetKindMouseDownAuxiliaryLineArea(AC, MouseDownCoordinate);
        }

        public void Execute(object operation)
        {
            Point mouseUpCoordinate = (Point)operation;
            int mouseMoveX = (int)mouseUpCoordinate.X - (int)MouseDownCoordinate.X;
            int mouseMoveY = (int)mouseUpCoordinate.Y - (int)MouseDownCoordinate.Y;

            if (MouseDownArea == Mouse.KindMouseDownAuxiliaryLineArea.RightBottom)
            {
                ExecuteWhereOperationBottomRight(mouseMoveX, mouseMoveY);
            }
        }

        public void ExecuteWhereOperationBottomRight(int changeWidth, int changeHeight)
        {
            // widthとheight、基準にする変更サイズに合わせてRatioの通りにサイズを変更する
            int newWidth = AC.AuxiliaryWidth + changeWidth;
            int newHeight = AC.AuxiliaryHeight + changeHeight;
            if (BaseWidthWhenChangeSize(changeWidth, changeHeight))
            {
                newHeight = CalcAuxiliaryLineHeightWithFitRatio(newWidth);
            }
            else
            {
                newWidth = CalcAuxiliaryLineWidthWithFitRatio(newHeight);
            }

            int maxWidth = AC.DisplayImageWidth - AC.AuxiliaryLeftRelativeImage;
            int maxHeight = AC.DisplayImageHeight - AC.AuxiliaryTopRelativeImage;

            // 右下点を思いっきり左や上に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if (newWidth < 0 || newHeight < 0)
            {
                return;
            }
            // 画像からはみ出るような変形の場合、画像一杯までの変形に制限する
            else if (newWidth > maxWidth)
            {
                newWidth = maxWidth;
                newHeight = CalcAuxiliaryLineHeightWithFitRatio(newWidth);
            }
            else if(newHeight > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = CalcAuxiliaryLineWidthWithFitRatio(newHeight);
            }

            AC.AuxiliaryWidth = newWidth;
            AC.AuxiliaryHeight = newHeight;
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
