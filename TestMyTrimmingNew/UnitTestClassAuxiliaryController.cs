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
            int fittedWidth = ac.DisplayImageWidth;
            int fittedHeight = (int)((double)fittedWidth / ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryRight);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryBottom);
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
            Assert.AreEqual(fittedWidth, ac.AuxiliaryRight);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryBottom);
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
            Assert.AreEqual(fittedWidth, ac.AuxiliaryRight);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryBottom);
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
            Assert.AreEqual(0, ac.AuxiliaryTop);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineStayInImageAfterInputCursorKeyDownIfAuxiliaryLineBottomIsImageBottom()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            int enableDownRange = ac.DisplayImageHeight - ac.AuxiliaryBottom;
            Keys.EnableKeys down = Keys.EnableKeys.Down;
            ac.SetEvent();
            for (int i = 1; i <= enableDownRange; i++)
            {
                ac.PublishEvent(down);
                Assert.AreEqual(i, ac.AuxiliaryTop);
            }

            int maxDownRange = enableDownRange - Common.AuxiliaryLineThickness + 1;
            Assert.AreEqual(maxDownRange, ac.AuxiliaryTop);

            ac.PublishEvent(down);
            Assert.AreEqual(maxDownRange, ac.AuxiliaryTop);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineOriginAfterInputCursorKeyLeftIfAuxiliaryLineOriginLeftIsZero()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Left);
            Assert.AreEqual(0, ac.AuxiliaryLeft);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineStayInImageAfterInputCursorKeyRightIfAuxiliaryLineRightIsImageRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            int enableRightRange = ac.DisplayImageWidth - ac.AuxiliaryRight;
            Keys.EnableKeys right = Keys.EnableKeys.Right;
            ac.SetEvent();
            for (int i = 1; i <= enableRightRange; i++)
            {
                ac.PublishEvent(right);
                // 原点を右に移動させるので、原点位置=Leftと比較する
                Assert.AreEqual(i, ac.AuxiliaryLeft);
            }

            int maxRightRange = enableRightRange - Common.AuxiliaryLineThickness + 1;
            Assert.AreEqual(maxRightRange, ac.AuxiliaryLeft);

            ac.PublishEvent(right);
            Assert.AreEqual(maxRightRange, ac.AuxiliaryLeft);
        }

        #endregion

        #region "移動: マウス操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectAuxiliaryLineLeftAndTopIfMoveByMouse()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // まず矩形を小さくして移動テストできるようにする(Width基準とする)
            int mouseUpPixelX = -(ac.AuxiliaryRight / 2);
            int mouseUpPixelY = -5;
            double mouseUpX = (double)ac.AuxiliaryRight + (double)mouseUpPixelX;
            double mouseUpY = (double)ac.AuxiliaryBottom + (double)mouseUpPixelY;
            System.Windows.Point mouseDown = new System.Windows.Point((double)ac.AuxiliaryRight, (double)ac.AuxiliaryBottom);
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
            int width = ac.AuxiliaryRight - ac.AuxiliaryLeft - ac.AuxiliaryLineThickness + 1;
            int height = ac.AuxiliaryBottom - ac.AuxiliaryTop - ac.AuxiliaryLineThickness + 1;
            int startMovePointX = ac.AuxiliaryLeft + (width / 2);
            int startMovePointY = ac.AuxiliaryTop + (height / 2);
            int finishMovePointX = startMovePointX + moveX;
            int finishMovePointY = startMovePointY + moveY;
            System.Windows.Point startMovePoint = new System.Windows.Point(startMovePointX, startMovePointY);
            System.Windows.Point finishMovePoint = new System.Windows.Point(finishMovePointX, finishMovePointY);

            int correctLeft = ac.AuxiliaryLeft + moveX;
            int correctTop = ac.AuxiliaryTop + moveY;
            MoveAuxiliaryLine(ac, startMovePoint, finishMovePoint, correctLeft, correctTop);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageIfMoveByMouseTooFarMoveDistanceToLeftTop()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形を大きく移動しようとして矩形が画像からはみ出る場合、画像一杯までの移動に制限する
            // テストとして選ぶ値の基準: ディスプレイより大きい距離を移動しようとするなら、確実に画像からはみ出る
            System.Windows.Point startMovePoint = new System.Windows.Point(ac.AuxiliaryRight / 2, ac.AuxiliaryBottom / 2);
            int tooMoveX = 3000;
            int tooMoveY = 3000;

            // 左上方向テスト
            System.Windows.Point finishMovePoint = new System.Windows.Point(-tooMoveX, -tooMoveY);
            int correctLeft = 0;
            int correctTop = 0;
            MoveAuxiliaryLine(ac, startMovePoint, finishMovePoint, correctLeft, correctTop);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageIfMoveByMouseTooFarMoveDistanceToRightTop()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            System.Windows.Point startMovePoint = new System.Windows.Point(ac.AuxiliaryRight / 2, ac.AuxiliaryBottom / 2);
            int tooMoveX = 3000;
            int tooMoveY = 3000;

            // 右上方向テスト
            System.Windows.Point finishMovePoint = new System.Windows.Point(tooMoveX, -tooMoveY);
            int correctLeft = ac.DisplayImageWidth - ac.AuxiliaryRight;
            int correctTop = 0;
            MoveAuxiliaryLine(ac, startMovePoint, finishMovePoint, correctLeft, correctTop);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageIfMoveByMouseTooFarMoveDistanceToLeftBottom()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            System.Windows.Point startMovePoint = new System.Windows.Point(ac.AuxiliaryRight / 2, ac.AuxiliaryBottom / 2);
            int tooMoveX = 3000;
            int tooMoveY = 3000;

            // 左下方向テスト
            System.Windows.Point finishMovePoint = new System.Windows.Point(-tooMoveX, tooMoveY);
            int correctLeft = 0;
            int correctTop = ac.DisplayImageHeight - ac.AuxiliaryBottom;
            MoveAuxiliaryLine(ac, startMovePoint, finishMovePoint, correctLeft, correctTop);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageIfMoveByMouseTooFarMoveDistanceToRightBottom()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            System.Windows.Point startMovePoint = new System.Windows.Point(ac.AuxiliaryRight / 2, ac.AuxiliaryBottom / 2);
            int tooMoveX = 3000;
            int tooMoveY = 3000;

            // 右下方向テスト
            System.Windows.Point finishMovePoint = new System.Windows.Point(tooMoveX, tooMoveY);
            int correctLeft = ac.DisplayImageWidth - ac.AuxiliaryRight;
            int correctTop = ac.DisplayImageHeight - ac.AuxiliaryBottom;
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
            Assert.AreEqual(ac.AuxiliaryLeft, correctLeftAfterMove);
            Assert.AreEqual(ac.AuxiliaryTop, correctTopAfterMove);
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
        public void TestAuxiliaryLineFitImageWhenChangeWidthSizeTooLongerThanImageSizeBottomRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像下に近いとき、横方向に大きく拡大して矩形サイズが画像一杯のサイズになるよう制御する
            // まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -5, -200, false);

            // 次に画像下に寄せる
            int moveX = 0;
            int moveY = 180;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, 250, 5, true);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeHeightSizeTooLongerThanImageSizeBottomRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像右に近いとき、下方向に大きく拡大して矩形サイズが画像一杯のサイズになるよう制御する
            // まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -200, -5, true);

            // 次に画像右に寄せる
            int moveX = 180;
            int moveY = 0;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, 5, 250, false);
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
                = new AuxiliaryLineChangeSizeBottomRight(ac).ChangeSize(changeSizeWidth,
                                                                        changeSizeHeight,
                                                                        isWidthMuchLongerThanHeight);
            AreParameterEqual(testData, ac);
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

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeWidthSizeTooLongerThanImageSizeBottomLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像下に近いとき、横方向に大きく拡大しても矩形サイズが画像一杯のサイズになるよう制御する
            // まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -200, -5, true);

            // 次に画像下に寄せる
            int moveX = 0;
            int moveY = 180;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, 250, 5, true);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeHeightSizeTooLongerThanImageSizeBottomLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像左に近いとき、下方向に大きく拡大して矩形サイズが画像一杯のサイズになるよう制御する
            // まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, -200, -5, true);

            // 次に画像左に少し移動し、拡大可能にする
            int moveX = -50;
            int moveY = 0;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac, 5, 250, false);
        }

        private void ChangeAuxiliaryLineSizeWhereBottomLeft(AuxiliaryController ac,
                                                            int changeSizeWidth,
                                                            int changeSizeHeight,
                                                            bool isWidthMuchLongerThanHeight)
        {
            AuxiliaryLineTestData testData
                = new AuxiliaryLineChangeSizeBottomLeft(ac).ChangeSize(changeSizeWidth,
                                                                       changeSizeHeight,
                                                                       isWidthMuchLongerThanHeight);
            AreParameterEqual(testData, ac);
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

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeWidthSizeTooLongerThanImageSizeTopRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像上に近いとき、横方向に大きく拡大しても矩形サイズが画像一杯のサイズになるよう制御する
            // まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -200, -5, true);

            // 次に画像上に寄せる
            int moveX = 0;
            int moveY = -100;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereTopRight(ac, 250, 5, true);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeHeightSizeTooLongerThanImageSizeTopRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像右に近いとき、上方向に大きく拡大しても矩形サイズが画像一杯のサイズになるよう制御する
            // まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -200, -5, true);

            // 次に画像右に寄せる
            int moveX = 100;
            int moveY = 0;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereTopRight(ac, 5, 250, false);
        }

        private void ChangeAuxiliaryLineSizeWhereTopRight(AuxiliaryController ac,
                                                          int changeSizeWidth,
                                                          int changeSizeHeight,
                                                          bool isWidthMuchLongerThanHeight)
        {
            AuxiliaryLineTestData testData
                = new AuxiliaryLineChangeSizeTopRight(ac).ChangeSize(changeSizeWidth,
                                                                     changeSizeHeight,
                                                                     isWidthMuchLongerThanHeight);
            AreParameterEqual(testData, ac);
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

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeWidthSizeTooLongerThanImageSizeTopLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像上に近いとき、横方向に大きく拡大しても矩形サイズが画像一杯のサイズになるよう制御する
            // まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, -200, -5, true);

            // 次に画像上に寄せる
            int moveX = 0;
            int moveY = -100;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, 250, 5, true);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineFitImageWhenChangeHeightSizeTooLongerThanImageSizeTopLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 補助線矩形が画像左に近いとき、上方向に大きく拡大しても矩形サイズが画像一杯のサイズになるよう制御する
            // まず補助線矩形を画像より小さくする
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, -200, -5, true);

            // 次に画像左に寄せる
            int moveX = -100;
            int moveY = 0;
            MoveAuxiliaryLine(ac, moveX, moveY);

            // 実際に画像からはみ出るような操作をする
            ChangeAuxiliaryLineSizeWhereTopLeft(ac, 5, 250, false);
        }

        private void ChangeAuxiliaryLineSizeWhereTopLeft(AuxiliaryController ac,
                                                         int changeSizeWidth,
                                                         int changeSizeHeight,
                                                         bool isWidthMuchLongerThanHeight)
        {
            AuxiliaryLineTestData testData
                = new AuxiliaryLineChangeSizeTopLeft(ac).ChangeSize(changeSizeWidth,
                                                                    changeSizeHeight,
                                                                    isWidthMuchLongerThanHeight);
            AreParameterEqual(testData, ac);
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
            int beforeWidth = ac.AuxiliaryRight;
            int beforeHeight = ac.AuxiliaryBottom;

            ac.CancelEvent();

            Assert.AreEqual(beforeWidth, ac.AuxiliaryRight);
            Assert.AreEqual(beforeHeight, ac.AuxiliaryBottom);
            Assert.AreEqual(0, ac.AuxiliaryTop);
            Assert.AreEqual(0, ac.AuxiliaryLeft);
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
            Assert.AreEqual(1, ac.AuxiliaryTop);
            Assert.AreEqual(0, ac.AuxiliaryLeft);

            ac.CancelEvent();
            Assert.AreEqual(0, ac.AuxiliaryTop);
            Assert.AreEqual(0, ac.AuxiliaryLeft);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoInputCursorKeyUp()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Up);
            Assert.AreEqual(0, ac.AuxiliaryTop);
            Assert.AreEqual(0, ac.AuxiliaryLeft);

            ac.CancelEvent();
            Assert.AreEqual(0, ac.AuxiliaryTop);
            Assert.AreEqual(0, ac.AuxiliaryLeft);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoInputCursorKeyLeft()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Left);
            Assert.AreEqual(0, ac.AuxiliaryTop);
            Assert.AreEqual(0, ac.AuxiliaryLeft);

            ac.CancelEvent();
            Assert.AreEqual(0, ac.AuxiliaryTop);
            Assert.AreEqual(0, ac.AuxiliaryLeft);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoInputCursorKeyRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 矩形を小さくして、右キーで矩形を移動できるだけのスペースを作る
            AuxiliaryLineTestData testData
                = new AuxiliaryLineChangeSizeBottomRight(ac).ChangeSize(-100, -5, true);

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Right);
            Assert.AreEqual(0, ac.AuxiliaryTop);
            Assert.AreEqual(1, ac.AuxiliaryLeft);

            ac.CancelEvent();
            Assert.AreEqual(0, ac.AuxiliaryTop);
            Assert.AreEqual(0, ac.AuxiliaryLeft);
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
            Assert.AreEqual(expected.LeftTop, actual.AuxiliaryLeftTop);
            Assert.AreEqual(expected.LeftBottom, actual.AuxiliaryLeftBottom);
            Assert.AreEqual(expected.RightTop, actual.AuxiliaryRightTop);
            Assert.AreEqual(expected.RightBottom, actual.AuxiliaryRightBottom);
            Assert.AreEqual(expected.Degree, actual.AuxiliaryDegree);
        }

        private void AreParameterEqual(AuxiliaryLineTestData expected, AuxiliaryController actual)
        {
            Assert.AreEqual(expected.ExpectLeftTop, actual.AuxiliaryLeftTop);
            Assert.AreEqual(expected.ExpectLeftBottom, actual.AuxiliaryLeftBottom);
            Assert.AreEqual(expected.ExpectRightTop, actual.AuxiliaryRightTop);
            Assert.AreEqual(expected.ExpectRightBottom, actual.AuxiliaryRightBottom);
            Assert.AreEqual(expected.ExpectDegree, actual.AuxiliaryDegree);
        }

        #endregion

        #region "Redo: 初期値"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestRedoNoProcessBeforeAtLeastOneOperation()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            int beforeRight = ac.AuxiliaryRight;
            int beforeBottom = ac.AuxiliaryBottom;

            ac.RedoEvent();

            Assert.AreEqual(beforeRight, ac.AuxiliaryRight);
            Assert.AreEqual(beforeBottom, ac.AuxiliaryBottom);
            Assert.AreEqual(0, ac.AuxiliaryTop);
            Assert.AreEqual(0, ac.AuxiliaryLeft);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestRedoNoProcessBeforeAtLeastOneOperationCanceled()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            int beforeRight = ac.AuxiliaryRight;
            int beforeBottom = ac.AuxiliaryBottom;
            int beforeTop = ac.AuxiliaryTop;
            int beforeLeft = ac.AuxiliaryLeft;

            // CancelせずにRedoを実行する
            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Left);
            ac.RedoEvent();

            Assert.AreEqual(beforeRight, ac.AuxiliaryRight);
            Assert.AreEqual(beforeBottom, ac.AuxiliaryBottom);
            Assert.AreEqual(beforeTop, ac.AuxiliaryTop);
            Assert.AreEqual(beforeLeft, ac.AuxiliaryLeft);
        }

        #endregion

        #region "Redo: キー操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestRedoInputCursorKeyDown()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            List<AuxiliaryLineParameter> list = new List<AuxiliaryLineParameter>();
            list.Add(ac.CloneParameter());

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Down);
            list.Add(ac.CloneParameter());

            ac.CancelEvent();
            AreParameterEqual(list[0], ac);

            ac.RedoEvent();
            AreParameterEqual(list[1], ac);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestRedoInputCursorKeyUp()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            List<AuxiliaryLineParameter> list = new List<AuxiliaryLineParameter>();
            list.Add(ac.CloneParameter());

            // 上キー操作で上に移動するようあらかじめ下に移動しておく
            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Down);
            list.Add(ac.CloneParameter());

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Up);
            list.Add(ac.CloneParameter());

            ac.CancelEvent();
            AreParameterEqual(list[1], ac);

            ac.RedoEvent();
            AreParameterEqual(list[2], ac);
        }

        #endregion

        #region "Redo: マウス操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestRedoSizeChangeOperationWhereTopRight()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            List<AuxiliaryLineParameter> list = new List<AuxiliaryLineParameter>();
            list.Add(ac.CloneParameter());

            // Width基準でHeightを変更するよう、Width >> height となる値を設定
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -100, -5, true);
            list.Add(ac.CloneParameter());

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -5, -100, false);
            list.Add(ac.CloneParameter());

            ac.CancelEvent();
            AreParameterEqual(list[1], ac);

            ac.CancelEvent();
            AreParameterEqual(list[0], ac);

            ac.RedoEvent();
            AreParameterEqual(list[1], ac);

            ac.RedoEvent();
            AreParameterEqual(list[2], ac);
        }

        #endregion

        #region "Undo/Redo: 複雑な操作"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoRedoIfNewOperationAfterUndo()
        {
            // 操作 → Undo → 別操作 → Redoしないことを確認する
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            List<AuxiliaryLineParameter> list = new List<AuxiliaryLineParameter>();
            list.Add(ac.CloneParameter());

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Down);
            list.Add(ac.CloneParameter());

            ac.CancelEvent();
            AreParameterEqual(list[0], ac);

            // 適当に別操作
            ChangeAuxiliaryLineSizeWhereTopRight(ac, -100, -5, true);
            list[1] = ac.CloneParameter();

            ac.RedoEvent();
            AreParameterEqual(list[1], ac);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoAndRedo()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();
            List<AuxiliaryLineParameter> list = new List<AuxiliaryLineParameter>();
            list.Add(ac.CloneParameter());

            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Down);
            list.Add(ac.CloneParameter());

            ChangeAuxiliaryLineSizeWhereTopRight(ac, -100, -5, true);
            list.Add(ac.CloneParameter());

            ChangeAuxiliaryLineSizeWhereTopRight(ac, -5, -100, false);
            list.Add(ac.CloneParameter());

            ac.CancelEvent();
            AreParameterEqual(list[2], ac);

            ac.CancelEvent();
            AreParameterEqual(list[1], ac);

            ac.RedoEvent();
            AreParameterEqual(list[2], ac);

            ChangeAuxiliaryLineSizeWhereTopRight(ac, 50, 5, true);
            list.Add(ac.CloneParameter());

            ac.CancelEvent();
            AreParameterEqual(list[2], ac);

            ac.RedoEvent();
            AreParameterEqual(list[4], ac);
        }

        #endregion

        #region "回転"

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineRotatePlus()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 縮小＆移動して回転できるスペースを作る
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -400, -5, true);
            MoveAuxiliaryLine(ac, 200, 200);

            RotateAuxiliaryLine(ac, 20);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineRotateMinus()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 縮小＆移動して回転できるスペースを作る
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -400, -5, true);
            MoveAuxiliaryLine(ac, 200, 200);

            RotateAuxiliaryLine(ac, -20);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestAuxiliaryLineNoRotateIfAuxiliaryLineOutOfRangeAfterRotate()
        {
            // 回転すると画像からはみ出る場合は回転しない
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // 縮小＆移動して回転できるスペースを作る
            ChangeAuxiliaryLineSizeWhereBottomRight(ac, -100, -5, true);
            MoveAuxiliaryLine(ac, 100, 80);

            AuxiliaryLineParameter before = ac.CloneParameter();
            AuxiliaryLineTestData testData
                = new AuxiliaryLineRotate().Execute(ac, 20);

            AreParameterEqual(before, ac);
        }

        private void RotateAuxiliaryLine(AuxiliaryController ac, int degree)
        {
            AuxiliaryLineTestData testData
                = new AuxiliaryLineRotate().Execute(ac, degree);
            AreParameterEqual(testData, ac);
        }

        #endregion
    }
}
