using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTestClassCommonImage
    {
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

        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectImageSizeAfterSaveTrimImage()
        {
            // 数字の根拠無し
            // 1/1スケールだと(x=400, y=600)からwidth=800, height=400の範囲を切り取る設定
            double ratio = 0.5;
            int seemingTrimLeft = 200;
            int seemingTrimTop = 300;
            int seemingTrimWidth = 400;
            int seemingTrimHeight = 200;

            System.IO.File.Delete(Common.TestToSaveTrimImagePath);

            Bitmap img = new Bitmap(Common.TestResourceImage001Path);
            MyTrimmingNew.common.Image.SaveTrimImage(img,
                                                     Common.TestToSaveTrimImagePath,
                                                     seemingTrimLeft,
                                                     seemingTrimTop,
                                                     seemingTrimWidth,
                                                     seemingTrimHeight,
                                                     ratio);

            Bitmap trimmedImg = new Bitmap(Common.TestToSaveTrimImagePath);

            int ansLeft = (int)((double)seemingTrimLeft / ratio);
            int ansTop = (int)((double)seemingTrimTop / ratio);
            int ansTrimWidth = (int)((double)seemingTrimWidth / ratio);
            int ansTrimHeight = (int)((double)seemingTrimHeight / ratio);
            Rectangle rect = new Rectangle(ansLeft, ansTop, ansTrimWidth, ansTrimHeight);
            Bitmap ansImg = img.Clone(rect, img.PixelFormat);

            EqualImage(trimmedImg, ansImg);
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
    }
}
