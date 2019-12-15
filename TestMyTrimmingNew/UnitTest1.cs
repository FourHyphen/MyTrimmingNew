using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestSuccessOfCreateBitmapAfterNewImage()
        {
            MyTrimmingNew.Image img = new MyTrimmingNew.Image(@".\Resource\test001.jpg");
            Assert.IsNotNull(img.BitmapImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNotEqualOfImageNameAndSaveNameExample()
        {
            MyTrimmingNew.Image img = new MyTrimmingNew.Image(@".\Resource\test001.jpg");
            Assert.AreNotEqual(img.SaveNameExample, "test001.jpg");
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestEqualOfReductionWidthAndHeight()
        {
            MyTrimmingNew.Image img = new MyTrimmingNew.Image(@".\Resource\test001.jpg");
            MyTrimmingNew.ImageFixedWindow ifw = new MyTrimmingNew.ImageFixedWindow(img, 600, 400);
            Assert.AreEqual(ifw.Width, 600);
            Assert.AreEqual(ifw.Height, 400);
        }
    }
}
