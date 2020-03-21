using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineRotate : AuxiliaryLineCommand
    {
        private int Degree { get; set; }

        public AuxiliaryLineRotate(AuxiliaryController ac, int degree): base(ac)
        {
            Degree = degree;
        }

        public override AuxiliaryLineParameter ExecuteCore(object operation = null)
        {
            Point newLeftTop = CalcRotatePoint(AC.AuxiliaryLeftTop, Degree);
            Point newLeftBottom = CalcRotatePoint(AC.AuxiliaryLeftBottom, Degree);
            Point newRightTop = CalcRotatePoint(AC.AuxiliaryRightTop, Degree);
            Point newRightBottom = CalcRotatePoint(AC.AuxiliaryRightBottom, Degree);

            AuxiliaryLineParameter newParameter = AC.CloneParameter();
            newParameter.ReplacePoint(newLeftTop, newLeftBottom, newRightTop, newRightBottom);
            newParameter.Degree += Degree;

            return newParameter;
        }

        private Point CalcRotatePoint(Point p, int degree)
        {
            double rad = ToRadian(degree);
            double rotateX = p.X * Math.Cos(rad) - p.Y * Math.Sin(rad);
            double rotateY = p.Y * Math.Cos(rad) + p.X * Math.Sin(rad);
            return new Point((int)rotateX, (int)rotateY);
        }

        private double ToRadian(int degree)
        {
            return (double)degree * Math.PI / 180.0;
        }
    }
}
