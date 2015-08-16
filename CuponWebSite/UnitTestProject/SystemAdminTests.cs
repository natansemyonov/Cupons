using System;
using System.Linq;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class SystemAdminTests
    {
        private SystemAdmin _user;
        [TestInitialize]
        public void TestInit()
        {
            using (ModelContainer m = new ModelContainer())
            {
                SystemAdmin u = new SystemAdmin()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il"
                };
                _user = (SystemAdmin) m.Users.Add(u);
                m.SaveChanges();
            }
        }

        [TestMethod]
        public void AddSystemAdmin()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                SystemAdmin u = new SystemAdmin()
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
        public void RemoveSystemAdmin()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                SystemAdmin u = new SystemAdmin()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il"
                };
                m.Users.Add(u);
                m.SaveChanges();
                m.Users.Remove(u);
                m.SaveChanges();
                Assert.AreEqual(i , m.Users.Count());
            }
        }
        
        [TestMethod]
        public void UpdateSystemAdmin()
        {
            using (ModelContainer m = new ModelContainer())
            {
                string old = _user.Email;
                _user.Email = "updated@gmail.com";
                m.Users.Attach(_user);
                m.SaveChanges();
                SystemAdmin u = m.Users.OfType<SystemAdmin>().FirstOrDefault(x => x.Id == _user.Id);
                Assert.AreEqual(_user.Email, u.Email);
            }
        }
        
        [TestMethod]
        public void SelectSystemAdmin()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                SystemAdmin u1 = m.Users.OfType<SystemAdmin>().FirstOrDefault(x => x.Id == _user.Id);
                Assert.AreEqual(_user.Id,u1.Id);
            }
        }
        
    }
}
