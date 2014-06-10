using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandaloneNode;

namespace DBConnectionUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("The UnitTest of Database Connection ! \n");


            // ==DB Connection==

            // Arrange
            bool value = true;
            List<string> dbTestInfo = new List<string>();
            dbTestInfo.Clear();
            dbTestInfo.Add("localhost");
            dbTestInfo.Add("enricolu");
            dbTestInfo.Add("111platform!");
            dbTestInfo.Add("test");


            // Act
            DBConnection db = new DBConnection(dbTestInfo);
            value = db.checkDBExist();


            // Assert
            Assert.AreEqual(value, true);


        }
    }
}
