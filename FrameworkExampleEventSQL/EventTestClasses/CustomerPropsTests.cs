using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EventPropsClasses;

namespace EventTestClasses
{
    [TestFixture]
    public class CustomerPropsTests
    {
        CustomerProps c;

        [SetUp]
        public void SetUp()
        {
            c = new CustomerProps();
            c.ID = 1;
            c.name = "Jane Doe";
            c.address = "555 W. 5th";
            c.city = "Eugene";
            c.state = "OR";
            c.ConcurrencyID = 1;
        }

        [Test]
        public void TestClone()
        {
            CustomerProps c2 = (CustomerProps)c.Clone();
            Assert.AreEqual(c.ID, c2.ID);
            Assert.AreEqual(c.name, c2.name);
            Assert.AreEqual(c.address, c2.address);
            Assert.AreEqual(c.city, c2.city);
            Assert.AreEqual(c.state, c2.state);
            Assert.AreEqual(c.zipCode, c2.zipCode);
            Assert.AreEqual(c.ConcurrencyID, c2.ConcurrencyID);

            c.ID = 4;
            Assert.AreNotEqual(c.ID, c2.ID);
        }

        [Test]
        public void TestGetState()
        {
            string pString = c.GetState();
            Console.WriteLine(pString);
        }

        [Test]
        public void TestSetState()
        {
            string pString = c.GetState();
            CustomerProps c2 = new CustomerProps();
            c2.SetState(pString);

            Assert.AreEqual(c.ID, c2.ID);
            Assert.AreEqual(c.name, c2.name);
            Assert.AreEqual(c.address, c2.address);
            Assert.AreEqual(c.city, c2.city);
            Assert.AreEqual(c.state, c2.state);
            Assert.AreEqual(c.zipCode, c2.zipCode);
            Assert.AreEqual(c.ConcurrencyID, c2.ConcurrencyID);
        }
    }
}
