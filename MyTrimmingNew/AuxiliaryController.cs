using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyTrimmingNew.AuxiliaryLine;

namespace MyTrimmingNew
{
    /// <summary>
    /// TODO: ちょっといろいろPublicにし過ぎでは？
    /// </summary>
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

        public int AuxiliaryLeftRelativeImage
        {
            get
            {
                return Parameter.Left;
            }
            set
            {
                Parameter.Left = value;
            }
        }

        public int AuxiliaryTopRelativeImage
        {
            get
            {
                return Parameter.Top;
            }
            set
            {
                Parameter.Top = value;
            }
        }

        public int AuxiliaryWidth
        {
            get
            {
                return Parameter.Width;
            }
            set
            {
                Parameter.Width = value;
            }
        }

        public int AuxiliaryHeight
        {
            get
            {
                return Parameter.Height;
            }
            set
            {
                Parameter.Height = value;
            }
        }

        public double? AuxiliaryRatio { get { return Parameter.Ratio; } }

        public int AuxiliaryLineThickness { get { return Parameter.Thickness; } }

        public int DisplayImageWidth { get { return Parameter.ImageWidth; } }

        public int DisplayImageHeight { get { return Parameter.ImageHeight; } }

        private AuxiliaryLineCommandList AuxiliaryLineCommandList { get; set; }

        public void SetEvent()
        {
            AuxiliaryLineCommandList.Append(new AuxiliaryLineOperationFactory().Create(this));
        }

        public void SetEvent(Point coordinateRelatedAuxiliaryLine)
        {
            AuxiliaryLineCommandList.Append(new AuxiliaryLineOperationFactory().Create(this, coordinateRelatedAuxiliaryLine));
        }

        public void PublishEvent(object operation)
        {
            AuxiliaryLineCommandList.Execute(operation);

            // 登録Observerに変更を通知
            Notify();
        }

        public void CancelEvent()
        {
            AuxiliaryLineCommandList.UnExecute();
        }
    }
}
