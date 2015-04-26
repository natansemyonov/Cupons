using System;
using System.Linq;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class BasicUserTests
    {
        private BasicUser _user;
        [TestInitialize]
        public void TestInit()
        {
            using (ModelContainer m = new ModelContainer())
            {
                BasicUser u = new BasicUser()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il",
                    BirthDate = DateTime.Now,
                    Gender = Gender.Female,
                    Location = new Location(){Longtitude=31.1254,Latitude = 32.1234},
                    PhoneNumber = "055-5432165"
                };
                _user = (BasicUser)m.Users.Add(u);
                m.SaveChanges();
            }
        }

        [TestMethod]
        public void AddBasicUser()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                BasicUser u = new BasicUser()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il",
                    BirthDate = DateTime.Now,
                    Gender = Gender.Female,
                    Location = new Location() { Longtitude = 31.1254, Latitude = 32.1234 },
                    PhoneNumber = "055-5432165",

                };
                m.Users.Add(u);
                m.SaveChanges();
                Assert.AreEqual(i + 1, m.Users.Count());
            }
        }

        [TestMethod]
        public void RemoveBasicUser()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                BasicUser u = new BasicUser()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il",
                    BirthDate = DateTime.Now,
                    Gender = Gender.Female,
                    Location = new Location(){Longtitude=31.1254,Latitude = 32.1234},
                    PhoneNumber = "055-5432165",
                };
                m.Users.Add(u);
                m.SaveChanges();
                m.Users.Remove(u);
                m.SaveChanges();
                Assert.AreEqual(i, m.Users.Count());
            }
        }

        [TestMethod]
        public void UpdateBasicUser()
        {
            using (ModelContainer m = new ModelContainer())
            {
                string old = _user.Email;
                _user.Email = "updated@gmail.com";
                m.Users.Attach(_user);
                m.SaveChanges();
                BasicUser u = m.Users.OfType<BasicUser>().First(x => x.Id == _user.Id);
                Assert.AreEqual(_user.Email, u.Email);
            }
        }

        [TestMethod]
        public void SelectBasicUser()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Users.Count();
                BasicUser u1 = m.Users.OfType<BasicUser>().First(x => x.Id == _user.Id);
                Assert.AreEqual(_user.Id, u1.Id);
            }
        }
        
    }
}
