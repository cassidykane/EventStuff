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
    public class CustomerTests
    {
        private string dataSource = "Data Source=LAPTOP-Q0MU1P5L\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void SetUp()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieveExistingCustomer()
        {
            Customer p = new Customer(1, dataSource);
            Assert.AreEqual(p.ID, 1);
            Assert.AreEqual(p.Name, "Molunguri, A");
            Assert.AreEqual(p.Address, "1108 Johanna Bay Drive");
            Assert.AreEqual(p.City, "Birmingham");
            Assert.AreEqual(p.State, "AL");
            Assert.AreEqual(p.ZipCode, "35216-6909     ");
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void TestCreateNewCustomer()
        {
            Customer c = new Customer(dataSource);

            c.Name = "Test Customer";
            c.Address= "123 Abc Ave";
            c.City = "Realtown";
            c.State = "OK";
            c.ZipCode = "12345";

            c.Save();
            Customer c2 = new Customer(c.ID, dataSource);

            Assert.AreEqual(c.ID, c2.ID);
            Assert.AreEqual(c.Name, c2.Name);
            Assert.AreEqual(c.Address, c2.Address);
            Assert.AreEqual(c.City, c2.City);
            Assert.AreEqual(c.State, c2.State);
            Assert.AreEqual(c.ZipCode, c2.ZipCode);
            Console.WriteLine(c2.ToString());
        }

        [Test]
        public void TestUpdateCustomer()
        {
            Customer c = new Customer(1, dataSource);
            c.Name = "First Customer";
            c.Address = "456 D St.";
            c.City = "Cityville";
            c.State = "NJ";
            c.ZipCode = "11111";

            c.Save();

            c = new Customer(1, dataSource);
            Assert.AreEqual(c.ID, 1);
            Assert.AreEqual(c.Name, "First Customer");
            Assert.AreEqual(c.Address, "456 D St.");
            Assert.AreEqual(c.City, "Cityville");
            Assert.AreEqual(c.State, "NJ");
            Assert.AreEqual(c.ZipCode, "11111");
        }

        [Test]
        public void TestDeleteCustomer()
        {
            Customer c = new Customer(2, dataSource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(2, dataSource));
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer p = new Customer(dataSource);
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "Nueman Newname";
            Assert.Throws<Exception>(() => c.Save());
            c.Address = "this is a test";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer(1, dataSource);
            Customer c2 = new Customer(1, dataSource);

            c1.Name = "Updated this first";
            c1.Save();

            c2.Name = "Updated this second";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}
