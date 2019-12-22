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
        private const string _testResourceImage002Path = @".\Resource\test002.jpg";

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

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageWidthLongerThanImageHeight()
        {
            MyTrimmingNew.ImageController ic = new MyTrimmingNew.ImageController(_testResourceImage001Path,
                                                                                 _windowInitWidth,
                                                                                 _windowInitHeight);

            int widthRatio = 16;
            int heightRatio = 9;
            MyTrimmingNew.AuxiliaryController ac = new MyTrimmingNew.AuxiliaryController(ic.DisplayImageWidth,
                                                                                         ic.DisplayImageHeight,
                                                                                         widthRatio,
                                                                                         heightRatio);

            double ratio = (double)widthRatio / (double)heightRatio;
            int fittedHeight = (int)((double)_windowInitWidth / ratio);
            Assert.AreEqual(ac.AuxiliaryWidth, _windowInitWidth);
            Assert.AreEqual(ac.AuxiliaryHeight, fittedHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test002.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageHeightLongerThanImageWidth()
        {
            MyTrimmingNew.ImageController ic = new MyTrimmingNew.ImageController(_testResourceImage002Path,
                                                                                 _windowInitWidth,
                                                                                 _windowInitHeight);

            int widthRatio = 16;
            int heightRatio = 9;
            MyTrimmingNew.AuxiliaryController ac = new MyTrimmingNew.AuxiliaryController(ic.DisplayImageWidth,
                                                                                         ic.DisplayImageHeight,
                                                                                         widthRatio,
                                                                                         heightRatio);

            double ratio = (double)widthRatio / (double)heightRatio;
            int fittedWidth = (int)((double)_windowInitHeight / ratio);
            Assert.AreEqual(ac.AuxiliaryWidth, fittedWidth);
            Assert.AreEqual(ac.AuxiliaryHeight, _windowInitHeight);
        }
    }
}
