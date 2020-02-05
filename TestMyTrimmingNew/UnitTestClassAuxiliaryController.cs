using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrimmingNew;
using MyTrimmingNew.AuxiliaryLine;
using static MyTrimmingNew.AuxiliaryLine.AuxiliaryLineParameter;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTestClassAuxiliaryController
    {
        #region "初期化"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineRatioW16H9IfImageWidthLongerThanImageHeight()
        {
            RatioType ratioType = RatioType.W16H9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path, ratioType);

            double ratio = (double)WidthRatio(ratioType) / (double)HeightRatio(ratioType);
            int fittedWidth = Common.WindowInitWidth - Constant.FixCanvasWidth;
            int fittedHeight = (int)((double)fittedWidth / ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineRatioW4H3IfImageWidthLongerThanImageHeight()
        {
            RatioType ratioType = RatioType.W4H3;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path, ratioType);

            double ratio = (double)WidthRatio(ratioType) / (double)HeightRatio(ratioType);
            int fittedHeight = ac.DisplayImageHeight;
            int fittedWidth = (int)((double)fittedHeight * ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test002.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageHeightLongerThanImageWidth()
        {
            RatioType ratioType = RatioType.W16H9;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage002Path, ratioType);

            double ratio = (double)WidthRatio(ratioType) / (double)HeightRatio(ratioType);

            int fittedWidth = ac.DisplayImageWidth;
            int fittedHeight = (int)((double)ac.DisplayImageWidth / ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryHeight);
        }

        #endregion

        #region "移動: カーソルキー操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineOriginAfterInputCursorKeyUpIfAuxiliaryLineOriginTopIsZero()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Up);
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineStayInImageAfterInputCursorKeyDownIfAuxiliaryLineBottomIsImageBottom()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

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
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Left);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineStayInImageAfterInputCursorKeyRightIfAuxiliaryLineRightIsImageRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

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
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

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
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

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
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -100, -5, true);

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -5, -100, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoChangeAuxiliaryLineSizeIfTooLongWhereBottomRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 原点が変わるような操作の場合(= 右下点を思いっきり左や上に引っ張る操作)、サイズを変更しない
            // TODO: 対応する？
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -1200, -10, true);
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -10, -1200, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeAuxiliaryLineSizeTooLongerThanImageSizeBottomRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像からはみ出るような操作の場合、矩形サイズが画像一杯のサイズになるよう制御する
            // ユーザー操作としてあり得るのは画像より小さい補助線矩形を画像一杯に合わせる操作
            //  -> まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -100, -5, true);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, 200, 10, true);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineSizeIfNoDefineRatio()
        {
            RatioType ratioType = RatioType.NoDefined;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path, ratioType);

            // 縮小
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -100, -5, true);

            // 拡大
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, 50, 1, true);
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
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -100, -5, true);

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -5, -100, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoChangeAuxiliaryLineSizeIfTooLongWhereBottomLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 原点が変わるような操作の場合(= 左下点を思いっきり右や上に引っ張る操作)、サイズを変更しない
            // TODO: 対応する？
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -1200, -10, true);
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -10, -1200, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeAuxiliaryLineSizeTooLongerThanImageSizeBottomLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

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

        #region "拡大/縮小: 右上点操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineParameterAfterOperationThatDecreaseWidthOfAuxiliaryLineWhereTopRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -100, -5, true);

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -5, -100, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoChangeAuxiliaryLineSizeIfTooLongWhereTopRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 原点が変わるような操作の場合(= 左下点を思いっきり右や上に引っ張る操作)、サイズを変更しない
            // TODO: 対応する？
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -1200, -10, true);
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -10, -1200, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeAuxiliaryLineSizeTooLongerThanImageSizeTopRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像からはみ出るような操作の場合、矩形サイズが画像一杯のサイズになるよう制御する
            // ユーザー操作としてあり得るのは画像より小さい補助線矩形を画像一杯に合わせる操作
            //  -> まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -100, -5, true);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereTopRight(ac, 200, 10, true);
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

        #endregion

        #region "拡大/縮小: 左上点操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineParameterAfterOperationThatDecreaseWidthOfAuxiliaryLineWhereTopLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, -100, -5, true);

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, -5, -100, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoChangeAuxiliaryLineSizeIfTooLongWhereTopLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 原点が変わるような操作の場合(= 左上点を思いっきり右や上に引っ張る操作)、サイズを変更しない
            // TODO: 対応する？
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, -1200, -10, true);
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, -10, -1200, false);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeAuxiliaryLineSizeTooLongerThanImageSizeTopLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像からはみ出るような操作の場合、矩形サイズが画像一杯のサイズになるよう制御する
            // ユーザー操作としてあり得るのは画像より小さい補助線矩形を画像一杯に合わせる操作
            //  -> まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, -100, -5, true);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, 200, 10, true);
        }

        private void ChangeAuxiliaryLineSizeWhereTopLeft(AuxiliaryController ac,
                                                         int changeSizeWidth,
                                                         int changeSizeHeight,
                                                         bool isWidthMuchLongerThanHeight)
        {
            AuxiliaryLineTestData testData
                = new AuxiliaryLineChangeSizeTopLeft().ChangeSize(ac,
                                                                  changeSizeWidth,
                                                                  changeSizeHeight,
                                                                  isWidthMuchLongerThanHeight);
            Assert.AreEqual(testData.ExpectLeft, ac.AuxiliaryLeftRelativeImage);
            Assert.AreEqual(testData.ExpectTop, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(testData.ExpectWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(testData.ExpectHeight, ac.AuxiliaryHeight);
        }

        #endregion

        #region "補助線矩形外操作: マウス操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoCrashAuxiliaryLineWhenCallPublishEventBeforeAnySetEvent()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 何もEventを設定していない状態でクラッシュしないことを確認する
            double mouseUpX = ac.DisplayImageWidth / 2;
            double mouseUpY = ac.DisplayImageHeight + 30;
            System.Windows.Point mouseUp = new System.Windows.Point(mouseUpX, mouseUpY);
            ac.PublishEvent(mouseUp);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoProcessingAuxiliaryLineIfMouseDownOuterAuxiliaryLine()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線を縮小して補助線外部のスペースを作り、そのスペースでテストする
            int sizeChangeWidth = -100;
            int sizeChangeHeight = -5;
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, sizeChangeWidth, sizeChangeHeight, true);

            double mouseDownX = ac.DisplayImageWidth + (sizeChangeWidth/2);
            double mouseDownY = ac.DisplayImageHeight + (sizeChangeHeight/2);
            System.Windows.Point mouseDown = new System.Windows.Point(mouseDownX, mouseDownY);
            System.Windows.Point mouseUp = mouseDown;
            ac.SetEvent(mouseDown);
            ac.PublishEvent(mouseUp);
        }

        #endregion

        #region "Undo: 初期値"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoNoProcessBeforeAtLeastOneOperation()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            int beforeWidth = ac.AuxiliaryWidth;
            int beforeHeight = ac.AuxiliaryHeight;

            ac.CancelEvent();

            Assert.AreEqual(beforeWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(beforeHeight, ac.AuxiliaryHeight);
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);
        }

        #endregion

        #region "Undo: キー操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoInputCursorKeyDown()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Down);
            Assert.AreEqual(1, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);

            ac.CancelEvent();
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoInputCursorKeyUp()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Up);
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);

            ac.CancelEvent();
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoInputCursorKeyLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Left);
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);

            ac.CancelEvent();
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoInputCursorKeyRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 矩形を小さくして、右キーで矩形を移動できるだけのスペースを作る
            AuxiliaryLineTestData testData
                = new AuxiliaryLineChangeSizeBottomRight().ChangeSize(ac,
                                                                      -100,
                                                                      -5,
                                                                      true);

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Right);
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(1, ac.AuxiliaryLeftRelativeImage);

            ac.CancelEvent();
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);
        }

        #endregion

        #region "Undo: マウス操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoSizeChangeOperationWhereBottomRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            List<AuxiliaryLineParameter> list = new List<AuxiliaryLineParameter>();
            list.Add(ac.CloneParameter());

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -100, -5, true);
            list.Add(ac.CloneParameter());

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -5, -100, false);

            ac.CancelEvent();
            AreParameterEqual(list[1], ac);

            ac.CancelEvent();
            AreParameterEqual(list[0], ac);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoSizeChangeOperationWhereBottomLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            List<AuxiliaryLineParameter> list = new List<AuxiliaryLineParameter>();
            list.Add(ac.CloneParameter());

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -100, -5, true);
            list.Add(ac.CloneParameter());

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -5, -100, false);

            ac.CancelEvent();
            AreParameterEqual(list[1], ac);

            ac.CancelEvent();
            AreParameterEqual(list[0], ac);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoSizeChangeOperationWhereTopRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            List<AuxiliaryLineParameter> list = new List<AuxiliaryLineParameter>();
            list.Add(ac.CloneParameter());

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -100, -5, true);
            list.Add(ac.CloneParameter());

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -5, -100, false);

            ac.CancelEvent();
            AreParameterEqual(list[1], ac);

            ac.CancelEvent();
            AreParameterEqual(list[0], ac);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoSizeChangeOperationWhereTopLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            List<AuxiliaryLineParameter> list = new List<AuxiliaryLineParameter>();
            list.Add(ac.CloneParameter());

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, -100, -5, true);
            list.Add(ac.CloneParameter());

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, -5, -100, false);

            ac.CancelEvent();
            AreParameterEqual(list[1], ac);

            ac.CancelEvent();
            AreParameterEqual(list[0], ac);
        }

        private void AreParameterEqual(AuxiliaryLineParameter expected, AuxiliaryController actual)
        {
            Assert.AreEqual(expected.Width, actual.AuxiliaryWidth);
            Assert.AreEqual(expected.Height, actual.AuxiliaryHeight);
            Assert.AreEqual(expected.Top, actual.AuxiliaryTopRelativeImage);
            Assert.AreEqual(expected.Left, actual.AuxiliaryLeftRelativeImage);
        }

        #endregion

        #region "Redo: 初期値"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestRedoNoProcessBeforeAtLeastOneOperation()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            int beforeWidth = ac.AuxiliaryWidth;
            int beforeHeight = ac.AuxiliaryHeight;

            ac.RedoEvent();

            Assert.AreEqual(beforeWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(beforeHeight, ac.AuxiliaryHeight);
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestRedoNoProcessBeforeAtLeastOneOperationCanceled()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            int beforeWidth = ac.AuxiliaryWidth;
            int beforeHeight = ac.AuxiliaryHeight;
            int beforeTop = ac.AuxiliaryTopRelativeImage;
            int beforeLeft = ac.AuxiliaryLeftRelativeImage;

            // CancelせずにRedoを実行する
            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Left);
            ac.RedoEvent();

            Assert.AreEqual(beforeWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(beforeHeight, ac.AuxiliaryHeight);
            Assert.AreEqual(beforeTop, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(beforeLeft, ac.AuxiliaryLeftRelativeImage);
        }

        #endregion

    }
}
