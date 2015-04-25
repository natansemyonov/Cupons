using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuponDataBase;

namespace CuponTests
{
    [TestClass]
    public class BasicUserTest
    {
        private CuponContext context;
        [TestInitialize]
        public void TestInit()
        {
            context = new CuponContext();
        }
        [TestMethod]
        public void TestAddUser()
        {
            using (context)
            {
                BasicUser user = new BasicUser() {BasicUserId = 1, User_name = "Natan", Password = "a", Email = "", Gender = "", Birth_Date = DateTime.Now, LastKnownLocation = "", Phone_Number = "" };
                context.Basic_Users.Add(user);

                context.SaveChanges();
                //BasicUser foundBasicUser = null;
                foreach (BasicUser foundBasicUser in context.Basic_Users.Local)
                    if (foundBasicUser.User_name == "Natan")
                    {
                        Assert.AreNotEqual("Natan", foundBasicUser.BasicUserId);
                        break;
                    }

                //var foundBasicUser = context.Basic_Users.SqlQuery("SELECT * FROM dbo.BasicUsers where User_Name = 'Natan'").SingleAsync();
                
            }
            
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            TestInit();
            
        }

        [TestMethod]
        public void TestUpdateUser()
        {
            TestInit();
           
        }
    }
}
