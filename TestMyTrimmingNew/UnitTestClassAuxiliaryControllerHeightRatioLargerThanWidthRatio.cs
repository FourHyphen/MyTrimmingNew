using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTestClassAuxiliaryControllerHeightRatioLargerThanWidthRatio
    {
        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageWidthLongerThanImageHeight()
        {
            int widthRatio = 9;
            int heightRatio = 16;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path,
                                                                   widthRatio,
                                                                   heightRatio);

            double ratio = (double)widthRatio / (double)heightRatio;
            int fittedHeight = ac.DisplayImageHeight;
            int fittedWidth = (int)((double)fittedHeight * ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test002.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageHeightLongerThanImageWidth()
        {
            int widthRatio = 9;
            int heightRatio = 16;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage002Path,
                                                                   widthRatio,
                                                                   heightRatio);

            double ratio = (double)widthRatio / (double)heightRatio;
            int fittedHeight = ac.DisplayImageHeight;
            int fittedWidth = (int)((double)fittedHeight * ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryHeight);
        }
    }
}
