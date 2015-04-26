using System;
using System.Linq;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class BussinessOwnerTests
    {
        private BussinessOwner _user;
        [TestInitialize]
        public void TestInit()
        {
            using (ModelContainer m = new ModelContainer())
            {
                BussinessOwner u = new BussinessOwner()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il"
                };
                _user = (BussinessOwner)m.Users.Add(u);
                m.SaveChanges();
            }
        }

        [TestMethod]
        public void AddBussinessOwner()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                BussinessOwner u = new BussinessOwner()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il",

                };
                m.Users.Add(u);
                m.SaveChanges();
                Assert.AreEqual(i + 1, m.Users.Count());
            }
        }

        [TestMethod]
        public void RemoveBussinessOwner()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                BussinessOwner u = new BussinessOwner()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il"
                };
                m.Users.Add(u);
                m.SaveChanges();
                m.Users.Remove(u);
                m.SaveChanges();
                Assert.AreEqual(i, m.Users.Count());
            }
        }

        [TestMethod]
        public void UpdateBussinessOwner()
        {
            using (ModelContainer m = new ModelContainer())
            {
                string old = _user.Email;
                _user.Email = "updated@gmail.com";
                m.Users.Attach(_user);
                m.SaveChanges();
                BussinessOwner u = m.Users.OfType<BussinessOwner>().First(x => x.Id == _user.Id);
                Assert.AreEqual(_user.Email, u.Email);
            }
        }

        [TestMethod]
        public void SelectBussinessOwner()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                BussinessOwner u1 = m.Users.OfType<BussinessOwner>().First(x => x.Id == _user.Id);
                Assert.AreEqual(_user.Id, u1.Id);
            }
        }
        
    }
}
