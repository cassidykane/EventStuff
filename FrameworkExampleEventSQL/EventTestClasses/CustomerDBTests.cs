using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EventPropsClasses;
using EventDBClasses;
using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class CustomerDBTests
    {
        CustomerProps db1Props;
        List<CustomerProps> propsList;
        CustomerSQLDB db;
        private string dataSource = "Data Source=LAPTOP-Q0MU1P5L\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";


        [SetUp]
        public void SetUp()
        {
            db = new CustomerSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;

            db.RunNonQueryProcedure(command);
            db1Props = (CustomerProps)db.Retrieve(1);
            propsList = (List<CustomerProps>)db.RetrieveAll(db.GetType());
        }

        [Test]
        public void TestRetrieve()
        {
            Assert.AreEqual("Molunguri, A", db1Props.name);
            Assert.AreEqual("1108 Johanna Bay Drive", db1Props.address);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> propsList = (List<CustomerProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(696, propsList.Count);
        }

        [Test]
        public void TestCreate()
        {
            CustomerProps c2 = (CustomerProps)db.Create(db1Props);
            Assert.AreEqual(700, c2.ID);
            Assert.AreEqual(1, c2.ConcurrencyID);

            db1Props = (CustomerProps)db.Retrieve(700);
            Assert.AreEqual("Molunguri, A", db1Props.name);
            Assert.AreEqual("1108 Johanna Bay Drive", db1Props.address);
        }

        [Test]
        public void TestDelete()
        {
            db.Delete(db1Props);
            propsList = (List<CustomerProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(695, propsList.Count);
            Assert.Throws<Exception>(() => db1Props = (CustomerProps)db.Retrieve(1));
        }

        [Test]
        public void TestUpdate()
        {
            db1Props.name = "new name";
            db1Props.address = "new address";
            db1Props.city = "new city";

            db.Update(db1Props);
            db1Props = (CustomerProps)db.Retrieve(1);

            Assert.AreEqual(db1Props.name, "new name");
            Assert.AreEqual(db1Props.address, "new address");
            Assert.AreEqual(db1Props.city, "new city");
            Assert.AreEqual(db1Props.ConcurrencyID, 2);
        }
    }
}
