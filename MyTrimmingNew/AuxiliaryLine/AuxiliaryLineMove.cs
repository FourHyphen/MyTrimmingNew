using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineMove : AuxiliaryLineCommand
    {
        private Point MoveStartPoint { get; set; }

        public AuxiliaryLineMove(AuxiliaryController ac) : base(ac)
        {
            // キー操作による移動
        }

        public AuxiliaryLineMove(AuxiliaryController ac, Point moveStartPoint) : base(ac)
        {
            // マウス操作による移動
            MoveStartPoint = moveStartPoint;
        }

        public override AuxiliaryLineParameter ExecuteCore(object o)
        {
            if (o is Keys.EnableKeys)
            {
                return MoveByKey((Keys.EnableKeys)o);
            }
            else if(o is Point)
            {
                return MoveByMouse((Point)o);
            }

            return AC.CloneParameter();
        }

        private AuxiliaryLineParameter MoveByKey(Keys.EnableKeys key)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            if (key == Keys.EnableKeys.Up)
            {
                newParameter.Top--;
            }
            else if (key == Keys.EnableKeys.Down)
            {
                newParameter.Top++;
            }
            else if (key == Keys.EnableKeys.Right)
            {
                newParameter.Left++;
            }
            else if (key == Keys.EnableKeys.Left)
            {
                newParameter.Left--;
            }

            return newParameter;
        }

        private AuxiliaryLineParameter MoveByMouse(Point moveFinishPoint)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            int moveDistanceX = (int)(moveFinishPoint.X - MoveStartPoint.X);
            int moveDistanceY = (int)(moveFinishPoint.Y - MoveStartPoint.Y);
            newParameter.Left += moveDistanceX;
            newParameter.Top += moveDistanceY;

            return newParameter;
        }
    }
}
