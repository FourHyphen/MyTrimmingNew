using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTest1
    {
        //private const string test001JpgPath = "../../../TestMyTrimmingNew/Resources/test001.jpg";

        [TestMethod]
        [DeploymentItem(@".\Resources\test001.jpg")]
        public void TestSuccessOfCreateBitmapAfterNewImage()
        {
            MyTrimmingNew.Image img = new MyTrimmingNew.Image(@".\Resources\test001.jpg");
            Assert.IsNotNull(img.BitmapImage);
        }
    }
}
