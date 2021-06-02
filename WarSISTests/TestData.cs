using System;
using System.Collections.Generic;

using WarSISDataBase.DataBase;
using WarSISDataBase.DataBase.Types;

namespace WarSISTests
{
    public static class TestItem
    {
        public static IDataBaseConnector Connector = new MSSQLEngine();
        public static IDataBaseCreator Creator = new MSSQLEngine();
        public static IDataBaseEditor Editor = new MSSQLEngine();
    }
    public static class TestData
    {
        public static string AcceptLocalDB = @"Data Source=.\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";
        public static string ErrorLocalDB = @"Data Source=.\SQLEXPRESS;Initial Catalog=NoDB;Integrated Security=True";

        public static Dictionary<string, string> Fields = new Dictionary<string, string>
            {
                { "id is", MSSQLTypes.GetType(typeof(Int32)) }
            };

    }
    public static class TestMethods
    {
        public static void TryCreateTable(String TableName, Dictionary<String, String> Fields)
        {
            TestItem.Creator.ConnectionString = TestData.AcceptLocalDB;
            try
            {
                TestItem.Creator.CreateTable(TableName, Fields);
            }
            catch { }
        }
        public static void TryDeleteTable(String TableName)
        {
            TestItem.Creator.ConnectionString = TestData.AcceptLocalDB;
            try
            {
                TestItem.Creator.DeleteTable(TableName);
            }
            catch { }
        }
    }
}
