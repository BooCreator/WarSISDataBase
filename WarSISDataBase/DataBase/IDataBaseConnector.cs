using System;

namespace WarSISDataBase.DataBase
{
    /// <summary>
    /// Утверждает что у класса есть строка подключения к БД
    /// и метод тестирования подключения
    /// </summary>
    public interface IDataBaseConnector
    {
        string ConnectionString { get; set; }

        Boolean Test(out String Message);
    }
}
