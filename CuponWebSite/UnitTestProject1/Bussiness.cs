using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuponWebSite.Controller;
using Newtonsoft.Json;

namespace UnitTestProject1
{
    [TestClass]
    public class Bussiness
    {
        BussinessServices target = new BussinessServices();

        [TestMethod]
        public void TestAddBussiness()
        {
            bool expected = false;
            bool actual;

            actual = target.AddBussiness("Piza" + DateTime.Now.ToString(), "Best Piza", "0", "2", "2", "", "2");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestDeleteBussiness()
        {
            bool expected = false;
            bool actual;

            actual = target.DeleteBussiness("2");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestApproveBussiness()
        {
            bool expected = false;
            bool actual;

            actual = target.ApproveBussiness("1");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestUpdateBussiness()
        {
            bool expected = false;
            bool actual;

            actual = target.UpdateBussiness("1","Piza" + DateTime.Now.ToString(), "Best Piza", "0", "2", "2", "");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindBussinessByName()
        {
            string expected = "false";
            string actual;

            actual = target.FindBussinessByName("Piza5/28/2015 1:33:27 PM");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindBussinessOfOwner()
        {
            string expected = "false";
            string actual;

            actual = target.FindBussinessOfOwner("2");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void FindBussinessByCategory()
        {
            string expected = "false";
            string actual;

            actual = target.FindBussinessByCategory("0");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetAllUnapprovedBussiness()
        {
            string expected = "";
            string actual;

            actual = target.GetAllUnapprovedBussiness();

            Assert.AreNotEqual(expected, actual);
        }
    }
}
