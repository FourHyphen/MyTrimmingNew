using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTestClassCommonImage
    {
        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestEqualOfChangedImageSizeWidthAndHeight()
        {
            // 数字の根拠無し
            int newWidth = 600;
            int newHeight = 350;

            Bitmap img = new Bitmap(Common.TestResourceImage001Path);
            Bitmap newImg = MyTrimmingNew.common.Image.CreateBitmap(img, newWidth, newHeight);
            Assert.AreEqual(newWidth, newImg.Width);
            Assert.AreEqual(newHeight, newImg.Height);
        }
    }
}
