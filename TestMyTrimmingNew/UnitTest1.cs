using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTest1
    {
        // TODO: リソース管理すればDeploymentItemと共有できるか？
        private const string _testResourceImage001Path = @".\Resource\test001.jpg";

        // TODO: リソース管理してxaml側と数字を共有する
        private const int _windowInitWidth = 800;
        private const int _windowInitHeight = 600;

        private MyTrimmingNew.ImageController GetDisplayImage(string filePath)
        {
            return new MyTrimmingNew.ImageController(filePath, _windowInitWidth, _windowInitHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestSuccessOfCreateBitmapAfterNewImage()
        {
            MyTrimmingNew.ImageController img = GetDisplayImage(_testResourceImage001Path);
            Assert.IsNotNull(img.BitmapImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNotEqualOfImageNameAndSaveNameExample()
        {
            MyTrimmingNew.ImageController img = GetDisplayImage(_testResourceImage001Path);
            Assert.AreNotEqual(img.SaveNameExample, System.IO.Path.GetFileName(_testResourceImage001Path));
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestEqualOfChangedImageSizeWidthAndHeight()
        {
            // 数字の根拠無し
            int newWidth = 600;
            int newHeight = 350;

            Bitmap img = new Bitmap(_testResourceImage001Path);
            Bitmap newImg = MyTrimmingNew.common.Image.CreateBitmap(img, newWidth, newHeight);
            Assert.AreEqual(newImg.Width, newWidth);
            Assert.AreEqual(newImg.Height, newHeight);
        }
    }
}
