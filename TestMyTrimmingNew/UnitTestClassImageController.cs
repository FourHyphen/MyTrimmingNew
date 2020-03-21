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
        public void TestDisplayImageFitWindowSize001()
        {
            MyTrimmingNew.ImageController ic = Common.GetDisplayImage(Common.TestResourceImage001Path);

            int ansImageHeight = Common.WindowInitHeight - MyTrimmingNew.Constant.FixCanvasHeight;
            double ratio = (double)ansImageHeight / (double)ic.BitmapImage.Height;
            int ansImageWidth = (int)((double)ic.BitmapImage.Width * ratio);

            Assert.AreEqual(ansImageWidth, ic.DisplayImageWidth);
            Assert.AreEqual(ansImageHeight, ic.DisplayImageHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test002.jpg")]
        public void TestDisplayImageFitWindowSize002()
        {
            MyTrimmingNew.ImageController ic = Common.GetDisplayImage(Common.TestResourceImage002Path);

            int ansImageHeight = Common.WindowInitHeight - MyTrimmingNew.Constant.FixCanvasHeight;
            double ratio = (double)ansImageHeight / (double)ic.BitmapImage.Height;
            int ansImageWidth = (int)((double)ic.BitmapImage.Width * ratio);

            Assert.AreEqual(ansImageWidth, ic.DisplayImageWidth);
            Assert.AreEqual(ansImageHeight, ic.DisplayImageHeight);
        }

    }
}
