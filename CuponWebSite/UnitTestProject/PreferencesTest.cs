using System;
using System.Linq;
using CuponWebSite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class PreferencesTest
    {
        private BasicUser _user;
        private Preference _preference;
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
                    Location = new Location() { Longtitude = 31.1254, Latitude = 32.1234 },
                    PhoneNumber = "055-5432165"
                };
                Preference p = new Preference()
                {
                    Category = Category.Resturants
                };
                u.Preferences.Add(p);
                _preference = m.Preferences.Add(p);
                _user = (BasicUser)m.Users.Add(u);
                m.SaveChanges();
            }
        }

        [TestMethod]
        public void AddPreference()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Preferences.Count();
                Preference u = new Preference()
                {
                    Category = Category.Resturants

                };
                m.Preferences.Add(u);
                _user.Preferences.Add(u);
                m.Users.Attach(_user);
                m.SaveChanges();
                Assert.AreEqual(i + 1, m.Preferences.Count());
            }
        }

        [TestMethod]
        public void RemovePreference()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Preferences.Count();
                Preference u = new Preference()
                {
                    Category = Category.Resturants
                };
                m.Preferences.Add(u);
                 _user.Preferences.Add(u);
                m.Users.Attach(_user);
                m.SaveChanges();
                m.Preferences.Remove(u);
                _user.Preferences.Remove(u);
                m.Users.Attach(_user);
                m.SaveChanges();
                Assert.AreEqual(i, m.Preferences.Count());
            }
        }

        [TestMethod]
        public void UpdatePreference()
        {
            using (ModelContainer m = new ModelContainer())
            {
                Category old = _preference.Category;
                _preference.Category = Category.Pleasure;
                m.Preferences.Attach(_preference);
                m.SaveChanges();
                Preference u = m.Preferences.FirstOrDefault(x => x.Id == _preference.Id);
                Assert.AreEqual(_preference.Category, u.Category);
            }
        }

        [TestMethod]
        public void SelectPreference()
        {
            using (ModelContainer m = new ModelContainer())
            {
                var i = m.Preferences.Count();
                Preference u1 = m.Preferences.OfType<Preference>().FirstOrDefault(x => x.Id == _preference.Id);
                Assert.AreEqual(_preference.Id, u1.Id);
            }
        }
    }
}
