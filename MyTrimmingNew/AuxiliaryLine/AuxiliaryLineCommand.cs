using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew.AuxiliaryLine
{
    abstract class AuxiliaryLineCommand
    {
        protected AuxiliaryController AC { get; set; }

        public AuxiliaryLineParameter BeforeParameter { get; private set; }

        public AuxiliaryLineCommand(AuxiliaryController ac)
        {
            AC = ac;
        }

        public AuxiliaryLineParameter Execute(object operation)
        {
            BeforeParameter = AC.CloneParameter();
            return ExecuteCore(operation);
        }

        public abstract AuxiliaryLineParameter ExecuteCore(object operation);
    }
}
