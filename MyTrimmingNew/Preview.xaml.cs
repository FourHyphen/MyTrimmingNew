using System;
using System.Windows;

namespace MyTrimmingNew
{
    public partial class Preview : Window
    {
        public Preview(ImageController ic, AuxiliaryController ac)
        {
            InitializeComponent();
            ShowTrimImage(ic, ac);
        }

        private void ShowTrimImage(ImageController ic, AuxiliaryController ac)
        {
            xShowPreview.Source = ic.GetTrimImage(ac, windowWidth:700);
        }
    }
}
