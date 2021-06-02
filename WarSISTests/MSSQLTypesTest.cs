using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Drawing;

using WarSISDataBase.DataBase.Types;

namespace WarSISTests
{
    [TestClass]
    public class MSSQLTypesTest
    {
        [TestMethod]
        public void MSSQLTypesTestGetType()
        {
            Assert.AreEqual<String>(MSSQLTypes.GetType(typeof(Int32)).ToUpper(), "int".ToUpper());
            Assert.AreEqual<String>(MSSQLTypes.GetType(typeof(Single)).ToUpper(), "float".ToUpper());
            Assert.AreEqual<String>(MSSQLTypes.GetType(typeof(Double)).ToUpper(), "float".ToUpper());
            Assert.AreEqual<String>(MSSQLTypes.GetType(typeof(String)).ToUpper(), "varchar(50)".ToUpper());
            Assert.AreEqual<String>(MSSQLTypes.GetType(typeof(String), 255).ToUpper(), "varchar(255)".ToUpper());
            Assert.AreEqual<String>(MSSQLTypes.GetType(typeof(Image)).ToUpper(), "image".ToUpper());
            Assert.AreEqual<String>(MSSQLTypes.GetType(typeof(Object)).ToUpper(), "binary(50)".ToUpper());
            Assert.AreEqual<String>(MSSQLTypes.GetType(typeof(Object), 255).ToUpper(), "binary(255)".ToUpper());
        }
    }
}
