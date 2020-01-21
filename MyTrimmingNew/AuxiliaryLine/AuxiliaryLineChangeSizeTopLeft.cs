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

        public override void SetAuxiliaryLeft(int changeSizeWidth)
        {
            AC.AuxiliaryLeftRelativeImage -= changeSizeWidth;
        }

        public override void SetAuxiliaryTop(int changeSizeHeight)
        {
            AC.AuxiliaryTopRelativeImage -= changeSizeHeight;
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
