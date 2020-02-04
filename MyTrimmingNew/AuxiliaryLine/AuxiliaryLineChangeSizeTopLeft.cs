using System;
namespace MyTrimmingNew.AuxiliaryLine
{
    internal class AuxiliaryLineChangeSizeTopLeft : AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineChangeSizeTopLeft(AuxiliaryController ac) : base(ac) { }

        public override int GetMaxChangeSizeWidth()
        {
            return (AC.AuxiliaryLeftRelativeImage - AC.AuxiliaryLineThickness + 1);
        }

        public override int GetMaxChangeSizeHeight()
        {
            return (AC.AuxiliaryTopRelativeImage - AC.AuxiliaryLineThickness + 1);
        }

        public override bool WillChangeAuxilirayOrigin(int changeSizeWidth, int changeSizeHeight)
        {
            // 左上点を思いっきり右や下に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
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
            return AC.AuxiliaryTopRelativeImage - changeSizeHeight;
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
