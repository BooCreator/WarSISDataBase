using System;

namespace WarSISDataBase.Args
{
    public interface ISelectArgs
    {
        String Data { get; set; }
        String GetValue();
    }
}
