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
            for (int i = 1; i <= enableDownRange; i++)
            {
                ac.PublishEvent(down);
                Assert.AreEqual(i, ac.AuxiliaryTopRelativeImage);
            }

            int maxDownRange = enableDownRange - _auxiliaryLineThickness + 1;
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
            for (int i = 1; i <= enableRightRange; i++)
            {
                ac.PublishEvent(right);
                // 原点を右に移動させるので、原点位置=Leftと比較する
                Assert.AreEqual(i, ac.AuxiliaryLeftRelativeImage);
            }

            int maxRightRange = enableRightRange - _auxiliaryLineThickness + 1;
            Assert.AreEqual(maxRightRange, ac.AuxiliaryLeftRelativeImage);

            ac.PublishEvent(right);
            Assert.AreEqual(maxRightRange, ac.AuxiliaryLeftRelativeImage);
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
            ChangeAuxiliaryLineSizeWhereBottomRight(ac,
                                                    willDecreaseWidthPixel,
                                                    willDecreaseHeightPixel,
                                                    true);

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            willDecreaseWidthPixel = -5;
            willDecreaseHeightPixel = -100;
            ChangeAuxiliaryLineSizeWhereBottomRight(ac,
                                                    willDecreaseWidthPixel,
                                                    willDecreaseHeightPixel,
                                                    false);
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
            ChangeAuxiliaryLineSizeWhereBottomRight(ac,
                                                    willDecreaseWidthPixel,
                                                    willDecreaseHeightPixel,
                                                    true);

            willDecreaseWidthPixel = -10;
            willDecreaseHeightPixel = -1200;
            ChangeAuxiliaryLineSizeWhereBottomRight(ac,
                                                    willDecreaseWidthPixel,
                                                    willDecreaseHeightPixel,
                                                    false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeAuxiliaryLineSizeTooLongerThanImageSizeBottomRight()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            // 補助線矩形が画像からはみ出るような操作の場合、矩形サイズが画像一杯のサイズになるよう制御する
            int willDecreaseWidthPixel = -100;
            int willDecreaseHeightPixel = -5;
            ChangeAuxiliaryLineSizeWhereBottomRight(ac,
                                                    willDecreaseWidthPixel,
                                                    willDecreaseHeightPixel,
                                                    true);

            int willIncreaseWidthPixel = 200;
            int willIncreaseHeightPixel = 10;
            ChangeAuxiliaryLineSizeWhereBottomRight(ac,
                                                    willIncreaseWidthPixel,
                                                    willIncreaseHeightPixel,
                                                    true);
        }

        private void ChangeAuxiliaryLineSizeWhereBottomRight(AuxiliaryController ac,
                                                             int mouseMoveWidthPixel,
                                                             int mouseMoveHeightPixel,
                                                             bool isWidthMuchLongerThanHeight)
        {
            // 操作前の値を保持
            int beforeLeftRelativeImage = ac.AuxiliaryLeftRelativeImage;
            int beforeTopRelativeImage = ac.AuxiliaryTopRelativeImage;
            int beforeChangeSizeWidth = ac.AuxiliaryWidth;
            int beforeChangeSizeHeight = ac.AuxiliaryHeight;

            // 操作
            double mouseUpX = (double)ac.AuxiliaryWidth + (double)mouseMoveWidthPixel;
            double mouseUpY = (double)ac.AuxiliaryHeight + (double)mouseMoveHeightPixel;
            System.Windows.Point mouseDown = new System.Windows.Point((double)ac.AuxiliaryWidth, (double)ac.AuxiliaryHeight);
            System.Windows.Point mouseUp = new System.Windows.Point(mouseUpX, mouseUpY);
            ac.SetEvent(mouseDown);
            ac.PublishEvent(mouseUp);

            // 右下の点の操作であれば、原点は変わらないのが正解
            Assert.AreEqual(beforeLeftRelativeImage, ac.AuxiliaryLeftRelativeImage);
            Assert.AreEqual(beforeTopRelativeImage, ac.AuxiliaryTopRelativeImage);

            // サイズ変更確認
            int expectSizeWidth;
            int expectSizeHeight;
            if (isWidthMuchLongerThanHeight)
            {
                expectSizeWidth = beforeChangeSizeWidth + mouseMoveWidthPixel;
                expectSizeHeight = (int)Math.Round((double)expectSizeWidth / ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                expectSizeHeight = beforeChangeSizeHeight + mouseMoveHeightPixel;
                expectSizeWidth = (int)Math.Round((double)expectSizeHeight * ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }

            if (expectSizeWidth < beforeLeftRelativeImage || expectSizeHeight < beforeTopRelativeImage)
            {
                // 原点が変わるようなサイズ変更が要求されても、サイズ変更しない
                expectSizeWidth = beforeChangeSizeWidth;
                expectSizeHeight = beforeChangeSizeHeight;
            }
            else if(expectSizeWidth > ac.DisplayImageWidth || expectSizeHeight > ac.DisplayImageHeight)
            {
                // 画像からはみ出るようなサイズ変更が要求された場合、代わりに画像一杯まで広げる
                if (isWidthMuchLongerThanHeight)
                {
                    expectSizeWidth = ac.DisplayImageWidth;
                    expectSizeHeight = (int)Math.Round((double)expectSizeWidth / ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    expectSizeHeight = ac.DisplayImageHeight;
                    expectSizeWidth = (int)Math.Round((double)expectSizeHeight * ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
                }
            }

            Assert.AreEqual(expectSizeWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(expectSizeHeight, ac.AuxiliaryHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineLeftAndTopIfMoveByMouse()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            // まず矩形を小さくして移動テストできるようにする(Width基準とする)
            int mouseUpPixelX = -(ac.AuxiliaryWidth / 2);
            int mouseUpPixelY = -5;
            double mouseUpX = (double)ac.AuxiliaryWidth + (double)mouseUpPixelX;
            double mouseUpY = (double)ac.AuxiliaryHeight + (double)mouseUpPixelY;
            System.Windows.Point mouseDown = new System.Windows.Point((double)ac.AuxiliaryWidth, (double)ac.AuxiliaryHeight);
            System.Windows.Point mouseUp = new System.Windows.Point(mouseUpX, mouseUpY);
            ac.SetEvent(mouseDown);
            ac.PublishEvent(mouseUp);

            // 右下方向テスト
            int moveX = 100;
            int moveY = 80;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 左上方向テスト
            moveX = -50;
            moveY = -70;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 左下方向テスト
            moveX = -20;
            moveY = 40;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 右上方向テスト
            moveX = 40;
            moveY = 20;
            MoveAuxiliaryLine(ac, moveX, moveY);
        }

        private void MoveAuxiliaryLine(AuxiliaryController ac,
                                       int moveX,
                                       int moveY)
        {
            int startMovePointX = ac.AuxiliaryLeftRelativeImage + (ac.AuxiliaryWidth / 2);
            int startMovePointY = ac.AuxiliaryTopRelativeImage + (ac.AuxiliaryHeight / 2);
            int finishMovePointX = startMovePointX + moveX;
            int finishMovePointY = startMovePointY + moveY;
            System.Windows.Point startMovePoint = new System.Windows.Point(startMovePointX, startMovePointY);
            System.Windows.Point finishMovePoint = new System.Windows.Point(finishMovePointX, finishMovePointY);

            int correctLeft = ac.AuxiliaryLeftRelativeImage + moveX;
            int correctTop = ac.AuxiliaryTopRelativeImage + moveY;
            MoveAuxiliaryLine(ac, startMovePoint, finishMovePoint, correctLeft, correctTop);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageIfMoveByMouseTooFarMoveDistance()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = GetAuxiliaryController(_testResourceImage001Path,
                                                             widthRatio,
                                                             heightRatio);

            // 補助線矩形を大きく移動しようとして矩形が画像からはみ出る場合、画像一杯までの移動に制限する
            // テストとして選ぶ値の基準: ディスプレイより大きい距離を移動しようとするなら、確実に画像からはみ出る
            System.Windows.Point startMovePoint = new System.Windows.Point(ac.AuxiliaryWidth / 2, ac.AuxiliaryHeight / 2);
            int tooMoveX = 3000;
            int tooMoveY = 3000;

            // 左上方向テスト
            System.Windows.Point finishMovePoint = new System.Windows.Point(-tooMoveX, -tooMoveY);
            int correctLeft = 0;
            int correctTop = 0;
            MoveAuxiliaryLine(ac, startMovePoint, finishMovePoint, correctLeft, correctTop);

            // 右上方向テスト
            finishMovePoint = new System.Windows.Point(tooMoveX, -tooMoveY);
            correctLeft = ac.DisplayImageWidth - ac.AuxiliaryWidth;
            correctTop = 0;
            MoveAuxiliaryLine(ac, startMovePoint, finishMovePoint, correctLeft, correctTop);

            // 左下方向テスト
            finishMovePoint = new System.Windows.Point(-tooMoveX, tooMoveY);
            correctLeft = 0;
            correctTop = ac.DisplayImageHeight - ac.AuxiliaryHeight;
            MoveAuxiliaryLine(ac, startMovePoint, finishMovePoint, correctLeft, correctTop);

            // 右下方向テスト
            finishMovePoint = new System.Windows.Point(tooMoveX, tooMoveY);
            correctLeft = ac.DisplayImageWidth - ac.AuxiliaryWidth;
            correctTop = ac.DisplayImageHeight - ac.AuxiliaryHeight;
            MoveAuxiliaryLine(ac, startMovePoint, finishMovePoint, correctLeft, correctTop);
        }

        private void MoveAuxiliaryLine(AuxiliaryController ac,
                                       System.Windows.Point startPoint,
                                       System.Windows.Point finishPoint,
                                       int correctLeftAfterMove,
                                       int correctTopAfterMove)
        {
            ac.SetEvent(startPoint);
            ac.PublishEvent(finishPoint);
            Assert.AreEqual(ac.AuxiliaryLeftRelativeImage, correctLeftAfterMove);
            Assert.AreEqual(ac.AuxiliaryTopRelativeImage, correctTopAfterMove);
        }
    }
}
