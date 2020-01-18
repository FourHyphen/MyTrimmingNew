using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTestClassImageController
    {
        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestSuccessOfCreateBitmapAfterNewImage()
        {
            MyTrimmingNew.ImageController ic = Common.GetDisplayImage(Common.TestResourceImage001Path);
            Assert.IsNotNull(ic.BitmapImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNotEqualOfImageNameAndSaveNameExample()
        {
            MyTrimmingNew.ImageController ic = Common.GetDisplayImage(Common.TestResourceImage001Path);
            Assert.AreNotEqual(System.IO.Path.GetFileName(Common.TestResourceImage001Path), ic.SaveNameExample);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestDisplayImageFitWindowSize()
        {
            MyTrimmingNew.ImageController ic = Common.GetDisplayImage(Common.TestResourceImage001Path);
            
            int ansImageWidth = Common.WindowInitWidth - MyTrimmingNew.Constant.FixCanvasWidth;
            double ratio = (double)ansImageWidth / (double)ic.BitmapImage.Width;
            int ansImageHeight = (int)((double)ic.BitmapImage.Height * ratio);

            Assert.AreEqual(ansImageWidth, ic.DisplayImageWidth);
            Assert.AreEqual(ansImageHeight, ic.DisplayImageHeight);
        }
    }
}
