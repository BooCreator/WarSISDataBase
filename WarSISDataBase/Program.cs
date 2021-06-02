using System;
using System.Collections.Generic;

using WarSISDataBase.DataBase.Types;
using WarSISDataBase.Args;

namespace WarSISDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            string con = @"Data Source=.\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";

            var DB = new DataBase.MSSQLEngine(con);

            Console.ReadLine();
        }

        static void TestAlterTable(DataBase.MSSQLEngine DB)
        {
            Dictionary<string, string> Fields = new Dictionary<string, string>
            {
                { "aaa", MSSQLTypes.GetType(typeof(Int32)) },
                { "bbb", MSSQLTypes.GetType(typeof(String), 15) },
                { "ccc", MSSQLTypes.GetType(typeof(Int32)) }
            };
            Dictionary<string, string> Fields1 = new Dictionary<string, string>
            {
                { "id", MSSQLTypes.GetType(typeof(Int32)) },
                { "Название", MSSQLTypes.GetType(typeof(String), 50) },
                { "Телефон", MSSQLTypes.GetType(typeof(Int32)) }
            };
            Dictionary<string, string> FieldsNames = new Dictionary<string, string>
            {
                { "id", "aaa" },
                { "Название", "bbb" },
                { "Телефон", "ccc" }
            };

            DB.EditTable("Table_1", Fields, DataBase.DataBaseEditorType.AddFields);
            DB.EditTable("Table_1", Fields, DataBase.DataBaseEditorType.RemoveFields);
            DB.EditTable("Table_1", FieldsNames, DataBase.DataBaseEditorType.RenameFields);
            DB.EditTable("Table_1", Fields, DataBase.DataBaseEditorType.EditFields);
            DB.EditTable("Table_1", Fields1);
        }

        static void TestEditor(DataBase.MSSQLEngine DB)
        {
            List<Dictionary<string, object>> Fields = new List<Dictionary<string, object>>() {
                new Dictionary<string, object>
                {
                    { "id", 0 },
                    { "Название", "aaa" },
                    { "Телефон", 12345678 },
                },
                new Dictionary<string, object>
                {
                    { "id", 1 },
                    { "Название", "bbb" },
                    { "Телефон", 87654321 },
                },
                new Dictionary<string, object>
                {
                    { "id", 2 },
                    { "Название", "bbb" },
                    { "Телефон", 1234321 },
                },
            };

            DB.Insert(Fields[0], "Table_1");
            DB.Insert(Fields[1], "Table_1");
            DB.Insert(Fields[2], "Table_1");

            Show(DB.Select(new List<String> { "id", "Название", "Телефон" }, "Table_1"));
            Console.WriteLine("TOP ");
            Show(DB.Select(new List<String> { "id", "Название", "Телефон" }, "Table_1", Args: new List<ISelectArgs> { new TOP(2) }));
            Console.WriteLine("ORDER BY: ");
            Show(DB.Select(new List<String> { "id", "Название", "Телефон" }, "Table_1", Args: new List<ISelectArgs> { new ORDER_BY("id", "DESC"), new TOP(2) }));
            Console.WriteLine("GROUP BY: ");
            Show(DB.Select(new List<String> { "Название", "Телефон" }, "Table_1", Args: new List<ISelectArgs> { new GROUP_BY("Название", "Телефон") }));
            Console.WriteLine("COUNT: ");
            Show(DB.Select(new List<String> { "id" }, "Table_1", Args: new List<ISelectArgs> { new COUNT("id") }));
            Show(DB.Select(new List<String> { "id", "Название" }, "Table_1", Args: new List<ISelectArgs> { new COUNT("id"), new GROUP_BY("Название") }));
            Console.WriteLine("SUM: ");
            Show(DB.Select(new List<String> { "id" }, "Table_1", Args: new List<ISelectArgs> { new SUM("id") }));
            Console.WriteLine("MIN: ");
            Show(DB.Select(new List<String> { "id" }, "Table_1", Args: new List<ISelectArgs> { new MIN("id") }));
            Console.WriteLine("MAX: ");
            Show(DB.Select(new List<String> { "id" }, "Table_1", Args: new List<ISelectArgs> { new MAX("id") }));
            Console.WriteLine("AVG: ");
            Show(DB.Select(new List<String> { "id" }, "Table_1", Args: new List<ISelectArgs> { new AVG("id") }));

            DB.Delete("Table_1");
        }

        static void Show(System.Data.DataTable Table)
        {
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                var Item = Table.Rows[i].ItemArray;
                for (int j = 0; j < Item.Length; j++)
                {
                    Console.Write($"{Item[j]} ");
                }
                Console.WriteLine();
            }
        }
    }
}
