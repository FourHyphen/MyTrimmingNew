using System.Collections.Generic;

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

        public AuxiliaryLineParameter Execute(AuxiliaryLineCommand command,
                                              object operation)
        {
            if (DoneUnExecuteBefore())
            {
                // かつての操作内容を削除し、今の操作内容を最新の履歴とする
                if (CommandListIndex >= 0)
                {
                    CommandList.RemoveRange(CommandListIndex, CommandList.Count - CommandListIndex);
                }
            }

            AuxiliaryLineParameter result = command.Execute(operation);
            CommandList.Add(command);
            CommandListIndex++;

            return result;
        }

        private bool DoneUnExecuteBefore()
        {
            return (CommandListIndex < CommandList.Count);
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
