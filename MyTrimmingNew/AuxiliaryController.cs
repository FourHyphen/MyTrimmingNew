using System.Drawing;
using MyTrimmingNew.AuxiliaryLine;
using MyTrimmingNew.common;

namespace MyTrimmingNew
{
    public class AuxiliaryController : Subject
    {
        private AuxiliaryLineParameter Parameter { get; set; }

        public AuxiliaryController(ImageController ic,
                                   AuxiliaryLineParameter.RatioType ratioType = AuxiliaryLineParameter.RatioType.W16H9,
                                   int auxiliaryLineThickness = 1)
        {
            Parameter = new AuxiliaryLineParameter(ic.DisplayImageWidth,
                                                   ic.DisplayImageHeight,
                                                   ratioType,
                                                   auxiliaryLineThickness);
            AuxiliaryLineCommandList = new AuxiliaryLineCommandList();
        }

        public AuxiliaryLineParameter CloneParameter()
        {
            return (AuxiliaryLineParameter)Parameter.Clone();
        }

        public int AuxiliaryLeft { get { return Parameter.Left; } }

        public int AuxiliaryTop { get { return Parameter.Top; } }

        public Point AuxiliaryLeftTop { get { return Parameter.LeftTop; } }

        public Point AuxiliaryLeftBottom { get { return Parameter.LeftBottom; } }

        public Point AuxiliaryRightTop { get { return Parameter.RightTop; } }

        public Point AuxiliaryRightBottom { get { return Parameter.RightBottom; } }

        public int AuxiliaryRight { get { return Parameter.Right; } }

        public int AuxiliaryBottom { get { return Parameter.Bottom; } }

        public double? AuxiliaryRatio { get { return Parameter.Ratio; } }

        public int AuxiliaryLineThickness { get { return Parameter.Thickness; } }

        public int DisplayImageWidth { get { return Parameter.ImageWidth; } }

        public int DisplayImageHeight { get { return Parameter.ImageHeight; } }

        public int AuxiliaryDegree { get { return Parameter.Degree; } }

        private AuxiliaryLineCommandList AuxiliaryLineCommandList { get; set; }

        private AuxiliaryLineCommand AuxiliaryLineCommand { get; set; }

        public void SetEvent()
        {
            // Key操作を想定
            AuxiliaryLineCommand = new AuxiliaryLineOperationFactory().Create(this);
        }

        public void SetEvent(int rotateDegree)
        {
            // 回転を想定
            AuxiliaryLineCommand = new AuxiliaryLineOperationFactory().Create(this, rotateDegree);
        }

        public void SetEvent(System.Windows.Point coordinateRelatedAuxiliaryLine)
        {
            // マウス操作を想定
            System.Drawing.Point p = Common.ToDrawingPoint(coordinateRelatedAuxiliaryLine);
            AuxiliaryLineCommand = new AuxiliaryLineOperationFactory().Create(this, p);
        }

        public void PublishEvent(object operation)
        {
            if(AuxiliaryLineCommand == null)
            {
                return;
            }

            Parameter = AuxiliaryLineCommandList.Execute(AuxiliaryLineCommand, operation);

            // 登録Observerに変更を通知
            Notify();
        }

        public void CancelEvent()
        {
            Parameter = AuxiliaryLineCommandList.UnExecute(this);

            // 登録Observerに変更を通知
            Notify();
        }

        public void RedoEvent()
        {
            Parameter = AuxiliaryLineCommandList.ReExecute(this);

            // 登録Observerに変更を通知
            Notify();
        }
    }
}
