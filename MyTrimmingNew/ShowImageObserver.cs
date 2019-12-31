using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyTrimmingNew
{
    class ShowImageObserver : IVisualComponentObserver
    {
        private Image Image { get; set; }

        private ImageController IC { get; set; }

        public ShowImageObserver(Image image, ImageController ic)
        {
            Image = image;
            IC = ic;
            IC.Attach(this);
            Draw(IC);
        }

        public void Update(object o)
        {
            ImageController ic = (ImageController)o;
            if(ic == IC)
            {
                Draw(ic);
            }
        }

        private void Draw(ImageController ic)
        {
            Image.Source = ic.GetImage();
        }
    }
}
