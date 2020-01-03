namespace MyTrimmingNew.AuxiliaryLine
{
    internal class AuxiliaryLineChangeSizeTopRight : AuxiliaryLineChangeSizeTemplate
    {
        public AuxiliaryLineChangeSizeTopRight(AuxiliaryController ac) : base(ac) { }

        public override int GetMaxChangeSizeWidth()
        {
            return (AC.DisplayImageWidth - AC.AuxiliaryWidth - AC.AuxiliaryLeftRelativeImage - AC.AuxiliaryLineThickness + 1);
        }

        public override int GetMaxChangeSizeHeight()
        {
            return (AC.AuxiliaryTopRelativeImage - AC.AuxiliaryLineThickness + 1);
        }

        public override bool WillChangeAuxilirayOrigin(int changeSizeWidth, int changeSizeHeight)
        {
            // 右上点を思いっきり左や下に引っ張ると原点が変わりうるが、その場合はサイズ変更しない
            if ((AC.AuxiliaryWidth + changeSizeWidth) < 0 || (AC.AuxiliaryHeight + changeSizeHeight) < 0)
            {
                return true;
            }
            return false;
        }

        public override void SetAuxiliaryLeft(int changeSizeWidth)
        {
            // 右上点の操作なら原点は変わらない
            return;
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