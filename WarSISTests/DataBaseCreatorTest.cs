using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using WarSISDataBase.DataBase;
using WarSISDataBase.DataBase.Types;

namespace WarSISTests
{
    [TestClass]
    public class DataBaseCreatorTest
    {
        [TestMethod]
        public void CreateTableTest()
        {
            TestItem.Creator.ConnectionString = TestData.AcceptLocalDB;
            TestMethods.TryDeleteTable("testTable");
            Assert.IsTrue(TestItem.Creator.CreateTable("TestTable", TestData.Fields));
            TestItem.Creator.DeleteTable("TestTable");
        }

        [TestMethod]
        public void AlterTableTest()
        {
            TestItem.Creator.ConnectionString = TestData.AcceptLocalDB;
            TestMethods.TryCreateTable("TestTable", TestData.Fields);

            Dictionary<string, string> Fields = new Dictionary<string, string>
            {
                { "aaa", MSSQLTypes.GetType(typeof(Int32)) },
                { "bbb", MSSQLTypes.GetType(typeof(String), 15) },
                { "ccc", MSSQLTypes.GetType(typeof(Int32)) }
            };
            Dictionary<string, string> Fields_2 = new Dictionary<string, string>
            {
                { "id", MSSQLTypes.GetType(typeof(Int32)) },
                { "name", MSSQLTypes.GetType(typeof(String), 50) },
                { "years to", MSSQLTypes.GetType(typeof(Int32)) }
            };
            Dictionary<string, string> FieldsNames = new Dictionary<string, string>
            {
                { "id", "aaa" },
                { "name", "bbb" },
                { "years to", "ccc" }
            };

            Assert.IsTrue(TestItem.Creator.EditTable("TestTable", Fields, DataBaseEditorType.AddFields));
            Assert.IsTrue(TestItem.Creator.EditTable("TestTable", Fields, DataBaseEditorType.RemoveFields));
            Assert.IsTrue(TestItem.Creator.EditTable("TestTable", Fields_2, DataBaseEditorType.ReplaceAll));
            Assert.IsTrue(TestItem.Creator.EditTable("TestTable", FieldsNames, DataBaseEditorType.RenameFields));
            Assert.IsTrue(TestItem.Creator.EditTable("TestTable", Fields, DataBaseEditorType.EditFields));
            TestItem.Creator.DeleteTable("TestTable");
        }

        [TestMethod]
        public void DropTableTest()
        {
            TestItem.Creator.ConnectionString = TestData.AcceptLocalDB;
            TestMethods.TryCreateTable("TestTable", TestData.Fields);
            Assert.IsTrue(TestItem.Creator.DeleteTable("TestTable"));
        }

    }
}
