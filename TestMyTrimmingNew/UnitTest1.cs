using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using MyTrimmingNew;

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

        private const int _auxiliaryLineThickness = 1;

        private ImageController GetDisplayImage(string filePath)
        {
            return new ImageController(filePath, _windowInitWidth, _windowInitHeight);
        }

        private AuxiliaryController GetAuxiliaryController(string filePath, int widthRatio, int heightRatio)
        {
            ImageController ic = new ImageController(filePath,
                                                     _windowInitWidth,
                                                     _windowInitHeight);
            return new AuxiliaryController(ic, widthRatio, heightRatio, _auxiliaryLineThickness);
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
            Assert.AreNotEqual(System.IO.Path.GetFileName(_testResourceImage001Path), img.SaveNameExample);
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
            Assert.AreEqual(newWidth, newImg.Width);
            Assert.AreEqual(newHeight, newImg.Height);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageWidthLongerThanImageHeight()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            double ratio = (double)widthRatio / (double)heightRatio;
            int fittedHeight = (int)((double)_windowInitWidth / ratio);
            Assert.AreEqual(_windowInitWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test002.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageHeightLongerThanImageWidth()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage002Path,
                                                             widthRatio,
                                                             heightRatio);

            double ratio = (double)widthRatio / (double)heightRatio;
            int fittedWidth = (int)((double)_windowInitHeight / ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(_windowInitHeight, ac.AuxiliaryHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineOriginAfterInputCursorKeyUpIfAuxiliaryLineOriginTopIsZero()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Up);
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineStayInImageAfterInputCursorKeyDownIfAuxiliaryLineBottomIsImageBottom()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            int enableDownRange = ac.DisplayImageHeight - ac.AuxiliaryHeight;
            Keys.EnableKeys down = Keys.EnableKeys.Down;
            ac.SetEvent();
            for (int i = 1; i < enableDownRange; i++)
            {
                ac.PublishEvent(down);
                Assert.AreEqual(i, ac.AuxiliaryTopRelativeImage);
            }

            int maxDownRange = enableDownRange - _auxiliaryLineThickness;
            Assert.AreEqual(maxDownRange, ac.AuxiliaryTopRelativeImage);

            ac.PublishEvent(down);
            Assert.AreEqual(maxDownRange, ac.AuxiliaryTopRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineOriginAfterInputCursorKeyLeftIfAuxiliaryLineOriginLeftIsZero()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Left);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineStayInImageAfterInputCursorKeyRightIfAuxiliaryLineRightIsImageRight()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            int enableRightRange = ac.DisplayImageWidth - ac.AuxiliaryWidth;
            Keys.EnableKeys right = Keys.EnableKeys.Right;
            ac.SetEvent();
            for (int i = 1; i < enableRightRange; i++)
            {
                ac.PublishEvent(right);
                // 原点を右に移動させるので、原点位置=Leftと比較する
                Assert.AreEqual(i, ac.AuxiliaryLeftRelativeImage);
            }

            Assert.AreEqual(enableRightRange, ac.AuxiliaryLeftRelativeImage);

            ac.PublishEvent(right);
            Assert.AreEqual(enableRightRange, ac.AuxiliaryLeftRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineParameterAfterOperationThatDecreaseWidthOfAuxiliaryLineWhereBottomRight()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            int willDecreaseWidthPixel = -100;
            int willDecreaseHeightPixel = -5;
            ChangeAuxiliaryLineSizeIfWidthLongerThanHeightWhereBottomRight(ac, willDecreaseWidthPixel, willDecreaseHeightPixel);

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            willDecreaseWidthPixel = -5;
            willDecreaseHeightPixel = -100;
            ChangeAuxiliaryLineSizeIfHeightLongerThanWidthWhereBottomRight(ac, willDecreaseWidthPixel, willDecreaseHeightPixel);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoChangeAuxiliaryLineSizeIfTooLongWhereBottomRight()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            // 原点が変わるような操作の場合(= 右下点を思いっきり左や上に引っ張る操作)、サイズを変更しない
            // TODO: 対応する？
            int willDecreaseWidthPixel = -1200;
            int willDecreaseHeightPixel = -10;
            ChangeAuxiliaryLineSizeIfWidthLongerThanHeightWhereBottomRight(ac, willDecreaseWidthPixel, willDecreaseHeightPixel);

            willDecreaseWidthPixel = -10;
            willDecreaseHeightPixel = -1200;
            ChangeAuxiliaryLineSizeIfHeightLongerThanWidthWhereBottomRight(ac, willDecreaseWidthPixel, willDecreaseHeightPixel);
        }

        private void ChangeAuxiliaryLineSizeIfWidthLongerThanHeightWhereBottomRight(AuxiliaryController ac,
                                                                                    int mouseMoveWidthPixel,
                                                                                    int mouseMoveHeightPixel)
        {
            // 操作前の値を保持
            int beforeLeftRelativeImage = ac.AuxiliaryLeftRelativeImage;
            int beforeTopRelativeImage = ac.AuxiliaryTopRelativeImage;
            int beforeChangeSizeWidth = ac.AuxiliaryWidth;
            int beforeChangeSizeHeight = ac.AuxiliaryHeight;

            // 操作
            ac.ChangeSizeAuxiliaryLineWhereOperationBottomRight(mouseMoveWidthPixel, mouseMoveHeightPixel);

            // 右下の点の操作であれば、原点は変わらないのが正解
            Assert.AreEqual(beforeLeftRelativeImage, ac.AuxiliaryLeftRelativeImage);
            Assert.AreEqual(beforeTopRelativeImage, ac.AuxiliaryTopRelativeImage);

            // サイズ変更確認
            int expectSizeWidth = beforeChangeSizeWidth + mouseMoveWidthPixel;
            int expectSizeHeight = (int)Math.Round((double)expectSizeWidth / ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            if (expectSizeWidth < beforeLeftRelativeImage || expectSizeHeight < beforeTopRelativeImage)
            {
                expectSizeWidth = beforeChangeSizeWidth;
                expectSizeHeight = beforeChangeSizeHeight;
            }
            Assert.AreEqual(expectSizeWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(expectSizeHeight, ac.AuxiliaryHeight);
        }

        private void ChangeAuxiliaryLineSizeIfHeightLongerThanWidthWhereBottomRight(AuxiliaryController ac,
                                                                                    int mouseMoveWidthPixel,
                                                                                    int mouseMoveHeightPixel)
        {
            // 操作前の値を保持
            int beforeLeftRelativeImage = ac.AuxiliaryLeftRelativeImage;
            int beforeTopRelativeImage = ac.AuxiliaryTopRelativeImage;
            int beforeChangeSizeWidth = ac.AuxiliaryWidth;
            int beforeChangeSizeHeight = ac.AuxiliaryHeight;

            // 操作
            ac.ChangeSizeAuxiliaryLineWhereOperationBottomRight(mouseMoveWidthPixel, mouseMoveHeightPixel);

            // 右下の点の操作であれば、原点は変わらないのが正解
            Assert.AreEqual(beforeLeftRelativeImage, ac.AuxiliaryLeftRelativeImage);
            Assert.AreEqual(beforeTopRelativeImage, ac.AuxiliaryTopRelativeImage);

            // サイズ変更確認
            int expectSizeHeight = beforeChangeSizeHeight + mouseMoveHeightPixel;
            int expectSizeWidth = (int)Math.Round((double)expectSizeHeight * ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            if (expectSizeWidth < beforeLeftRelativeImage || expectSizeHeight < beforeTopRelativeImage)
            {
                expectSizeWidth = beforeChangeSizeWidth;
                expectSizeHeight = beforeChangeSizeHeight;
            }
            Assert.AreEqual(expectSizeWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(expectSizeHeight, ac.AuxiliaryHeight);
        }
    }
}
