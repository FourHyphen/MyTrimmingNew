using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    class AuxiliaryLineMove : IAuxiliaryLineOperation
    {
        private AuxiliaryController AC { get; set; }

        public AuxiliaryLineMove(AuxiliaryController ac)
        {
            AC = ac;
        }

        public void Execute(object object_key)
        {
            Keys.EnableKeys key = (Keys.EnableKeys)object_key;
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
    }
}
