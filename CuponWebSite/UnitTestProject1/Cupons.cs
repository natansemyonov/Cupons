using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuponWebSite.Controller;
using Newtonsoft.Json;

namespace UnitTestProject1
{
    [TestClass]
    public class Cupons
    {
        CuponServices target = new CuponServices();

        [TestMethod]
        public void TestAddBussinessCupon()
        {
            bool expected = false;
            bool actual;

            actual = target.AddBussinessCupon("Piza" + DateTime.Now.ToString(),"best piza ever", "3", "2", DateTime.Now.ToString(), "0", "3", "3", "","1");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestDeleteCupon()
        {
            bool expected = false;
            bool actual;

            actual = target.DeleteCupon("2");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestUpdateCupon()
        {
            bool expected = false;
            bool actual;

            actual = target.UpdateCupon("1", "Piza", "best piza ever", "3", "2", "2", DateTime.Now.ToString(),"0","false","3","3","");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetAllBussinessCupons()
        {
            string expected = "false";
            string actual;

            actual = target.GetAllBussinessCupons();

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetAllUnAprrovedCupons()
        {
            string expected = "false";
            string actual;

            actual = target.GetAllUnAprrovedCupons();

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindCuponsById()
        {
            string expected = "false";
            string actual;

            actual = target.FindCuponsById("1");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindCuponsByNameAndDescription()
        {
            string expected = "false";
            string actual;

            actual = target.FindCuponsByNameAndDescription("Piza");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindCuponsByLocation()
        {
            string expected = "false";
            string actual;

            actual = target.FindCuponsByLocation("3","3","4");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindCuponByPreference()
        {
            string expected = "false";
            string actual;

            actual = target.FindCuponByPreference("0");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestApproveCupon()
        {
            bool expected = false;
            bool actual;

            actual = target.ApproveCupon("1","2");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindCuponByBussiness()
        {
            string expected = "false";
            string actual;

            actual = target.FindCuponByBussiness("1");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddSocialNetworkCupon()
        {
            bool expected = false;
            bool actual;

            actual = target.AddSocialNetworkCupon("bestpiza.co.il","1");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestPurchaseCupon()
        {
            string expected = "false";
            string actual;

            actual = target.PurchaseCupon("1","1");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindPurchasedCuponBySerialKey()
        {
            string expected = "false";
            string actual;

            actual = target.FindPurchasedCuponBySerialKey("6730-2454-3646");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestUseCupon()
        {
            bool expected = false;
            bool actual;

            actual = target.UseCupon("1","6730-2454-3646");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestRateCupon()
        {
            bool expected = false;
            bool actual;

            actual = target.RateCupon("1","4");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestCuponHistory()
        {
            string expected = "false";
            string actual;

            actual = target.CuponHistory("1");

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestBussinessCuponHistory()
        {
            string expected = "false";
            string actual;

            actual = target.BussinessCuponHistory("1");

            Assert.AreNotEqual(expected, actual);
        }
    }
}