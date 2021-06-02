using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using WarSISDataBase.Args;

namespace WarSISDataBase.DataBase
{
    public class MSSQLEngine : IDataBaseEditor, IDataBaseConnector, IDataBaseCreator
    {
        // Регулярное выражения для символов
        // запрещённых при названии параметра SQL (@na.me => @name)
        Regex RegEx = new Regex(@"[\., /\\]");
        public string ConnectionString { get; set; }

        public MSSQLEngine() : this("") { }
        public MSSQLEngine(String ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public DataTable Select(String Query)
        {
            using (var DB = new SqlConnection(this.ConnectionString))
            {
                DB.Open();
                DbDataAdapter adapter = new SqlDataAdapter(new SqlCommand(Query, DB));
                var res = new DataTable();
                adapter.Fill(res);
                return res;
            }
        }
        public Int32 Insert(String Query)
        {
            using (var DB = new SqlConnection(this.ConnectionString))
            {
                DB.Open();
                return new SqlCommand(Query, DB).ExecuteNonQuery();
            }
        }
        public Int32 Update(String Query)
        {
            using (var DB = new SqlConnection(this.ConnectionString))
            {
                DB.Open();
                return new SqlCommand(Query, DB).ExecuteNonQuery();
            }
        }
        public Int32 Delete(String Query)
        {
            using (var DB = new SqlConnection(this.ConnectionString))
            {
                DB.Open();
                return new SqlCommand(Query, DB).ExecuteNonQuery();
            }
        }

        private void Check(Action<String> Callback, ref List<ISelectArgs> Args, params String[] Items)
        {
            if (Args != null)
            {
                foreach (var Item in Items)
                {
                    var item = Args.Find(x => x.GetValue().ToUpper().IndexOf(Item.ToUpper()) != -1);
                    if (item != null)
                    {
                        Callback(item.GetValue());
                        Args.Remove(item);
                        break;
                    }
                }
            }
        }
        private void Check(Action<String, String> Callback, ref List<ISelectArgs> Args, params String[] Items)
        {
            if (Args != null)
            {
                foreach (var Item in Items)
                {
                    var item = Args.Find(x => x.GetValue().ToUpper().IndexOf(Item.ToUpper()) != -1);
                    if (item != null)
                    {
                        Args.Remove(item);
                        Callback(item.GetValue(), item.Data);
                        break;
                    }
                }
            }
        }

        // Args = NO_QUOTES => [table] => table
        public DataTable Select(List<String> Fields, String Table, String Where = "", List<ISelectArgs> Args = null)
        {
            StringBuilder SQL = new StringBuilder("select ");
            var quotes = Args?.Find(x => x.GetType() == new NO_QUOTES().GetType());
            if (quotes == null)
                Table = $"[{Table}]";
            else
                Args.Remove(quotes);
            for (int i = 0; i < Fields?.Count; i++)
            {
                if (Fields[i].IndexOf("*") == -1)
                    Fields[i] = $"[{Fields[i]}]";
            }

            if (Args != null && Args.Count > 0)
                this.Check((String x) => { SQL.Append($"{x} "); }, ref Args, TOP.Name);
            if (Args != null && Args.Count > 0)
                this.Check((String Value, String Field) => {
                    SQL.Append($"{Value} ");
                    var field = Fields.Find(x => x.CompareTo(Field) == 0);
                    Fields.Remove(field);
                    if (Args.Count > 0 || Fields.Count > 0)
                        SQL.Append(", ");
                }, ref Args, COUNT.Name, MAX.Name, MIN.Name, AVG.Name, SUM.Name);
            if (Fields?.Count > 0)
                SQL.Append($"{String.Join(", ", Fields)} ");
            SQL.Append($"from {Table}" + ((Where.Length > 0) ? $" where {Where}" : ""));

            if (Args != null && Args.Count > 0)
                this.Check((String x) => { SQL.Append(x + " "); }, ref Args, GROUP_BY.Name, ORDER_BY.Name);

            return this.Select(SQL.ToString());
        }

        public Int32 Update(Dictionary<String, Object> Data, String Table, String Where = "")
        {
            if (Data.Count > 0)
            {
                using (var DB = new SqlConnection(this.ConnectionString))
                {
                    DB.Open();
                    DbCommand command = new SqlCommand
                    {
                        Connection = DB
                    };
                    StringBuilder Query = new StringBuilder($"update [{Table}] set ");
                    foreach (var Item in Data)
                    {
                        if(Item.Value != null)
                            command.Parameters.Add(new SqlParameter(RegEx.Replace(Item.Key, ""), Item.Value));
                        else
                            command.Parameters.Add(new SqlParameter(RegEx.Replace(Item.Key, ""), DBNull.Value));
                        Query.Append($"[{Item.Key}] = @{RegEx.Replace(Item.Key, "")},");
                    }
                    Query.Remove(Query.Length - 1, 1);
                    if (Where.Length > 0)
                        Query.Append($" where {Where}");
                    command.CommandText = Query.ToString();
                    return command.ExecuteNonQuery();
                }
            }
            return -1;
        }
        public Int32 Insert(Dictionary<String, Object> Data, String Table)
        {
            if (Data.Count > 0)
            {
                using (var DB = new SqlConnection(this.ConnectionString))
                {
                    DB.Open();
                    DbCommand command = new SqlCommand
                    {
                        Connection = DB
                    };
                    StringBuilder Query = new StringBuilder($"insert into [{Table}](");
                    StringBuilder Values = new StringBuilder($"values (");
                    foreach (var Item in Data)
                    {
                        if(Item.Value != null)
                            command.Parameters.Add(new SqlParameter(RegEx.Replace(Item.Key, ""), Item.Value));
                        else
                            command.Parameters.Add(new SqlParameter(RegEx.Replace(Item.Key, ""), DBNull.Value));
                        Query.Append($"[{Item.Key}],");
                        Values.Append($"@{RegEx.Replace(Item.Key, "")},");
                    }
                    Query.Remove(Query.Length - 1, 1);
                    Values.Remove(Values.Length - 1, 1);
                    Query.Append($") {Values})");
                    command.CommandText = Query.ToString();
                    return command.ExecuteNonQuery();
                }
            }
            return -1;
        }
        public Int32 Delete(String Table, String Where = "")
            => this.Delete($"delete from [{Table}]" + ((Where.Length > 0) ? $" where {Where}" : ""));

        public Boolean Test(out String Message)
        {
            Message = "";
            try
            {
                var DB = new SqlConnection(this.ConnectionString);
                DB.Open();
                DB.Close();
            }
            catch (Exception Error)
            {
                Message = Error.Message;
                return false;
            }
            return true;
        }

        public bool CreateTable(string Name, Dictionary<string, string> Fields)
        {
            using (var DB = new SqlConnection(this.ConnectionString))
            {
                DB.Open();
                StringBuilder Query = new StringBuilder($"create table {Name} (");
                foreach (var Item in Fields)
                {
                    Query.Append($"[{Item.Key}] {Item.Value},");
                }
                Query.Remove(Query.Length - 1, 1);
                Query.Append(")");
                return new SqlCommand(Query.ToString(), DB).ExecuteNonQuery() != 0;
            }
        }

        public bool EditTable(string Name, Dictionary<string, string> Fields, DataBaseEditorType Type = DataBaseEditorType.ReplaceAll)
        {
            using (var DB = new SqlConnection(this.ConnectionString))
            {
                DB.Open();

                Dictionary<string, string> ExistFields = new Dictionary<string, string>();
                if (Type == DataBaseEditorType.ReplaceAll)
                {
                    var db = this.Select(new List<String> { "COLUMN_NAME", "DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH" }, "INFORMATION_SCHEMA.COLUMNS", $"TABLE_NAME = '{Name}'", Args: new List<ISelectArgs>() { new NO_QUOTES() });
                    for (int i = 0; i < db.Rows.Count; i++)
                    {
                        ExistFields.Add(
                            db.Rows[i].ItemArray[0].ToString(),
                            db.Rows[i].ItemArray[1].ToString() +
                            ((db.Rows[i].ItemArray[2].ToString().Length > 0) ? $"({db.Rows[i].ItemArray[2]})" : ""));
                    }
                }

                foreach (var Item in Fields)
                {
                    switch (Type)
                    {
                        case DataBaseEditorType.AddFields:
                            new SqlCommand($"alter table [{Name}] add [{Item.Key}] {Item.Value}", DB).ExecuteNonQuery();
                            break;
                        case DataBaseEditorType.EditFields:
                            new SqlCommand($"alter table [{Name}] alter column [{Item.Key}] {Item.Value}", DB).ExecuteNonQuery();
                            break;
                        case DataBaseEditorType.RemoveFields:
                            new SqlCommand($"alter table [{Name}] drop column [{Item.Key}]", DB).ExecuteNonQuery();
                            break;
                        case DataBaseEditorType.RenameFields:
                            new SqlCommand($"EXEC sp_RENAME '{Name}.{Item.Key}', '{Item.Value}', 'COLUMN'", DB).ExecuteNonQuery();
                            break;
                        default:
                            if (ExistFields.TryGetValue(Item.Key, out string Field))
                            {
                                if (Item.Value.CompareTo(Field) != 0)
                                {
                                    new SqlCommand($"alter table [{Name}] alter column [{Item.Key}] {Item.Value}", DB).ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                new SqlCommand($"alter table [{Name}] add [{Item.Key}] {Item.Value}", DB).ExecuteNonQuery();
                            }
                            ExistFields.Remove(Item.Key);
                            break;
                    }
                }
                if (Type == DataBaseEditorType.ReplaceAll)
                {
                    foreach (var Item in ExistFields)
                    {
                        new SqlCommand($"alter table [{Name}] drop column [{Item.Key}]", DB).ExecuteNonQuery();
                    }
                }
                return true;
            }
        }

        public bool DeleteTable(string Name)
        {
            using (var DB = new SqlConnection(this.ConnectionString))
            {
                DB.Open();
                return new SqlCommand($"drop table [{Name}]", DB).ExecuteNonQuery() != 0;
            }
        }

    }
}
