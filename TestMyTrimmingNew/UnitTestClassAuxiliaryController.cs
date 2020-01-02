using System;
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
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
                                                                   widthRatio,
                                                                   heightRatio);

            // 補助線矩形が画像からはみ出るような操作の場合、矩形サイズが画像一杯のサイズになるよう制御する
            // ユーザー操作としてあり得るのは画像より小さい補助線矩形を画像一杯に合わせる操作
            //  -> まず補助線矩形を画像より小さくする
            int willDecreaseWidthPixel = -100;
            int willDecreaseHeightPixel = -5;
            ChangeAuxiliaryLineSizeWhereBottomRight(ac,
                                                    willDecreaseWidthPixel,
                                                    willDecreaseHeightPixel,
                                                    true);

            // 実際に画像からはみ出るような操作をする
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
            int beforeWidth = ac.AuxiliaryWidth;
            int beforeHeight = ac.AuxiliaryHeight;

            // 操作
            double mouseUpX = (double)ac.AuxiliaryWidth + (double)mouseMoveWidthPixel;
            double mouseUpY = (double)ac.AuxiliaryHeight + (double)mouseMoveHeightPixel;
            System.Windows.Point mouseDown = new System.Windows.Point((double)ac.AuxiliaryWidth, (double)ac.AuxiliaryHeight);
            System.Windows.Point mouseUp = new System.Windows.Point(mouseUpX, mouseUpY);
            ac.SetEvent(mouseDown);
            ac.PublishEvent(mouseUp);

            // X方向操作距離とY方向操作距離を、矩形の縦横比率に合わせる
            int changeSizeWidth = mouseMoveWidthPixel;
            int changeSizeHeight = mouseMoveHeightPixel;
            if (isWidthMuchLongerThanHeight)
            {
                changeSizeHeight = (int)Math.Round((double)changeSizeWidth / ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                changeSizeWidth = (int)Math.Round((double)changeSizeHeight * ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }

            int maxChangeSizeWidth = ac.DisplayImageWidth - beforeWidth - beforeLeftRelativeImage - Common.AuxiliaryLineThickness + 1;
            int maxChangeHeight = ac.DisplayImageHeight - beforeHeight - beforeTopRelativeImage - Common.AuxiliaryLineThickness + 1;
            if (((beforeWidth+changeSizeWidth) < 0) || ((beforeHeight + changeSizeHeight) < 0))
            {
                // 原点が変わるようなサイズ変更が要求されても、サイズ変更しない
                changeSizeWidth = 0;
                changeSizeHeight = 0;
            }
            else if(changeSizeWidth > maxChangeSizeWidth || changeSizeHeight > maxChangeHeight)
            {
                // 画像からはみ出るようなサイズ変更が要求された場合、代わりに画像一杯まで広げる
                if (isWidthMuchLongerThanHeight)
                {
                    changeSizeWidth = maxChangeSizeWidth;
                    changeSizeHeight = (int)Math.Round((double)changeSizeWidth / ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    changeSizeHeight = maxChangeHeight;
                    changeSizeWidth = (int)Math.Round((double)changeSizeHeight * ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
                }
            }

            // 右下の点の操作であれば、原点は変わらないのが正解
            Assert.AreEqual(beforeLeftRelativeImage, ac.AuxiliaryLeftRelativeImage);
            Assert.AreEqual(beforeTopRelativeImage, ac.AuxiliaryTopRelativeImage);

            // 変更後サイズの確認
            int expectSizeWidth = beforeWidth + changeSizeWidth;
            int expectSizeHeight = beforeHeight + changeSizeHeight;
            Assert.AreEqual(expectSizeWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(expectSizeHeight, ac.AuxiliaryHeight);
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
            int willDecreaseWidthPixel = 100;
            int willDecreaseHeightPixel = -5;
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac,
                                                   willDecreaseWidthPixel,
                                                   willDecreaseHeightPixel,
                                                   true);

            // Height基準でWidthを変更するよう、Height >> Width となる値を設定
            willDecreaseWidthPixel = -5;
            willDecreaseHeightPixel = -100;
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac,
                                                   willDecreaseWidthPixel,
                                                   willDecreaseHeightPixel,
                                                   false);
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
            int willDecreaseWidthPixel = -1200;
            int willDecreaseHeightPixel = -10;
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac,
                                                   willDecreaseWidthPixel,
                                                   willDecreaseHeightPixel,
                                                   true);

            willDecreaseWidthPixel = -10;
            willDecreaseHeightPixel = -1200;
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac,
                                                   willDecreaseWidthPixel,
                                                   willDecreaseHeightPixel,
                                                   false);
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
            int willDecreaseWidthPixel = 100;
            int willDecreaseHeightPixel = -5;
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac,
                                                   willDecreaseWidthPixel,
                                                   willDecreaseHeightPixel,
                                                   true);

            // 実際に画像からはみ出るような操作をする
            int willIncreaseWidthPixel = -200;
            int willIncreaseHeightPixel = 10;
            ChangeAuxiliaryLineSizeWhereBottomLeft(ac,
                                                   willIncreaseWidthPixel,
                                                   willIncreaseHeightPixel,
                                                   true);
        }

        private void ChangeAuxiliaryLineSizeWhereBottomLeft(AuxiliaryController ac,
                                                            int mouseMoveWidthPixel,
                                                            int mouseMoveHeightPixel,
                                                            bool isWidthMuchLongerThanHeight)
        {
            // 操作前の値を保持
            int beforeLeftRelativeImage = ac.AuxiliaryLeftRelativeImage;
            int beforeTopRelativeImage = ac.AuxiliaryTopRelativeImage;
            int beforeWidth = ac.AuxiliaryWidth;
            int beforeHeight = ac.AuxiliaryHeight;

            // 操作
            double mouseUpX = (double)ac.AuxiliaryLeftRelativeImage + (double)mouseMoveWidthPixel;
            double mouseUpY = (double)ac.AuxiliaryHeight + (double)mouseMoveHeightPixel;
            System.Windows.Point mouseDownRelatedAuxiliaryLine = new System.Windows.Point(0, (double)ac.AuxiliaryHeight);
            System.Windows.Point mouseUpRelatedAuxiliaryLine = new System.Windows.Point(mouseUpX, mouseUpY);
            ac.SetEvent(mouseDownRelatedAuxiliaryLine);
            ac.PublishEvent(mouseUpRelatedAuxiliaryLine);

            // X方向操作距離とY方向操作距離を、矩形の縦横比率に合わせる
            int changeSizeWidth = -mouseMoveWidthPixel;
            int changeSizeHeight = mouseMoveHeightPixel;
            if (isWidthMuchLongerThanHeight)
            {
                changeSizeHeight = (int)Math.Round((double)changeSizeWidth / ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                changeSizeWidth = (int)Math.Round((double)changeSizeHeight * ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
            }

            int maxChangeSizeWidth = beforeLeftRelativeImage - Common.AuxiliaryLineThickness + 1;
            int maxChangeSizeHeight = ac.DisplayImageHeight - beforeHeight - beforeTopRelativeImage - Common.AuxiliaryLineThickness + 1;
            if ((-changeSizeWidth > beforeWidth) || (-changeSizeHeight > beforeHeight))
            {
                // 原点が変わるようなサイズ変更が要求されても、サイズ変更しない
                changeSizeWidth = 0;
                changeSizeHeight = 0;
            }
            else if (changeSizeWidth > maxChangeSizeWidth || changeSizeHeight > maxChangeSizeHeight)
            {
                // 画像からはみ出るようなサイズ変更が要求された場合、代わりに画像一杯まで広げる
                if (isWidthMuchLongerThanHeight)
                {
                    changeSizeWidth = maxChangeSizeWidth;
                    changeSizeHeight = (int)Math.Round((double)changeSizeWidth / ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    changeSizeHeight = maxChangeSizeHeight;
                    changeSizeWidth = (int)Math.Round((double)changeSizeHeight * ac.AuxiliaryRatio, 0, MidpointRounding.AwayFromZero);
                }
            }

            // 左下の点の操作であれば、原点はLeftだけ変わる
            int expectLeft = beforeLeftRelativeImage - changeSizeWidth;
            Assert.AreEqual(expectLeft, ac.AuxiliaryLeftRelativeImage);
            Assert.AreEqual(beforeTopRelativeImage, ac.AuxiliaryTopRelativeImage);

            // 変更後サイズの確認
            int expectWidth = beforeWidth + changeSizeWidth;
            int expectHeight = beforeHeight + changeSizeHeight;
            Assert.AreEqual(expectWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(expectHeight, ac.AuxiliaryHeight);
        }

        #endregion
    }
}
