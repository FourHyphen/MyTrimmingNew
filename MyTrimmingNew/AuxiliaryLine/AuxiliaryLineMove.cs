using System.Drawing;
using MyTrimmingNew.common;

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
            else if(o is System.Windows.Point)
            {
                return MoveByMouse(Common.ToDrawingPoint((System.Windows.Point)o));
            }
            else if (o is System.Drawing.Point)
            {
                return MoveByMouse((System.Drawing.Point)o);
            }

            return AC.CloneParameter();
        }

        private AuxiliaryLineParameter MoveByKey(Keys.EnableKeys key)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            // TODO: カーソルキーでの移動距離を可変にする
            if (key == Keys.EnableKeys.Up)
            {
                newParameter.MoveHeight(-1);
            }
            else if (key == Keys.EnableKeys.Down)
            {
                newParameter.MoveHeight(1);
            }
            else if (key == Keys.EnableKeys.Right)
            {
                newParameter.MoveWidth(1);
            }
            else if (key == Keys.EnableKeys.Left)
            {
                newParameter.MoveWidth(-1);
            }

            return newParameter;
        }

        private AuxiliaryLineParameter MoveByMouse(Point moveFinishPoint)
        {
            AuxiliaryLineParameter newParameter = AC.CloneParameter();

            int moveDistanceX = (int)(moveFinishPoint.X - MoveStartPoint.X);
            int moveDistanceY = (int)(moveFinishPoint.Y - MoveStartPoint.Y);
            newParameter.Move(moveDistanceX, moveDistanceY);

            return newParameter;
        }
    }
}
