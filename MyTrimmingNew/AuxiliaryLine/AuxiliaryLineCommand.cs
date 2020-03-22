namespace MyTrimmingNew.AuxiliaryLine
{
    abstract class AuxiliaryLineCommand
    {
        protected AuxiliaryController AC { get; set; }

        public AuxiliaryLineParameter BeforeParameter { get; private set; }

        public AuxiliaryLineParameter AfterParameter { get; private set; }

        public AuxiliaryLineCommand(AuxiliaryController ac)
        {
            AC = ac;
        }

        public AuxiliaryLineParameter Execute(object operation)
        {
            BeforeParameter = AC.CloneParameter();
            AfterParameter = ExecuteCore(operation);
            return AfterParameter;
        }

        public abstract AuxiliaryLineParameter ExecuteCore(object operation);
    }
}
