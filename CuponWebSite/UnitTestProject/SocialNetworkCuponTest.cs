using System;
using System.Linq;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class SocialNetworkCuponTest
    {
        private SocialNetworkCupon _cupon;
        private User _user;
        [TestInitialize]
        public void TestInit()
        {
            using (ModelContainer m = new ModelContainer())
            {
                User use = new User()
                {
                    UserName = "testing" + DateTime.Now.ToString(),
                    Password = "test",
                    Email = "test@post.bgu.ac.il"
                };
                _user = m.Users.Add(use);
                SocialNetworkCupon u = new SocialNetworkCupon()
                {
                    Name = "Free Pizza",
                    Approved = true,
                    Category = Category.Resturants,
                    Description = "Free Pizza",
                    ExpirationDate = DateTime.Now,
                    OriginalPrice = 45.0,
                    Location = new Location() { Longtitude = 31.1254, Latitude = 32.1234 },
                    Price = 0.0,
                    Rate = Rate.NA,
                    URL = "I dont Know",
                    PhoneNumber="054-4543222"
                };
                _cupon = (SocialNetworkCupon)m.Cupons.Add(u);
                _user.SocialNetworkCupons.Add(u);
                use.SocialNetworkCupons.Add(u);
                m.SaveChanges();
            }
        }

        [TestMethod]
        public void AddSocialNetworkCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Cupons.Count();
                SocialNetworkCupon u = new SocialNetworkCupon()
                {
                    Name = "Free Pizza",
                    Approved = true,
                    Category = Category.Resturants,
                    Description = "Free Pizza",
                    ExpirationDate = DateTime.Now,
                    OriginalPrice = 45.0,
                    Location = new Location() { Longtitude = 31.1254, Latitude = 32.1234 },
                    Price = 0.0,
                    Rate = Rate.NA,
                    URL = "",
                    PhoneNumber = ""
                };
                m.Cupons.Add(u);
                _user.SocialNetworkCupons.Add(u);
                m.Users.Attach(_user);
                m.SaveChanges();
                Assert.AreEqual(i + 1, m.Cupons.Count());
            }
        }

        [TestMethod]
        public void RemoveSocialNetworkCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Cupons.Count();
                SocialNetworkCupon u = new SocialNetworkCupon()
                {
                    Name = "Free Pizza",
                    Approved = true,
                    Category = Category.Resturants,
                    Description = "Free Pizza",
                    ExpirationDate = DateTime.Now,
                    OriginalPrice = 45.0,
                    Location = new Location() { Longtitude = 31.1254, Latitude = 32.1234 },
                    Price = 0.0,
                    Rate = Rate.NA,
                    URL = "I dont care",
                    PhoneNumber = "054-4543222"
                };
                m.Cupons.Add(u);
                _user.SocialNetworkCupons.Add(u);
                m.Users.Attach(_user);
                m.SaveChanges();
                m.Cupons.Remove(u);
                _user.SocialNetworkCupons.Remove(u);
                m.Users.Attach(_user);
                m.SaveChanges();
                Assert.AreEqual(i, m.Cupons.Count());
            }
        }

        [TestMethod]
        public void UpdateSocialNetworkCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                Category old = _cupon.Category;
                _cupon.Category = Category.Pleasure;
                m.Cupons.Attach(_cupon);
                m.SaveChanges();
                SocialNetworkCupon u = m.Cupons.OfType<SocialNetworkCupon>().First(x => x.Id == _cupon.Id);
                Assert.AreEqual(_cupon.Category, u.Category);
            }
        }

        [TestMethod]
        public void SelectSocialNetworkCupon()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Cupons.Count();
                SocialNetworkCupon u1 = m.Cupons.OfType<SocialNetworkCupon>().First(x => x.Id == _cupon.Id);
                Assert.AreEqual(_cupon.Id, u1.Id);
            }
        }
    }
}
