using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTrimmingNew;

namespace TestMyTrimmingNew
{
    [TestClass]
    public class UnitTestFunctionUndoRedoAuxiliaryOperation
    {
        [TestMethod]
        [DeploymentItem(@".\Resource\test001.jpg")]
        public void TestUndoInputCursorKeyDown()
        {
            AuxiliaryController ac = Common.GetAuxiliaryControllerImage001RatioTypeW16H9();

            // selfメッセージ: 今Undo/Redo実装中！
            ac.SetEvent();
            ac.PublishEvent(Keys.EnableKeys.Down);
            Assert.AreEqual(1, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);

            //ac.CancelEvent();
            Assert.AreEqual(0, ac.AuxiliaryTopRelativeImage);
            Assert.AreEqual(0, ac.AuxiliaryLeftRelativeImage);
        }
    }
}
