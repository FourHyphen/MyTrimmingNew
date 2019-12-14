using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DeploymentItem(@".\Resources\test001.jpg")]
        public void TestSuccessOfCreateBitmapAfterNewImage()
        {
            MyTrimmingNew.Image img = new MyTrimmingNew.Image(@".\Resources\test001.jpg");
            Assert.IsNotNull(img.BitmapImage);
        }

        [TestMethod]
        [DeploymentItem(@".\Resources\test001.jpg")]
        public void TestNotEqualOfImageNameAndSaveNameExample()
        {
            MyTrimmingNew.Image img = new MyTrimmingNew.Image(@".\Resources\test001.jpg");
            Assert.AreNotEqual(img.SaveNameExample, "test001.jpg");
        }
    }
}
