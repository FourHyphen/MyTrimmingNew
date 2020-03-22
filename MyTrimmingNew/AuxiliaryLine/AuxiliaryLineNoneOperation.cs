namespace MyTrimmingNew.AuxiliaryLine
{
    /// <summary>
    /// 何もしないOperationを定義するspecial case objectクラス
    /// </summary>
    class AuxiliaryLineNoneOperation : AuxiliaryLineCommand
    {
        public AuxiliaryLineNoneOperation(AuxiliaryController ac) : base(ac)
        {

        }

        public override AuxiliaryLineParameter ExecuteCore(object operation)
        {
            return AC.CloneParameter();
        }
    }
}
