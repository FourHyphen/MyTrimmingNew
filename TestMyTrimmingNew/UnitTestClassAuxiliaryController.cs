﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTestClassAuxiliaryController
    {
        #region "初期化"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageWidthLongerThanImageHeight()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
                                                                   widthRatio,
                                                                   heightRatio);

            double ratio = (double)widthRatio / (double)heightRatio;
            int fittedHeight = (int)((double)Common.WindowInitWidth / ratio);
            Assert.AreEqual(Common.WindowInitWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test002.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageHeightLongerThanImageWidth()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage002Path,
                                                                   widthRatio,
                                                                   heightRatio);

            double ratio = (double)widthRatio / (double)heightRatio;
            int fittedWidth = (int)((double)Common.WindowInitHeight / ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(Common.WindowInitHeight, ac.AuxiliaryHeight);
        }

        #endregion

        #region "移動: カーソルキー操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineOriginAfterInputCursorKeyUpIfAuxiliaryLineOriginTopIsZero()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
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
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
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

            int maxDownRange = enableDownRange - Common.AuxiliaryLineThickness + 1;
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
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
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
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
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

            int maxRightRange = enableRightRange - Common.AuxiliaryLineThickness + 1;
            Assert.AreEqual(maxRightRange, ac.AuxiliaryLeftRelativeImage);

            ac.PublishEvent(right);
            Assert.AreEqual(maxRightRange, ac.AuxiliaryLeftRelativeImage);
        }

        #endregion

        #region "移動: マウス操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineLeftAndTopIfMoveByMouse()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
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
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
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

        #endregion

        #region "拡大/縮小: 右下点操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineParameterAfterOperationThatDecreaseWidthOfAuxiliaryLineWhereBottomRight()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
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
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
                                                                   widthRatio,
                                                                   heightRatio);

            // 原点が変わるような操作の場合(= 右下点を思いっきり左や上に引っ張る操作)、サイズを変更しない
            // TODO: 対応する？
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -1200, -10, true);
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -10, -1200, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeAuxiliaryLineSizeTooLongerThanImageSizeBottomRight()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
                                                                   widthRatio,
                                                                   heightRatio);

            // 補助線矩形が画像からはみ出るような操作の場合、矩形サイズが画像一杯のサイズになるよう制御する
            // ユーザー操作としてあり得るのは画像より小さい補助線矩形を画像一杯に合わせる操作
            //  -> まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -100, -5, true);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, 200, 10, true);
        }

        private void ChangeAuxiliaryLineSizeWhereBottomRight(AuxiliaryController ac,
                                                             int changeSizeWidth,
                                                             int changeSizeHeight,
                                                             bool isWidthMuchLongerThanHeight)
        {
            AuxiliaryLineTestData testData
                = new AuxiliaryLineChangeSizeBottomRight().ChangeSize(ac,
                                                                      changeSizeWidth,
                                                                      changeSizeHeight,
                                                                      isWidthMuchLongerThanHeight);
            Assert.AreEqual(testData.ExpectLeft, ac.AuxiliaryLeftRelativeImage);
            Assert.AreEqual(testData.ExpectTop, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(testData.ExpectWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(testData.ExpectHeight, ac.AuxiliaryHeight);
        }

        #endregion

        #region "拡大/縮小: 左下点操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineParameterAfterOperationThatDecreaseWidthOfAuxiliaryLineWhereBottomLeft()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
                                                                   widthRatio,
                                                                   heightRatio);

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -100, -5, true);

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -5, -100, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoChangeAuxiliaryLineSizeIfTooLongWhereBottomLeft()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
                                                                   widthRatio,
                                                                   heightRatio);

            // 原点が変わるような操作の場合(= 左下点を思いっきり右や上に引っ張る操作)、サイズを変更しない
            // TODO: 対応する？
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -1200, -10, true);
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -10, -1200, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeAuxiliaryLineSizeTooLongerThanImageSizeBottomLeft()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
                                                                   widthRatio,
                                                                   heightRatio);

            // 補助線矩形が画像からはみ出るような操作の場合、矩形サイズが画像一杯のサイズになるよう制御する
            // ユーザー操作としてあり得るのは画像より小さい補助線矩形を画像一杯に合わせる操作
            //  -> まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -100, -5, true);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, 200, 10, true);
        }

        private void ChangeAuxiliaryLineSizeWhereBottomLeft(AuxiliaryController ac,
                                                            int changeSizeWidth,
                                                            int changeSizeHeight,
                                                            bool isWidthMuchLongerThanHeight)
        {
            AuxiliaryLineTestData testData
                = new AuxiliaryLineChangeSizeBottomLeft().ChangeSize(ac,
                                                                     changeSizeWidth,
                                                                     changeSizeHeight,
                                                                     isWidthMuchLongerThanHeight);
            Assert.AreEqual(testData.ExpectLeft, ac.AuxiliaryLeftRelativeImage);
            Assert.AreEqual(testData.ExpectTop, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(testData.ExpectWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(testData.ExpectHeight, ac.AuxiliaryHeight);
        }

        #endregion

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineParameterAfterOperationThatDecreaseWidthOfAuxiliaryLineWhereTopRight()
        {
            int widthRatio = 16;
            int heightRatio = 9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
                                                                   widthRatio,
                                                                   heightRatio);

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -100, -5, true);

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -5, -100, false);
        }

        private void ChangeAuxiliaryLineSizeWhereTopRight(AuxiliaryController ac,
                                                          int changeSizeWidth,
                                                          int changeSizeHeight,
                                                          bool isWidthMuchLongerThanHeight)
        {
            AuxiliaryLineTestData testData
                = new AuxiliaryLineChangeSizeTopRight().ChangeSize(ac,
                                                                   changeSizeWidth,
                                                                   changeSizeHeight,
                                                                   isWidthMuchLongerThanHeight);
            Assert.AreEqual(testData.ExpectLeft, ac.AuxiliaryLeftRelativeImage);
            Assert.AreEqual(testData.ExpectTop, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(testData.ExpectWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(testData.ExpectHeight, ac.AuxiliaryHeight);
        }
    }
}
