using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineChangeSizeBottomRight : AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineChangeSizeBottomRight(AuxiliaryController ac) : base(ac) { }

        public override int GetMaxChangeSizeWidth()
        {
            return (AC.DisplayImageWidth - AC.AuxiliaryWidth - AC.AuxiliaryLeftRelativeImage - AC.AuxiliaryLineThickness + 1);
        }

        public override int GetMaxChangeSizeHeight()
        {
            return (AC.DisplayImageHeight - AC.AuxiliaryHeight - AC.AuxiliaryTopRelativeImage - AC.AuxiliaryLineThickness + 1);
        }

        public override bool WillChangeAuxilirayOrigin(int changeSizeWidth, int changeSizeHeight)
        {
            // 右下点を思いっきり左や上に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if ((AC.AuxiliaryWidth + changeSizeWidth) < 0 || (AC.AuxiliaryHeight + changeSizeHeight) < 0)
            {
                return true;
            }
            return false;
        }

        public override void SetAuxiliaryLeft(int changeSizeWidth)
        {
            // 右下点の操作なら原点は変わらない
            return;
        }

        public override void SetAuxiliaryTop(int changeSizeHeight)
        {
            // 右下点の操作なら原点は変わらない
            return;
        }

        public override void SetAuxiliaryWidth(int changeSizeWidth)
        {
            AC.AuxiliaryWidth += changeSizeWidth;
        }

        public override void SetAuxiliaryHeight(int changeSizeHeight)
        {
            AC.AuxiliaryHeight += changeSizeHeight;
        }
    }
}
