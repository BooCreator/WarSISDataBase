using System;
using System.Text;

namespace WarSISDataBase.Args
{
    public class SUM : ISelectArgs
    {
        public SUM(String Field, Boolean QUOTES = true)
            => this.Data = (QUOTES) ? $"[{Field}]" : Field;
        public static string Name => "SUM";
        public string Data { get; set; }
        public string GetValue() => $"{Name}({Data})";
    }
    public class MIN : ISelectArgs
    {
        public MIN(String Field, Boolean QUOTES = true)
            => this.Data = (QUOTES) ? $"[{Field}]" : Field;
        public static string Name => "MIN";
        public string Data { get; set; }
        public string GetValue() => $"{Name}({Data})";
    }
    public class MAX : ISelectArgs
    {
        public MAX(String Field, Boolean QUOTES = true)
            => this.Data = (QUOTES) ? $"[{Field}]" : Field;
        public static string Name => "MAX";
        public string Data { get; set; }
        public string GetValue() => $"{Name}({Data})";
    }
    public class AVG : ISelectArgs
    {
        public AVG(String Field, Boolean QUOTES = true)
            => this.Data = (QUOTES) ? $"[{Field}]" : Field;
        public static string Name => "AVG";
        public string Data { get; set; }
        public string GetValue() => $"{Name}({Data})";
    }
    public class COUNT : ISelectArgs
    {
        public COUNT(String Field, Boolean QUOTES = true)
            => this.Data = (QUOTES) ? $"[{Field}]" : Field;
        public static string Name => "COUNT";
        public string Data { get; set; }
        public string GetValue() => $"{Name}({Data})";
    }
    public class TOP : ISelectArgs
    {
        public TOP(Int32 Value)
            => this.Data = Value.ToString();
        public static string Name => "TOP";
        public string Data { get; set; }
        public string GetValue() => $"{Name} {Data}";
    }
    public class GROUP_BY : ISelectArgs
    {
        public GROUP_BY(params String[] Fields)
        {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < Fields.Length; i++)
            {
                res.Append($"[{Fields[i]}]");
                if (i < Fields.Length - 1)
                    res.Append(", ");
            }
            this.Data = res.ToString();
        }
        public static string Name => "GROUP BY";
        public string Data { get; set; } = "";
        public string GetValue() => $"{Name} {Data}";
    }

    public class ORDER_BY : ISelectArgs
    {
        public ORDER_BY(String Field, String Desc = "", Boolean QUOTES = true)
        {
            this.Data = (QUOTES) ? $"[{Field}]" : Field;
            this.Desc = Desc;
        }
        public static string Name => "ORDER BY";
        public string Data { get; set; }
        public String Desc { get; set; }
        public string GetValue() => $"{Name} {Data} {Desc}";
    }

    public class NO_QUOTES : ISelectArgs
    {
        public static string Name => "NO QUOTES";
        public string Data { get; set; }

        public string GetValue() => $"{Name}";
    }
}
