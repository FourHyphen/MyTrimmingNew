using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    class AuxiliaryLineMove : IAuxiliaryLineKeyOperation
    {
        private AuxiliaryController AC { get; set; }

        private Keys.EnableKeys Key { get; set; }

        public AuxiliaryLineMove(AuxiliaryController ac, Keys.EnableKeys key)
        {
            AC = ac;
            Key = key;
        }

        public void Execute()
        {
            if (Key == Keys.EnableKeys.Up)
            {
                AC.AuxiliaryTopRelativeImage--;
            }
            else if (Key == Keys.EnableKeys.Down)
            {
                AC.AuxiliaryTopRelativeImage++;
            }
            else if (Key == Keys.EnableKeys.Right)
            {
                AC.AuxiliaryLeftRelativeImage++;
            }
            else if (Key == Keys.EnableKeys.Left)
            {
                AC.AuxiliaryLeftRelativeImage--;
            }
        }
    }
}
