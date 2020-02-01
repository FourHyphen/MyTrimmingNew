using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew.AuxiliaryLine
{
    /// <summary>
    /// 何もしないOperationを定義するspecial case objectクラス
    /// </summary>
    class AuxiliaryLineNoneOperation : AuxiliaryLineCommand
    {
        public void Execute(object operation)
        {

        }

        public void UnExecute()
        {

        }
    }
}
