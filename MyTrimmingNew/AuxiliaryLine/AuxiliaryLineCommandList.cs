using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineCommandList
    {
        private List<AuxiliaryLineCommand> CommandList { get; set; }

        private int CommandListIndex { get; set; }

        public AuxiliaryLineCommandList()
        {
            CommandList = new List<AuxiliaryLineCommand>();
            CommandListIndex = 0;
        }

        public AuxiliaryLineParameter Execute(AuxiliaryController ac,
                                              AuxiliaryLineCommand command,
                                              object operation)
        {
            // TODO: 
            // 間にUnExecuteを挟んでいる場合、今の操作内容で現在Indexのを上書きし、それを最新とする
            // (整合の取れなくなった操作内容を削除する)
            // 判別はListIndexとList.Countの差で可能
            CommandList.Add(command);
            CommandListIndex++;

            return command.Execute(operation);
        }

        public AuxiliaryLineParameter UnExecute(AuxiliaryController ac)
        {
            if(CommandListIndex <= 0)
            {
                return ac.CloneParameter();
            }

            CommandListIndex--;
            return CommandList[CommandListIndex].BeforeParameter;
        }

        public AuxiliaryLineParameter ReExecute(AuxiliaryController ac)
        {
            if (CommandListIndex >= CommandList.Count)
            {
                return ac.CloneParameter();
            }

            AuxiliaryLineParameter redo = CommandList[CommandListIndex].AfterParameter;
            CommandListIndex++;
            return redo;
        }
    }
}
