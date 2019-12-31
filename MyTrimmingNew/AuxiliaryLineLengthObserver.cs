using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyTrimmingNew
{
    class AuxiliaryLineLengthObserver : IVisualComponentObserver
    {
        private ContentControl CC { get; set; }

        private AuxiliaryController AC { get; set; }

        public AuxiliaryLineLengthObserver(ContentControl cc, AuxiliaryController ac)
        {
            CC = cc;
            AC = ac;
            AC.Attach(this);
            Draw(AC);
        }

        public void Update(object o)
        {
            AuxiliaryController ac = (AuxiliaryController)o;
            if(ac == AC)
            {
                Draw(ac);
            }
        }

        private void Draw(AuxiliaryController ac)
        {
            CC.Content = "矩形: 横" + ac.AuxiliaryWidth.ToString() + "x縦" + ac.AuxiliaryHeight.ToString();
        }
    }
}
