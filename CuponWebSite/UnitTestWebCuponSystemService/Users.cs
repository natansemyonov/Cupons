using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using CuponWebSite.Controller;


namespace UnitTestWebCuponSystemService
{
    [TestClass]
    public class Users
    {
        UserServices target = new UserServices();

        [TestMethod]
        public void TestAuthenticateUser()
        {
            string expected = "false";
            string actual;

            actual = target.AuthenticateUser("natan","semyonov");

            Assert.AreNotEqual(expected,actual );
        }

        [TestMethod]
        public void TestBussinessOwnerRegister()
        {
            string expected = "false";
            string actual;

            actual = target.BussinessOwnerRegister("Ben" + DateTime.Now.ToString(), "gurion", "bgu@bgu.ac.il", "");
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void TestUpdateBasicUser()
        {
            bool expected = false;
            bool actual;

            actual = target.UpdateBasicUser("1","Ben" + DateTime.Now.ToString(), "gurion", "bgu@bgu.ac.il", "0569879879","1","1","");
            Assert.AreNotEqual(expected, actual);
        }
         [TestMethod]
        public void TestBasicUserRegister()
        {
            string expected = "false";
            string actual;

            actual = target.BasicUserRegister("Ben" + DateTime.Now.ToString(), "gurion", "bgu@bgu.ac.il", "0", "0569879879", DateTime.Now.ToString(), "1", "1", "");
            Assert.AreNotEqual(expected, actual);
        }

         [TestMethod]
         public void TestFindBasicUserByName_Email()
        {
            string expected = "false";
            string actual;

            actual = target.FindBasicUserByName_Email("ben", "bgu@bgu.ac.il");
            Assert.AreNotEqual(expected, actual);
        }
        
        
    }
}
