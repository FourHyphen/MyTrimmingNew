using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace MyTrimmingNew
{
    class AuxiliaryLineRectangleObserver : IVisualComponentObserver
    {
        private Rectangle Rect { get; set; }
        
        private AuxiliaryController AC { get; set; }

        public AuxiliaryLineRectangleObserver(Rectangle rect, AuxiliaryController ac)
        {
            Rect = rect;
            AC = ac;
            AC.Attach(this);
            Draw(AC);
        }

        public void Update(object o)
        {
            AuxiliaryController ac = (AuxiliaryController)o;
            if (ac == AC)
            {
                Draw(ac);
            }
        }

        private void Draw(AuxiliaryController ac)
        {
            Canvas.SetLeft(Rect, (double)ac.AuxiliaryLeftRelativeImage);
            Canvas.SetTop(Rect, (double)ac.AuxiliaryTopRelativeImage);
            Rect.Width = ac.AuxiliaryWidth;
            Rect.Height = ac.AuxiliaryHeight;
        }
    }
}
