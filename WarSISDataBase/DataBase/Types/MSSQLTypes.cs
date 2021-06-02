using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSISDataBase.DataBase.Types
{
    public static class MSSQLTypes
    {
        public static string GetType(Object Value)
        {
            if(Value is Int32)
                return "int";
            if(Value is Single || Value is Double)
                return "float";
            if (Value is DateTime)
                return "datetime";
            if(Value is String)
            {
                int len = (Value as String).Length;
                return $"varchar({len})";
            }
            if (Value is Image)
                return "image";

            return "binary(MAX)";
        }
        public static string GetType(Type Value, Int32 Len = 50)
        {
            switch(Value.Name)
            {
                case "Int32":
                    return "int";
                case "Single": case "Double":
                    return "float";
                case "DateTime":
                    return "datetime";
                case "Image":
                    return "image";
                case "String":
                    return $"varchar({Len})";
                default:
                    return $"binary({Len})";
            }
        }
    }
}
