using System;
using System.Data;
using System.Collections.Generic;
using WarSISDataBase.Args;

namespace WarSISDataBase.DataBase
{
    /// <summary>
    /// Утверждает что у класса есть базовые методы
    /// для получения, изменения или добавления данных в БД
    /// (Select, Insert, Update, Delete)
    /// </summary>
    public interface IDataBaseEditor : IDataBaseConnector
    {
        DataTable Select(List<String> Fields, String Table, String Where = "", List<ISelectArgs> Args = null);
        Int32 Insert(Dictionary<String, Object> Data, String Table);
        Int32 Update(Dictionary<String, Object> Data, String Table, String Where = "");
        Int32 Delete(String Table, String Where = "");
    }

}
