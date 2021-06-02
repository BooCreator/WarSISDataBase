using System;
using System.Collections.Generic;

namespace WarSISDataBase.DataBase
{
    /// <summary>
    /// Утверждает что класс имеет методы для
    /// создания, изменения или удаления таблиц в БД
    /// </summary>
    public interface IDataBaseCreator : IDataBaseConnector
    {
        Boolean CreateTable(String Name, Dictionary<String, String> Fields);
        Boolean EditTable(String Name, Dictionary<String, String> Fields, DataBaseEditorType Type);
        Boolean DeleteTable(String Name);
    }

    public enum DataBaseEditorType
    {
        AddFields,
        EditFields,
        RemoveFields,
        RenameFields,
        ReplaceAll
    }
}
