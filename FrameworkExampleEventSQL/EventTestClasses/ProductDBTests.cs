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
    public class ProductDBTests
    {
        ProductProps db1Props;
        List<ProductProps> propsList;
        ProductSQLDB db;
        private string dataSource = "Data Source=LAPTOP-Q0MU1P5L\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        

        [SetUp]
        public void SetUp()
        {
            db = new ProductSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;

            db.RunNonQueryProcedure(command);
            db1Props = (ProductProps)db.Retrieve(1);
            propsList = (List<ProductProps>)db.RetrieveAll(db.GetType());
        }

        [Test]
        public void TestRetrieve()
        {
            //props = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("A4CS", db1Props.code);
            Assert.AreEqual(56.5m, db1Props.price);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> propsList = (List<ProductProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(16, propsList.Count);
        }

        [Test]
        public void TestCreate()
        {
            //props = (ProductProps)db.Retrieve(1);
            ProductProps p2 = (ProductProps)db.Create(db1Props);
            Assert.AreEqual(17, p2.ID);
            Assert.AreEqual(1, p2.ConcurrencyID);

            db1Props = (ProductProps)db.Retrieve(17);
            Assert.AreEqual("A4CS", db1Props.code);
            Assert.AreEqual(56.5m, db1Props.price);
        }

        [Test]
        public void TestDelete()
        {
            db.Delete(db1Props);
            propsList = (List<ProductProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(15, propsList.Count);
            Assert.Throws<Exception>(() => db1Props = (ProductProps)db.Retrieve(1));
        }

        [Test]
        public void TestUpdate()
        {
            db1Props.code = "XXXX";
            db1Props.description = "new description";
            db1Props.price = 12.22m;

            db.Update(db1Props);
            db1Props = (ProductProps)db.Retrieve(1);

            Assert.AreEqual(db1Props.code, "XXXX");
            Assert.AreEqual(db1Props.description, "new description");
            Assert.AreEqual(db1Props.price, 12.22m);
            Assert.AreEqual(db1Props.ConcurrencyID, 2);
        }
    }
}
