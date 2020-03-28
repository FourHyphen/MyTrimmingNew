using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTestClassCommonImage
    {
        #region 画像のresize & open

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestEqualOfChangedImageSizeWidthAndHeight()
        {
            // 数字の根拠無し
            int newWidth = 600;
            int newHeight = 350;

            Bitmap img = new Bitmap(Common.TestResourceImage001Path);
            Bitmap newImg = MyTrimmingNew.common.Image.CreateBitmap(img, newWidth, newHeight);
            Assert.AreEqual(newWidth, newImg.Width);
            Assert.AreEqual(newHeight, newImg.Height);
        }

        #endregion

        #region 保存

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectImageSizeAfterSaveTrimImage()
        {
            // 数字の根拠無し
            // 1/1スケールだと(x=400, y=600)からwidth=800, height=400の範囲を切り取る設定
            double ratio = 0.5;
            int seemingTrimLeft = 200;
            int seemingTrimTop = 300;
            int seemingTrimRight = 600;
            int seemingTrimBottom = 500;

            System.IO.File.Delete(Common.TestToSaveTrimImagePath001);

            Bitmap img = new Bitmap(Common.TestResourceImage001Path);
            MyTrimmingNew.common.Image.SaveTrimImage(img,
                                                     Common.TestToSaveTrimImagePath001,
                                                     seemingTrimLeft,
                                                     seemingTrimTop,
                                                     seemingTrimRight,
                                                     seemingTrimBottom,
                                                     ratio);

            Bitmap trimmedImg = new Bitmap(Common.TestToSaveTrimImagePath001);

            int seemingTrimWidth = seemingTrimRight - seemingTrimLeft;
            int seemingTrimHeight = seemingTrimBottom - seemingTrimTop;
            int ansLeft = (int)((double)seemingTrimLeft / ratio);
            int ansTop = (int)((double)seemingTrimTop / ratio);
            int ansTrimWidth = (int)((double)seemingTrimWidth / ratio);
            int ansTrimHeight = (int)((double)seemingTrimHeight / ratio);
            Rectangle rect = new Rectangle(ansLeft, ansTop, ansTrimWidth, ansTrimHeight);
            Bitmap ansImg = img.Clone(rect, img.PixelFormat);

            EqualImage(trimmedImg, ansImg);
        }

        #endregion

        #region 回転して保存

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoCrashImageSizeAfterSaveTrimImageWithRotatePlus()
        {
            // 第一段階: まずクラッシュしなければよしとする
            // 回転: 正方向(数字の根拠無し)
            ImageRotateAndSave(openImagePath: Common.TestResourceImage001Path,
                               saveImagePath: Common.TestToSaveTrimImagePath002,
                               beforeRotateSeemingLeftTop: new Point(200, 200),
                               beforeRotateSeemingLeftBottom: new Point(200, 400),
                               beforeRotateSeemingRightTop: new Point(600, 200),
                               beforeRotateSeemingRightBottom: new Point(600, 400),
                               ratio: 0.5,
                               degree: 20);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestNoCrashImageSizeAfterSaveTrimImageWithRotateMinus()
        {
            // 第一段階: まずクラッシュしなければよしとする
            // 回転: 負方向(数字の根拠無し)
            ImageRotateAndSave(openImagePath: Common.TestResourceImage001Path,
                               saveImagePath: Common.TestToSaveTrimImagePath003,
                               beforeRotateSeemingLeftTop: new Point(400, 800),
                               beforeRotateSeemingLeftBottom: new Point(400, 1200),
                               beforeRotateSeemingRightTop: new Point(1200, 800),
                               beforeRotateSeemingRightBottom: new Point(1200, 1200),
                               ratio: 0.5,
                               degree: -20);
        }

        private void ImageRotateAndSave(String openImagePath,
                                        String saveImagePath,
                                        Point beforeRotateSeemingLeftTop,
                                        Point beforeRotateSeemingLeftBottom,
                                        Point beforeRotateSeemingRightTop,
                                        Point beforeRotateSeemingRightBottom,
                                        double ratio,
                                        int degree)
        {
            Point rotateLeftTop = Common.CalcRotatePoint(beforeRotateSeemingLeftTop, degree);
            Point rotateLeftBottom = Common.CalcRotatePoint(beforeRotateSeemingLeftBottom, degree);
            Point rotateRightTop = Common.CalcRotatePoint(beforeRotateSeemingRightTop, degree);
            Point rotateRightBottom = Common.CalcRotatePoint(beforeRotateSeemingRightBottom, degree);

            System.IO.File.Delete(saveImagePath);

            Bitmap img = new Bitmap(openImagePath);
            MyTrimmingNew.common.Image.SaveTrimImage(img,
                                                     saveImagePath,
                                                     rotateLeftTop,
                                                     rotateLeftBottom,
                                                     rotateRightTop,
                                                     rotateRightBottom,
                                                     ratio,
                                                     degree);
        }

        // TODO: テストが完成するまでignoreにする
        [TestMethod]
        [Ignore]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectImageSizeAfterSaveTrimImageWithRotatePlus()
        {
            // 数字の根拠無し
            double ratio = 0.5;
            Point seemingLeftTop = new Point(200, 200);
            Point seemingLeftBottom = new Point(200, 400);
            Point seemingRightTop = new Point(600, 200);
            Point seemingRightBottom = new Point(600, 400);

            int degree = 20;
            Point rotateLeftTop = Common.CalcRotatePoint(seemingLeftTop, degree);
            Point rotateLeftBottom = Common.CalcRotatePoint(seemingLeftBottom, degree);
            Point rotateRightTop = Common.CalcRotatePoint(seemingRightTop, degree);
            Point rotateRightBottom = Common.CalcRotatePoint(seemingRightBottom, degree);

            System.IO.File.Delete(Common.TestToSaveTrimImagePath004);

            Bitmap img = new Bitmap(Common.TestResourceImage001Path);
            MyTrimmingNew.common.Image.SaveTrimImage(img,
                                                     Common.TestToSaveTrimImagePath004,
                                                     rotateLeftTop,
                                                     rotateLeftBottom,
                                                     rotateRightTop,
                                                     rotateRightBottom,
                                                     ratio,
                                                     degree);

            Bitmap trimmedImg = new Bitmap(Common.TestToSaveTrimImagePath004);
            EqualImage(img, trimmedImg, rotateLeftTop, rotateLeftBottom, rotateRightTop, rotateRightBottom, ratio, degree);
        }

        private void EqualImage(Bitmap img1, Bitmap img2)
        {
            Assert.AreEqual(img1.Width, img2.Width);
            Assert.AreEqual(img1.Height, img2.Height);

            for (int y=0; y<img1.Height; y++)
            {
                for(int x=0; x<img1.Width; x++)
                {
                    Assert.AreEqual(img1.GetPixel(x, y), img2.GetPixel(x, y));
                }
            }
        }

        private void EqualImage(Bitmap beforeTrim,
                                Bitmap afterTrim,
                                Point trimLeftTop,
                                Point trimLeftBottom,
                                Point trimRightTop,
                                Point trimRightBottom,
                                double ratio,
                                int rotateDegree
                                )
        {
            // TODO: 画像の中身をチェックする
        }

        #endregion
    }
}
