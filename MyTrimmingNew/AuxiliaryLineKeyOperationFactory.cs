using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    class AuxiliaryLineKeyOperationFactory
    {
        private AuxiliaryLineKeyOperationFactory()
        {

        }

        public static IAuxiliaryLineKeyOperation Create(AuxiliaryController ac,
                                                        Keys.EnableKeys key)
        {
            return new AuxiliaryLineMove(ac, key);
        }
    }
}
