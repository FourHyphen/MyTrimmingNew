using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew.AuxiliaryLine
{
    class AuxiliaryLineChangeSizeBottomLeft : AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineChangeSizeBottomLeft(AuxiliaryController ac) : base(ac) { }

        public override int GetMaxChangeSizeWidth()
        {
            return (AC.AuxiliaryLeftRelativeImage - AC.AuxiliaryLineThickness + 1);
        }

        public override int GetMaxChangeSizeHeight()
        {
            return (AC.DisplayImageHeight - AC.AuxiliaryHeight - AC.AuxiliaryTopRelativeImage - AC.AuxiliaryLineThickness + 1);
        }

        public override bool WillChangeAuxilirayOrigin(int changeSizeWidth, int changeSizeHeight)
        {
            // 左下点を思いっきり右や上に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if ((-changeSizeWidth > AC.AuxiliaryWidth) || (AC.AuxiliaryHeight + changeSizeHeight) < 0)
            {
                return true;
            }
            return false;
        }

        public override int GetNewAuxiliaryLeft(int changeSizeWidth)
        {
            return AC.AuxiliaryLeftRelativeImage - changeSizeWidth;
        }

        public override int GetNewAuxiliaryTop(int changeSizeHeight)
        {
            // 左下点の操作なら原点は変わらない
            return AC.AuxiliaryTopRelativeImage;
        }

        public override int GetNewAuxiliaryWidth(int changeSizeWidth)
        {
            return AC.AuxiliaryWidth + changeSizeWidth;
        }

        public override int GetNewAuxiliaryHeight(int changeSizeHeight)
        {
            return AC.AuxiliaryHeight + changeSizeHeight;
        }
    }
}
