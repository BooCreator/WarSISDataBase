using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarSISTests
{
    [TestClass]
    public class DataBaseConnectorTest
    {
        [TestMethod]
        public void TestConnectionTestOnErrorDB()
        {
            TestItem.Connector.ConnectionString = TestData.ErrorLocalDB;
            Assert.IsFalse(TestItem.Connector.Test(out string _));
        }
        [TestMethod]
        public void TestConnectionTestAcceptDB()
        {
            TestItem.Connector.ConnectionString = TestData.AcceptLocalDB;
            Assert.IsTrue(TestItem.Connector.Test(out string _));
        }
        [TestMethod]
        public void TestConnectionTestOnNoDB()
        {
            TestItem.Connector.ConnectionString = "";
            Assert.IsFalse(TestItem.Connector.Test(out string _));
        }
        [TestMethod]
        public void TestConnectionTestOnRandomText()
        {
            TestItem.Connector.ConnectionString = "asdasdasd";
            Assert.IsFalse(TestItem.Connector.Test(out string _));
        }
    }
}
