using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTrimmingNew
{
    class AuxiliaryLineOperationFactory
    {
        public IAuxiliaryLineOperation Create(AuxiliaryController ac)
        {
            // TODO: いずれMouseでのMove操作も必要になる
            //        -> そのときでも終点だけ必要だからこのインタフェースでも大丈夫なのか...？
            return new AuxiliaryLineMove(ac);
        }

        public IAuxiliaryLineOperation Create(AuxiliaryController ac, 
                                              Point coordinateRelatedAuxiliaryLine)
        {
            return new AuxiliaryLineChangeSize(ac, coordinateRelatedAuxiliaryLine);
        }
    }
}
