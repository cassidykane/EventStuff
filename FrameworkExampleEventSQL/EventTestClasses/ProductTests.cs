using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using EventClasses;
using EventPropsClasses;
using EventDBClasses;
using ToolsCSharp;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class ProductTests
    {
        private string dataSource = "Data Source=LAPTOP-Q0MU1P5L\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void SetUp()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieveExistingProduct()
        {
            Product p = new Product(1, dataSource);
            Assert.AreEqual(p.ID, 1);
            Assert.AreEqual(p.Code, "A4CS");
            Assert.AreEqual(p.Description, "Murach's ASP.NET 4 Web Programming with C# 2010");
            Assert.AreEqual(p.Price, 56.5m);
            Assert.AreEqual(p.Quantity, 4637);
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void TestCreateNewProduct()
        {
            Product p = new Product(dataSource);

            p.Code = "YYYY";
            p.Description = "This is a test Product";
            p.Quantity = 10;
            p.Price = 10.99m;

            p.Save();
            Product p2 = new Product(p.ID, dataSource);

            Assert.AreEqual(p.ID, p2.ID);
            Assert.AreEqual(p.Code, p2.Code);
            Assert.AreEqual(p.Description, p2.Description);
            Assert.AreEqual(p.Price, p2.Price);
            Assert.AreEqual(p.Quantity, p.Quantity);
            Console.WriteLine(p2.ToString());
        }

        [Test]
        public void TestUpdateProduct()
        {
            Product p = new Product(1, dataSource);
            p.Code = "ZZZZ";
            p.Description = "This is an updated Product";
            p.Price = 5.55m;

            p.Save();

            p = new Product(1, dataSource);
            Assert.AreEqual(p.ID, 1);
            Assert.AreEqual(p.Code, "ZZZZ");
            Assert.AreEqual(p.Description, "This is an updated Product");
            Assert.AreEqual(p.Price, 5.55m);
        }

        [Test]
        public void TestDeleteProduct()
        {
            Product p = new Product(2, dataSource);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(2, dataSource));
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
            p.Price = 10.0m;
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "this is a test";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Product p1 = new Product(1, dataSource);
            Product p2 = new Product(1, dataSource);

            p1.Description = "Updated this first";
            p1.Save();

            p2.Description = "Updated this second";
            Assert.Throws<Exception>(() => p2.Save());
        }
    }
}
