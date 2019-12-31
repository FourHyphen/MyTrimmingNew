using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTrimmingNew
{
    class AuxiliaryLineMove : IAuxiliaryLineOperation
    {
        private AuxiliaryController AC { get; set; }

        public AuxiliaryLineMove(AuxiliaryController ac)
        {
            AC = ac;
        }

        public void Execute(object o)
        {
            if (o is Keys.EnableKeys)
            {
                MoveByKey((Keys.EnableKeys)o);
            }
            else if(o is Point)
            {
                MoveByMouse((Point)o);
            }
        }

        private void MoveByKey(Keys.EnableKeys key)
        {
            if (key == Keys.EnableKeys.Up)
            {
                AC.AuxiliaryTopRelativeImage--;
            }
            else if (key == Keys.EnableKeys.Down)
            {
                AC.AuxiliaryTopRelativeImage++;
            }
            else if (key == Keys.EnableKeys.Right)
            {
                AC.AuxiliaryLeftRelativeImage++;
            }
            else if (key == Keys.EnableKeys.Left)
            {
                AC.AuxiliaryLeftRelativeImage--;
            }
        }

        private void MoveByMouse(Point p)
        {

        }
    }
}
