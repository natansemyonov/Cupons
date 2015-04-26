using System;
using System.Linq;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class CuponTest
    {
        private Cupon _cupon;
        [TestInitialize]
        public void TestInit()
        {
            using (ModelContainer m = new ModelContainer())
            {
                Cupon u = new Cupon()
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
                _cupon = m.Cupons.Add(u);
                m.SaveChanges();
            }
        }

        [TestMethod]
        public void AddCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Cupons.Count();
                Cupon u = new Cupon()
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
                m.SaveChanges();
                Assert.AreEqual(i + 1, m.Cupons.Count());
            }
        }

        [TestMethod]
        public void RemoveCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Cupons.Count();
                Cupon u = new Cupon()
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
                m.SaveChanges();
                m.Cupons.Remove(u);
                m.SaveChanges();
                Assert.AreEqual(i, m.Cupons.Count());
            }
        }

        [TestMethod]
        public void UpdateCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                Category old = _cupon.Category;
                _cupon.Category = Category.Pleasure;
                m.Cupons.Attach(_cupon);
                m.SaveChanges();
                Cupon u = m.Cupons.First(x => x.Id == _cupon.Id);
                Assert.AreEqual(_cupon.Category, u.Category);
            }
        }

        [TestMethod]
        public void SelectCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Cupons.Count();
                Cupon u1 = m.Cupons.OfType<Cupon>().First(x => x.Id == _cupon.Id);
                Assert.AreEqual(_cupon.Id, u1.Id);
            }
        }
    }
}
