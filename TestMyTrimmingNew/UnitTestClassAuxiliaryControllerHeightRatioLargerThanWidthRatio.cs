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
            AuxiliaryController.RatioType ratioType = AuxiliaryController.RatioType.W9H16;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage001Path, ratioType);

            double ratio = (double)ac.WidthRatio(ratioType) / (double)ac.HeightRatio(ratioType);

            int fittedHeight = ac.DisplayImageHeight;
            int fittedWidth = (int)((double)fittedHeight * ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryHeight);
        }

        [TestMethod]
        [DeploymentItem(@".\Resource\test002.jpg")]
        public void TestCorrectWidthAndHeightOfInitAuxiliaryLineIfImageHeightLongerThanImageWidth()
        {
            AuxiliaryController.RatioType ratioType = AuxiliaryController.RatioType.W9H16;
            AuxiliaryController ac = Common.GetAuxiliaryController(Common.TestResourceImage002Path, ratioType);

            double ratio = (double)ac.WidthRatio(ratioType) / (double)ac.HeightRatio(ratioType);

            int fittedHeight = ac.DisplayImageHeight;
            int fittedWidth = (int)((double)fittedHeight * ratio);
            Assert.AreEqual(fittedWidth, ac.AuxiliaryWidth);
            Assert.AreEqual(fittedHeight, ac.AuxiliaryHeight);
        }
    }
}
