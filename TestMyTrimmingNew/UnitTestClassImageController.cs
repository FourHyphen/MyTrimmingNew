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
            MyTrimmingNew.ImageController img = Common.GetDisplayImage(Common.TestResourceImage001Path);
            Assert.IsNotNull(img.BitmapImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNotEqualOfImageNameAndSaveNameExample()
        {
            MyTrimmingNew.ImageController img = Common.GetDisplayImage(Common.TestResourceImage001Path);
            Assert.AreNotEqual(System.IO.Path.GetFileName(Common.TestResourceImage001Path), img.SaveNameExample);
        }
    }
}
