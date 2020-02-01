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
            CommandListIndex = -1;    // CommandListへの初回Addで0になるような初期値にする
        }

        public void Append(AuxiliaryLineCommand command)
        {
            CommandList.Add(command);
            CommandListIndex++;
        }

        public void Execute(object operation)
        {
            if (CommandListIndex < 0)
            {
                return;
            }

            // 何もしないcommand(special case obj)チェック
            if (CommandList[CommandListIndex].GetType().Name == "AuxiliaryLineNoneOperation")
            {
                CommandList.RemoveAt(CommandListIndex);
                CommandListIndex--;
            }
            else
            {
                CommandList[CommandListIndex].Execute(operation);
            }
        }

        public void UnExecute()
        {
            //CommandList[CommandListIndex].UnExecute();
            //CommandListIndex--;
        }
    }
}
