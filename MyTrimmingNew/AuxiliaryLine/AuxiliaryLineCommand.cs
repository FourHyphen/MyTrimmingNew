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

        private AuxiliaryLineParameter BeforeParameter { get; set; }

        public AuxiliaryLineCommand(AuxiliaryController ac)
        {
            AC = ac;
        }

        public void Execute(object operation)
        {
            BeforeParameter = AC.CloneParameter();
            ExecuteCore(operation);
        }

        public abstract void ExecuteCore(object operation);

        public void UnExecute()
        {
            AC.AuxiliaryWidth = BeforeParameter.Width;
            AC.AuxiliaryHeight = BeforeParameter.Height;
            AC.AuxiliaryTopRelativeImage = BeforeParameter.Top;
            AC.AuxiliaryLeftRelativeImage = BeforeParameter.Left;
        }
    }
}
