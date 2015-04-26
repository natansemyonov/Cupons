using System;
using System.Linq;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UserTests
    {
        private User _user;
        [TestInitialize]
        public void TestInit()
        {
            using (ModelContainer m = new ModelContainer())
            {
                    User u = new User()
                    {
                        UserName = "testing" + DateTime.Now.ToString(),
                        Password = "test",
                        Email = "test@post.bgu.ac.il"
                    };
                    _user = m.Users.Add(u);
                    m.SaveChanges();
            }
        }

        [TestMethod]
        public void AddUser()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                User u = new User()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il"
                };
                m.Users.Add(u);
                m.SaveChanges();
                Assert.AreEqual(i + 1, m.Users.Count());
            }
        }
        [TestMethod]
        public void RemoveUser()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                User u = new User()
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
        public void UpdateUser()
        {
            using (ModelContainer m = new ModelContainer())
            {
                string old = _user.Email;
                _user.Email = "updated@gmail.com";
                m.Users.Attach(_user);
                m.SaveChanges();
                User u = m.Users.First(x => x.Id == _user.Id);
                Assert.AreEqual(_user.Email, u.Email);
            }
        }
        
        [TestMethod]
        public void SelectUser()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                User u = new User()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il"
                };
                u = m.Users.Add(u);
                m.SaveChanges();
                User u1 = m.Users.First(x => x.Id == u.Id);
                Assert.AreEqual(u,u1);
            }
        }
        
    }
}
