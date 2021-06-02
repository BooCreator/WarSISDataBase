using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using WarSISDataBase.Args;
using WarSISDataBase.DataBase.Types;

namespace WarSISTests
{
    [TestClass]
    public class DataBaseEditorTest
    {
        Dictionary<String, Object> Data = new Dictionary<string, object> {
                { "id", 0 },
                { "name", "asd" },
                { "years to", 1995 }
            };

        public void CreateTable()
        {
            TestItem.Editor.ConnectionString = TestData.AcceptLocalDB;
            Dictionary<string, string> Fields = new Dictionary<string, string>
            {
                { "id", MSSQLTypes.GetType(typeof(Int32)) },
                { "name", MSSQLTypes.GetType(typeof(String), 50) },
                { "years to", MSSQLTypes.GetType(typeof(Int32)) }
            };
            TestMethods.TryCreateTable("TestTable", Fields);
        }

        [TestMethod]
        public void InsertTest()
        {
            CreateTable();

            Assert.AreEqual(TestItem.Editor.Insert(Data, "TestTable"), 1);

            TestItem.Creator.DeleteTable("TestTable");
        }
        [TestMethod]
        public void SelectTest()
        {
            CreateTable();
            Dictionary<String, Object> Data1 = new Dictionary<string, object> {
                { "id", 1 },
                { "name", "dsa" },
                { "years to", 1995 }
            };
            Dictionary<String, Object> Data2 = new Dictionary<string, object> {
                { "id", 2 },
                { "name", "asd" },
                { "years to", 1995 }
            };
            TestItem.Editor.Insert(this.Data, "TestTable");
            TestItem.Editor.Insert(Data1, "TestTable");
            TestItem.Editor.Insert(Data2, "TestTable");
            Assert.AreEqual(TestItem.Editor.Select(new List<String> { "*" }, "TestTable", "id = 0").Rows.Count, 1);
            Assert.AreEqual(TestItem.Editor.Select(new List<String> { "*" }, "TestTable", Args: new List<ISelectArgs>() { new TOP(2) }).Rows.Count, 2);
            Assert.AreEqual(TestItem.Editor.Select(new List<String> { "id" }, "TestTable", Args: new List<ISelectArgs>() { new COUNT("id") }).Rows[0].ItemArray[0], 3);
            Assert.AreEqual(TestItem.Editor.Select(new List<String> { "id" }, "TestTable", Args: new List<ISelectArgs>() { new MAX("id") }).Rows[0].ItemArray[0], 2);
            Assert.AreEqual(TestItem.Editor.Select(new List<String> { "id" }, "TestTable", Args: new List<ISelectArgs>() { new MIN("id") }).Rows[0].ItemArray[0], 0);
            Assert.AreEqual(TestItem.Editor.Select(new List<String> { "id" }, "TestTable", Args: new List<ISelectArgs>() { new SUM("id") }).Rows[0].ItemArray[0], 3);
            Assert.AreEqual(TestItem.Editor.Select(new List<String> { "id", "name" }, "TestTable", Args: new List<ISelectArgs>() { new COUNT("id"), new GROUP_BY("name") }).Rows[0].ItemArray[0], 2);
            TestItem.Creator.DeleteTable("TestTable");
        }
        [TestMethod]
        public void UpdateTest()
        {
            CreateTable();
            TestItem.Editor.Insert(this.Data, "TestTable");
            Dictionary<String, Object> Data = new Dictionary<string, object> {
                { "name", "dsa" },
                { "years to", 2000 }
            };
            Assert.IsTrue(TestItem.Editor.Update(Data, "TestTable", "id = 0") > -1);
            TestItem.Creator.DeleteTable("TestTable");
        }

        [TestMethod]
        public void DeleteTest()
        {
            CreateTable();
            TestItem.Editor.Insert(Data, "TestTable");

            Assert.IsTrue(TestItem.Editor.Delete("TestTable", "id = 0") == 1);

            TestItem.Creator.DeleteTable("TestTable");
        }
    }
}
