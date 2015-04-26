using System;
using System.Linq;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class BussinessCuponTest
    {
        private BussinessCupon _cupon;
        private Bussiness _bussiness;
        [TestInitialize]
        public void TestInit()
        {
            using (ModelContainer m = new ModelContainer())
            {
                BussinessOwner bo = new BussinessOwner()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il"
                };
                m.Users.Add(bo);
                Bussiness b = new Bussiness()
                {
                    BussinessOwner = bo,
                    Category = Category.Resturants,
                    Description = "",
                    Location = new Location() { Latitude = 31.1234, Longtitude = 31.1234 },
                    Name = "StakeHouse"
                };
                _bussiness = m.Bussinesses.Add(b);
                BussinessCupon u = new BussinessCupon()
                {
                    Name = "Free Pizza",
                    Approved = true,
                    Category = Category.Resturants,
                    Description = "Free Pizza",
                    ExpirationDate= DateTime.Now,
                    OriginalPrice = 45.0,
                    Location = new Location() { Longtitude = 31.1254, Latitude = 32.1234 },
                    Price = 0.0,
                    Rate = Rate.NA
                };
                _cupon = (BussinessCupon)m.Cupons.Add(u);
                b.BussinessCupons.Add(u);
                m.SaveChanges();
            }
        }

        [TestMethod]
        public void AddBussinessCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Cupons.Count();
                BussinessCupon u = new BussinessCupon()
                {
                    Name = "Free Pizza",
                    Approved = true,
                    Category = Category.Resturants,
                    Description = "Free Pizza",
                    ExpirationDate = DateTime.Now,
                    OriginalPrice = 45.0,
                    Location = new Location() { Longtitude = 31.1254, Latitude = 32.1234 },
                    Price = 0.0,
                    Rate = Rate.NA
                };
                m.Cupons.Add(u);
                _bussiness.BussinessCupons.Add(u);
                m.Bussinesses.Attach(_bussiness);
                m.SaveChanges();
                Assert.AreEqual(i + 1, m.Cupons.Count());
            }
        }

        [TestMethod]
        public void RemoveBussinessCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Cupons.Count();
                BussinessCupon u = new BussinessCupon()
                {
                    Name = "Free Pizza",
                    Approved = true,
                    Category = Category.Resturants,
                    Description = "Free Pizza",
                    ExpirationDate = DateTime.Now,
                    OriginalPrice = 45.0,
                    Location = new Location() { Longtitude = 31.1254, Latitude = 32.1234 },
                    Price = 0.0,
                    Rate = Rate.NA
                };
                m.Cupons.Add(u);
                _bussiness.BussinessCupons.Add(u);
                m.Bussinesses.Attach(_bussiness);
                m.SaveChanges();
                m.Cupons.Remove(u);
                _bussiness.BussinessCupons.Remove(u);
                m.Bussinesses.Attach(_bussiness);
                m.SaveChanges();
                Assert.AreEqual(i, m.Cupons.Count());
            }
        }

        [TestMethod]
        public void UpdateBussinessCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                Category old = _cupon.Category;
                _cupon.Category = Category.Pleasure;
                m.Cupons.Attach(_cupon);
                m.SaveChanges();
                BussinessCupon u = m.Cupons.OfType<BussinessCupon>().First(x => x.Id == _cupon.Id);
                Assert.AreEqual(_cupon.Category, u.Category);
            }
        }

        [TestMethod]
        public void SelectBussinessCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Cupons.Count();
                BussinessCupon u1 = m.Cupons.OfType<BussinessCupon>().First(x => x.Id == _cupon.Id);
                Assert.AreEqual(_cupon.Id, u1.Id);
            }
        }
    }
}
