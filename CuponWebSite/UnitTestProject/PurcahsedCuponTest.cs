using System;
using System.Linq;
using System.Runtime.InteropServices;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class PurcahsedCuponTest
    {
        private PurchasedCupon _purchased;
        private BussinessCupon _cupon;
        private BasicUser _user;

        [TestInitialize]
        public void TestInit()
        {
            using (ModelContainer m = new ModelContainer())
            {
                BasicUser use = new BasicUser()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il",
                    BirthDate = DateTime.Now,
                    Gender = Gender.Female,
                    Location = new Location() { Longtitude = 31.1254, Latitude = 32.1234 },
                    PhoneNumber = "055-5432165"
                };
                _user = (BasicUser)m.Users.Add(use);
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
                m.Bussinesses.Add(b);
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
        public void AddPurchasedCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.PurchasedCupons.Count();
                PurchasedCupon u = new PurchasedCupon()
                {
                    BasicUser = _user,
                    BussinessCupon = _cupon,
                    SerialKey="1234567890",
                    Rate = Rate.NA,
                    State = CuponState.Pending
                };
                m.PurchasedCupons.Add(u);
                _user.PurchasedCupons.Add(u);
                _cupon.PurchasedCupons.Add(u);
                m.Users.Attach(_user);
                m.SaveChanges();
                m.Cupons.Attach(_cupon);
                m.SaveChanges();
                Assert.AreEqual(i + 1, m.PurchasedCupons.Count());
            }
        }

        [TestMethod]
        public void RemovePurchasedCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.PurchasedCupons.Count();
                PurchasedCupon u = new PurchasedCupon()
                {
                    BasicUser = _user,
                    BussinessCupon = _cupon,
                    SerialKey = "1234567890",
                    Rate = Rate.NA,
                    State = CuponState.Pending
                };
                m.PurchasedCupons.Add(u);
               _user.PurchasedCupons.Add(u);
                m.Users.Attach(_user);
                m.SaveChanges();
                m.PurchasedCupons.Remove(u);
                _user.PurchasedCupons.Remove(u);
                m.Users.Attach(_user);
                m.SaveChanges();
                Assert.AreEqual(i, m.PurchasedCupons.Count());
            }
        }

        [TestMethod]
        public void UpdatePurchasedCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                PurchasedCupon p = new PurchasedCupon()
                {
                    BasicUser = _user,
                    BussinessCupon = _cupon,
                    SerialKey = "1234567890",
                    Rate = Rate.NA,
                    State = CuponState.Pending
                };
                _purchased = m.PurchasedCupons.Add(p);
                m.SaveChanges();
                _purchased.State = CuponState.Used;
                m.PurchasedCupons.Attach(_purchased);
                m.SaveChanges();
                PurchasedCupon u = m.PurchasedCupons.First(x => x.Id == _purchased.Id);
                Assert.AreEqual(_purchased.State, u.State);
            }
        }

        [TestMethod]
        public void SelectPurchasedCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                PurchasedCupon p = new PurchasedCupon()
                {
                    BasicUser = _user,
                    BussinessCupon = _cupon,
                    SerialKey = "1234567890",
                    Rate = Rate.NA,
                    State = CuponState.Pending
                };
                _purchased = m.PurchasedCupons.Add(p);
                m.SaveChanges();
                PurchasedCupon u1 = m.PurchasedCupons.First(x => x.Id == _purchased.Id);
                Assert.AreEqual(_purchased.Id, u1.Id);
            }
        }
    }
}
