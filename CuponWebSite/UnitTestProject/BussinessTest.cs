using System;
using System.Linq;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class BussinessTest
    {
        private Bussiness _bussiness;
        private BussinessOwner _owner;
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
                _owner = (BussinessOwner)m.Users.Add(u);
                m.SaveChanges();
                //Bussiness b = new Bussiness()
                //{
                //    BussinessOwner = _owner,
                //    Category = Category.Resturants,
                //    Description = "",
                //    Location = new Location() { Latitude = 31.1234, Longtitude = 31.1234 },
                //    Name = "StakeHouse"
                //};
                //_bussiness = m.Bussinesses.Add(b);
                m.SaveChanges();
            }
        }

        [TestMethod]
        public void AddBussiness()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Bussinesses.Count();
                Bussiness u = new Bussiness()
                {
                    BussinessOwner = _owner,
                    Category = Category.Resturants,
                    Description = "",
                    Location = new Location() { Latitude = 31.1234, Longtitude = 31.1234 },
                    Name = "StakeHouse"
                };
                _bussiness = m.Bussinesses.Add(u);
                m.SaveChanges();
                Assert.AreEqual(i + 1, m.Bussinesses.Count());
            }
        }

        [TestMethod]
        public void RemoveBussiness()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Bussinesses.Count();
                Bussiness u = new Bussiness()
                {
                    BussinessOwner = _owner,
                    Category = Category.Resturants,
                    Description = "",
                    Location = new Location() { Latitude = 31.1234, Longtitude = 31.1234 },
                    Name = "StakeHouse"
                };
                m.Bussinesses.Add(u);
                m.SaveChanges();
                m.Bussinesses.Remove(u);
                m.SaveChanges();
                Assert.AreEqual(i, m.Bussinesses.Count());
            }
        }

        [TestMethod]
        public void UpdateBussiness()
        {
            using (ModelContainer m = new ModelContainer())
            {
                Bussiness u = new Bussiness()
                {
                    BussinessOwner = _owner,
                    Category = Category.Resturants,
                    Description = "",
                    Location = new Location() { Latitude = 31.1234, Longtitude = 31.1234 },
                    Name = "StakeHouse"
                };
                u = m.Bussinesses.Add(u);
                m.SaveChanges();
                u.Name = "updated";
                m.Bussinesses.Attach(u);
                m.SaveChanges();
                Bussiness u1= m.Bussinesses.First(x => x.Id == u.Id);
                Assert.AreEqual(u.Name, u1.Name);
            }
        }

        [TestMethod]
        public void SelectBussiness()
        {
            using (ModelContainer m = new ModelContainer())
            {
                Bussiness u = new Bussiness()
                {
                    BussinessOwner = _owner,
                    Category = Category.Resturants,
                    Description = "",
                    Location = new Location() { Latitude = 31.1234, Longtitude = 31.1234 },
                    Name = "StakeHouse"
                };
                u = m.Bussinesses.Add(u);
                m.SaveChanges();
                Bussiness u1 = m.Bussinesses.First(x => x.Id == u.Id);
                Assert.AreEqual(u, u1);
            }
        }
    }
}
