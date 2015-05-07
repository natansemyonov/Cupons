using System;
using System.Collections.Generic;
using System.ServiceModel.Activities;
using System.ServiceModel.Web;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

namespace CuponWebSite
{
    /// <summary>
    /// Summary description for CuponSystemWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class CuponSystemWebService : System.Web.Services.WebService
    {
        public CuponSystemWebService()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent();
        }

        #region --------USERS---------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AuthenticateUser(string userName, string password)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Users.Where(x => x.UserName == userName && x.Password == password).ToList();
                if (data.Count == 0)
                    return false;
                return true;

            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool BussinessOwnerRegister(string userName, string password, string email)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Users.Where(x => x.UserName == userName).ToList();
                if (data.Count != 0)
                    return false;
                BussinessOwner user = new BussinessOwner
                {
                    UserName = userName,
                    Password = password,
                    Email = email
                };
                entities.Users.Add(user);
                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool UpdateBasicUser(int userId, string userName, string password, string email, string phoneNumber, Location location)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.First(x => x.Id == userId);
                if (user == null)
                    return false;
                user.UserName = userName;
                user.Password = password;
                user.Email = email;
                ((BasicUser)user).PhoneNumber = phoneNumber;
                ((BasicUser)user).Location = location;

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindBasicUserByName_Email( string userName, string email)
        {
            BasicUser bUser;
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.First(x => x.UserName == userName & x.Email == email);
                if (user == null)
                    return "";
                bUser = new BasicUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = ((BasicUser)user).PhoneNumber,
                    Location = ((BasicUser)user).Location,
                    Password = user.Password,
                    BirthDate = ((BasicUser)user).BirthDate,
                    Gender = ((BasicUser)user).Gender,
                    Preferences = ((BasicUser)user).Preferences
                };
                return JsonConvert.SerializeObject(bUser, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool ChangePassword(int userId, string newPassword)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.First(x => x.Id == userId);
                if (user == null)
                    return false;
                user.Password = newPassword;
                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddPreference(Category category, int userId)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.First(x => x.Id == userId);
                if (user == null)
                    return false;
                var data = entities.Preferences.Where(x => x.Category == category & x.BasicUser.Id == userId).ToList();
                if (data.Count != 0)
                    return false;
                Preference preference = new Preference
                {
                    Category = category,
                    BasicUser = (BasicUser)user
                };

                ((BasicUser)user).Preferences.Add(preference);
                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool BasicUserRegister(string userName, string password, string email, Gender gender, string phoneNumber, DateTime birthDate, Location location)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Users.Where(x => x.UserName == userName).ToList();
                if (data.Count != 0)
                    return false;
                BasicUser basicUser = new BasicUser
                {
                    UserName = userName,
                    Password = password,
                    Email = email,
                    Gender = gender,
                    PhoneNumber = phoneNumber,
                    BirthDate = birthDate,
                    Location = location
                };
                entities.Users.Add(basicUser);
                entities.SaveChanges();
                return true;
            }
        }
        #endregion --------USERS---------------

        #region --------Bussiness---------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddBussiness(string name, string description, Category category, Location location, int bussinessOwnerId)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.Where(x => x.Name == name).ToList();
                if (data.Count != 0)
                    return false;
                var bussinessOwner = entities.Users.First(x => x.Id == bussinessOwnerId);
                if (bussinessOwner == null)
                    return false;
                Bussiness bussiness = new Bussiness
                {
                    Name = name,
                    Description = description,
                    Category = category,
                    Location = location,
                    BussinessOwner = (BussinessOwner)bussinessOwner
                };
                entities.Bussinesses.Add(bussiness);
                entities.SaveChanges();
                return true;
            }
        }
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool DeleteBussiness(int bussinessId)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.First(x => x.Id == bussinessId);
                if (data == null)
                    return false;
                entities.Bussinesses.Remove(data);
                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool UpdateBussiness(int bussinessId, string name, string description, Category category, Location location)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var bussiness = entities.Bussinesses.First(x => x.Id == bussinessId);
                if (bussiness == null)
                    return false;
                bussiness.Name = name;
                bussiness.Description = description;
                bussiness.Category = category;
                bussiness.Location = location;
                bussiness.Id = bussinessId;

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String FindBussinessByName(string bussinessName)
        {
            Cupon cupon;
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.First(x => x.Name == bussinessName);
                if (data == null)
                    return "";
                Bussiness bussiness = new Bussiness
                {
                    Name = data.Name,
                    Description = data.Description,
                    Category = data.Category,
                    Location = data.Location,
                    Id = data.Id,
                    BussinessCupons =data.BussinessCupons,
                    BussinessOwner = data.BussinessOwner
                };
                return JsonConvert.SerializeObject(bussiness, Formatting.Indented);
            }
        }
        #endregion --------Bussiness---------------

        #region --------Cupons---------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddCupon(string name, string description, double originalPrice, double price, Rate rate, DateTime expirationDate, Category category, bool approved, Location location)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.Where(x => x.Name == name).ToList();
                if (data.Count != 0)
                    return false;
                Cupon cupon = new Cupon()
                 {
                     Name = name,
                     Description = description,
                     Category = category,
                     Location = location,
                     OriginalPrice = originalPrice,
                     Price = price,
                     Rate = rate,
                     ExpirationDate = expirationDate,
                     Approved = approved
                 };
                entities.Cupons.Add(cupon);
                entities.SaveChanges();
                return true;

            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool DeleteCupon(int cuponId)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.First(x => x.Id == cuponId);
                if (data == null)
                    return false;
                entities.Cupons.Remove(data);
                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool UpdateCupon(int cuponId, string name, string description, double originalPrice, double price, Rate rate, DateTime expirationDate, Category category, bool approved, Location location)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var cupon = entities.Cupons.First(x => x.Id == cuponId);
                if (cupon == null)
                    return false;
                cupon.Name = name;
                cupon.Description = description;
                cupon.Category = category;
                cupon.Location = location;
                cupon.OriginalPrice = originalPrice;
                cupon.Price = price;
                cupon.Rate = rate;
                cupon.ExpirationDate = expirationDate;
                cupon.Approved = approved;

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String FindCuponById(int cuponId)
        {
            Cupon cupon;
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.First(x => x.Id == cuponId);
                if (data == null)
                    return "";
                cupon = new Cupon
                {
                    Name = data.Name,
                    Description = data.Description,
                    Category = data.Category,
                    Location = data.Location,
                    OriginalPrice = data.OriginalPrice,
                    Price = data.Price,
                    Rate = data.Rate,
                    ExpirationDate = data.ExpirationDate,
                    Approved = data.Approved,
                    Id = data.Id
                };
                return JsonConvert.SerializeObject(cupon, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String FindCuponByNameAndDescription(string name, string description)
        {
            Cupon cupon;
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.First(x => x.Name == name & x.Description == description);
                if (data == null)
                    return "";
                cupon = new Cupon
                {
                    Name = data.Name,
                    Description = data.Description,
                    Category = data.Category,
                    Location = data.Location,
                    OriginalPrice = data.OriginalPrice,
                    Price = data.Price,
                    Rate = data.Rate,
                    ExpirationDate = data.ExpirationDate,
                    Approved = data.Approved,
                    Id = data.Id
                };
                return JsonConvert.SerializeObject(cupon, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String FindCuponByLocation(double latitude, double longtitude)
        {
            List<Cupon> cuponsList = new List<Cupon>() ;
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.Where(x => x.Location.Latitude == latitude & x.Location.Longtitude == longtitude).ToList();
                if (data.Count == 0)
                    return "";
                cuponsList.AddRange(data.Select( cupon => new Cupon
                {
                    Name = cupon.Name,
                    Description = cupon.Description,
                    Category = cupon.Category,
                    Location = cupon.Location,
                    OriginalPrice = cupon.OriginalPrice,
                    Price = cupon.Price,
                    Rate = cupon.Rate,
                    ExpirationDate = cupon.ExpirationDate,
                    Approved = cupon.Approved,
                    Id = cupon.Id
                }));
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String FindCuponByPreference(Category category)
        {
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.Where(x => x.Category == category).ToList();
                if (data.Count == 0)
                    return "";
                cuponsList.AddRange(data.Select(cupon => new Cupon
                {
                    Name = cupon.Name,
                    Description = cupon.Description,
                    Category = cupon.Category,
                    Location = cupon.Location,
                    OriginalPrice = cupon.OriginalPrice,
                    Price = cupon.Price,
                    Rate = cupon.Rate,
                    ExpirationDate = cupon.ExpirationDate,
                    Approved = cupon.Approved,
                    Id = cupon.Id
                }));
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented);
            }
        }
       
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool ApproveCupon(int cuponId)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.First(x => x.Id == cuponId);
                if (data == null)
                    return false;
                data.Approved = true;
                entities.SaveChanges();
                return true;
            }
        }
        
        #endregion --------Cupons---------------

        #region --------Bussiness Cupons---------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddBussinessCupon(string name, string description, double originalPrice, double price, Rate rate, DateTime expirationDate, Category category, bool approved, Location location, int bussinessId)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.Where(x => x.Name == name).ToList();
                if (data.Count != 0)
                    return false;
                var bussiness = entities.Bussinesses.First(x => x.Id == bussinessId);
                if (bussiness == null) return false;
                BussinessCupon cupon = new BussinessCupon()
                {
                    Name = name,
                    Description = description,
                    Category = category,
                    Location = location,
                    OriginalPrice = originalPrice,
                    Price = price,
                    Rate = rate,
                    ExpirationDate = expirationDate,
                    Approved = approved,
                    Bussiness = bussiness
                };
                entities.Cupons.Add(cupon);
                entities.SaveChanges();
                return true;

            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String FindCuponByBussiness(double latitude, double longtitude)
        {
            List<BussinessCupon> cuponsList = new List<BussinessCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                //var data = entities.Cupons.Where(x => x. == latitude & x.Location.Longtitude == longtitude).ToList();
                //if (data.Count == 0)
                //    return "";
                //cuponsList.AddRange(data.Select(cupon => new Cupon
                //{
                //    Name = cupon.Name,
                //    Description = cupon.Description,
                //    Category = cupon.Category,
                //    Location = cupon.Location,
                //    OriginalPrice = cupon.OriginalPrice,
                //    Price = cupon.Price,
                //    Rate = cupon.Rate,
                //    ExpirationDate = cupon.ExpirationDate,
                //    Approved = cupon.Approved,
                //    Id = cupon.Id
                //}));
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented);
            }
        }
        #endregion --------Bussiness Cupons---------------

        #region ---------Purchased Cupons --------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool PurchaseCupon(int cuponId, int userId, string serialkey, CuponState state, Rate rate)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.First(x => x.Id == userId);
                if (user == null) return false;
                var cupon = entities.Cupons.First(x => x.Id == cuponId);
                if (cupon == null) return false;
                PurchasedCupon purchasedCupon = new PurchasedCupon
                {
                    SerialKey = serialkey,
                    State = state,
                    Rate =rate,
                    BasicUser = (BasicUser)user,
                    BussinessCupon = (BussinessCupon)cupon
                };
                entities.PurchasedCupons.Add(purchasedCupon);
                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindPurchasedCuponBySerialKey(string serialKey)
        {
            PurchasedCupon purchasedCupon;
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.First(x => x.SerialKey == serialKey);
                if (data == null)
                    return "";
                purchasedCupon = new PurchasedCupon
                {
                    Id = data.Id,
                    SerialKey = data.SerialKey,
                    Rate = data.Rate,
                    State = data.State,
                    BasicUser = data.BasicUser,
                    BussinessCupon = data.BussinessCupon
                };
                return JsonConvert.SerializeObject(purchasedCupon, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool UseCupon(int purchasedCuponId)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.First(x => x.Id == purchasedCuponId);
                if (data == null) return false;

                data.State = CuponState.Used;

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool RateCupon(int purchasedCuponId, Rate rate)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.First(x => x.Id == purchasedCuponId);
                if (data == null) return false;

                data.Rate = rate;

                entities.SaveChanges();
                return true;
            }
        }

        #endregion ---------Purchased Cupons --------

        #region --------Social Network Cupons---------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddSocialNetworkCupon(string name, string description, double originalPrice, double price, Rate rate, DateTime expirationDate, Category category, bool approved, Location location, string url, string phoneNumber, int userId)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.Where(x => x.Name == name).ToList();
                if (data.Count != 0)
                    return false;
                var user = entities.Users.First(x => x.Id == userId);
                if (user == null) return false;
                SocialNetworkCupon SNCupon = new SocialNetworkCupon()
                {
                    Name = name,
                    Description = description,
                    Category = category,
                    Location = location,
                    OriginalPrice = originalPrice,
                    Price = price,
                    Rate = rate,
                    ExpirationDate = expirationDate,
                    Approved = approved,
                    PhoneNumber = phoneNumber,
                    URL = url,
                    User = user
                };
                entities.Cupons.Add(SNCupon);
                entities.SaveChanges();
                return true;
            }
        }

        #endregion --------Social Network Cupons---------------
    }
}
