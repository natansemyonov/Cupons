using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CuponWebSite.Controller
{
    /// <summary>
    /// Summary description for CuponServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CuponServices : System.Web.Services.WebService
    {
        #region -------- Cupons---------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool DeleteCupon(string cuponId)
        {
            int p_CuponID = int.Parse(cuponId);
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.First(x => x.Id == p_CuponID);
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
        public bool UpdateCupon(string cuponId, string name, string description, string originalPrice, string price, string rate,
            string expirationDate, string category, string approved, string latitude, string longtitude, string photo)
        {
            int p_CuponID = int.Parse(cuponId);
            double p_OriginalPrice = double.Parse(originalPrice);
            double p_Price = double.Parse(price);
            Rate p_Rate = (Rate)int.Parse(rate);
            DateTime p_ExpirationDate = DateTime.Parse(expirationDate);
            Category p_Category = (Category)int.Parse(category);
            bool p_Approved = bool.Parse(approved);
            Location p_Location = new Location();
            p_Location.Latitude = double.Parse(latitude);
            p_Location.Longtitude = double.Parse(longtitude);
            using (ModelContainer entities = new ModelContainer())
            {
                var cupon = entities.Cupons.OfType<BussinessCupon>().First(x => x.Id == p_CuponID);
                if (cupon == null)
                    return false;
                cupon.Name = name;
                cupon.Description = description;
                cupon.Category = p_Category;
                cupon.Location = p_Location;
                cupon.OriginalPrice = p_OriginalPrice;
                cupon.Price = p_Price;
                cupon.Rate = p_Rate;
                cupon.ExpirationDate = p_ExpirationDate;
                cupon.Approved = p_Approved;
                cupon.Photo = photo;

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public String GetAllBussinessCupons()
        {
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.OfType<BussinessCupon>().Where(x=>x.ExpirationDate>=DateTime.Now).ToList();
                if (data.Count == 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                cuponsList.AddRange(data.Select(cupon => new BussinessCupon
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
                    Photo = cupon.Photo,
                    Id = cupon.Id,
                    PurchasedCupons = cupon.PurchasedCupons,
                    Bussiness = cupon.Bussiness
                }).Where(x => x.Approved).Reverse().Take(100));
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public String GetRecommendation(int type, string data)
        {
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                IRecommendation recommender = new LocationRecommender();
                Object o = new object();
                switch ((RecommendationType)type)
                {
                    case RecommendationType.Off:
                        return "";
                    case RecommendationType.Preference:
                        int userId = int.Parse(data);
                        recommender = new PreferencesRecommender();
                        var user = entities.Users.OfType<BasicUser>().FirstOrDefault(x => x.Id == userId);
                        if (user == null)
                            return JsonConvert.SerializeObject(false, Formatting.Indented);
                        o = user;
                        break;
                    case RecommendationType.Location:
                        Location loc = JsonConvert.DeserializeObject<Location>(data);
                        recommender = new LocationRecommender();
                        if (loc == null)
                            return JsonConvert.SerializeObject(false, Formatting.Indented);
                        o = loc;
                        break;
                    case RecommendationType.Combined:
                        JObject j = JsonConvert.DeserializeObject(data) as JObject;
                        userId = int.Parse(j.GetValue("id").ToString());
                        recommender = new PreferencesRecommender();
                        user = entities.Users.OfType<BasicUser>().FirstOrDefault(x => x.Id == userId);
                        if (user == null)
                            return JsonConvert.SerializeObject(false, Formatting.Indented);
                        o = user;
                        List<BussinessCupon> tmp =
                            JsonConvert.DeserializeObject<List<BussinessCupon>>(recommender.RecommendCupons(o));
                        loc = JsonConvert.DeserializeObject<Location>(j.GetValue("loc").ToString());
                        recommender = new LocationRecommender();
                        if (loc == null)
                            return JsonConvert.SerializeObject(false, Formatting.Indented);
                        o = loc;
                        tmp = tmp.Union(JsonConvert.DeserializeObject<List<BussinessCupon>>(recommender.RecommendCupons(o)), new BussinessCuponComparer()).ToList();
                        JsonSerializerSettings settings = new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        };
                        return JsonConvert.SerializeObject(tmp, Formatting.Indented, settings);
                        break;
                }
                //cuponsList.AddRange(recommender.RecommendCupons(o).OfType<BussinessCupon>());
                //JsonSerializerSettings settings = new JsonSerializerSettings
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //};
                return recommender.RecommendCupons(o);
                    //JsonConvert.SerializeObject(cuponsList, Formatting.Indented, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public String GetAllUnAprrovedCupons()
        {
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.OfType<BussinessCupon>().Where(x => x.Approved == false).ToList();
                if (data.Count == 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                cuponsList.AddRange(data.Select(cupon => new BussinessCupon
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
                    Photo = cupon.Photo,
                    Id = cupon.Id
                }).Reverse().Take(100));
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindCuponsById(string cuponId)
        {
            int p_CuponID = int.Parse(cuponId);
            Cupon cupon;
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.OfType<BussinessCupon>().First(x => x.Id == p_CuponID);
                if (data == null)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                cupon = new BussinessCupon
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
                    Photo = data.Photo,
                    Id = data.Id
                };
                return JsonConvert.SerializeObject(cupon, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindCuponsByNameAndDescription(string text)
        {
            Cupon cupon;
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.OfType<BussinessCupon>().First(x => x.Name.Contains(text) & x.Description.Contains(text));
                if (data == null)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                cupon = new BussinessCupon
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
                    Photo = data.Photo,
                    Id = data.Id
                };
                return JsonConvert.SerializeObject(cupon, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindCuponsByLocation(string latitude, string longtitude, string distance)
        {
            Location p_Location = new Location();
            p_Location.Latitude = double.Parse(latitude);
            p_Location.Longtitude = double.Parse(longtitude);
            double p_Distance = double.Parse(distance);
            List<Cupon> cuponsList = new List<Cupon>();

            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.OfType<BussinessCupon>().Where(x => Math.Pow(Math.Pow(x.Location.Latitude - p_Location.Latitude, 2) + Math.Pow(x.Location.Longtitude - p_Location.Longtitude, 2), 0.5) < p_Distance).ToList();
                if (data.Count == 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                cuponsList.AddRange(data.Select(cupon => new BussinessCupon
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
                    Photo = cupon.Photo,
                    Id = cupon.Id
                }));
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindCuponByPreference(string ctegory)
        {
            Category p_Category = (Category)int.Parse(ctegory);
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.OfType<BussinessCupon>().Where(x => x.Category == p_Category).ToList();
                if (data.Count == 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                cuponsList.AddRange(data.Select(cupon => new BussinessCupon
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
                    Photo = cupon.Photo,
                    Id = cupon.Id
                }));
                return JsonConvert.SerializeObject(cuponsList.GetRange(0, cuponsList.Count < 100 ? cuponsList.Count : 100), Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool ApproveCupon(string cuponId, string adminId)
        {
            int p_CuponID = int.Parse(cuponId);
            int p_AdminId = int.Parse(adminId);

            using (ModelContainer entities = new ModelContainer())
            {
                var admin = entities.Users.OfType<SystemAdmin>().First(x => x.Id == p_AdminId);
                if (admin == null)
                    return false;
                var data = entities.Cupons.OfType<BussinessCupon>().First(x => x.Id == p_CuponID);
                if (data == null)
                    return false;
                data.Approved = true;
                entities.SaveChanges();
                Logger.GetInstance.Log(LogType.Info,admin.UserName + " has approved cupon " + data.Name);
                return true;
            }
        }
        #endregion -------- Cupons---------------

        #region --------Bussiness Cupons  -------------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddBussinessCupon(string name, string description, string originalPrice,
            string price, string expirationDate, string category,
            string longitude, string latitude, string photo, string bussinessId)
        {
            double p_OriginalPrice = double.Parse(originalPrice);
            double p_Price = double.Parse(price);
            Category p_Category = (Category)int.Parse(category);
            int p_BussinessID = int.Parse(bussinessId);

            using (ModelContainer entities = new ModelContainer())
            {
                Location location = new Location
                {
                    Latitude = double.Parse(latitude),
                    Longtitude = double.Parse(longitude)
                };
                DateTime date = DateTime.Parse(expirationDate);
                var data = entities.Cupons.Where(x => x.Name == name).ToList();
                if (data.Count != 0)
                    return false;
                var bussiness = entities.Bussinesses.First(x => x.Id == p_BussinessID);
                if (bussiness == null) return false;
                BussinessCupon cupon = new BussinessCupon()
                {
                    Name = name,
                    Description = description,
                    Category = p_Category,
                    Location = location,
                    OriginalPrice = p_OriginalPrice,
                    Price = p_Price,
                    Rate = Rate.NA,
                    ExpirationDate = date,
                    Approved = false,
                    Photo = photo,
                    Bussiness = bussiness
                };
                entities.Cupons.Add(cupon);
                entities.SaveChanges();
                Logger.GetInstance.Log(LogType.Info, bussiness.Name + " added new cupon");
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String FindCuponByBussiness(string bussinessId)
        {
            int p_BussinessID = int.Parse(bussinessId);
            List<BussinessCupon> cuponsList = new List<BussinessCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.OfType<BussinessCupon>().Where(x => x.Bussiness.Id == p_BussinessID).ToList();
                if (data.Count == 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                cuponsList.AddRange(data.Select(cupon => new BussinessCupon
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
                    Photo = cupon.Photo,
                    Id = cupon.Id,
                    Bussiness = cupon.Bussiness,
                    PurchasedCupons = cupon.PurchasedCupons
                }));
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented, settings);
            }
        }
        #endregion --------Bussiness Cupons---------------

        #region --------Social Network Cupons---------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddSocialNetworkCupon(string url, string userId)
        {
            int p_UserID = int.Parse(userId);
            using (ModelContainer entities = new ModelContainer())
            {
                try
                {
                    var data = entities.Cupons.OfType<SocialNetworkCupon>().Where(x => x.URL == url).ToList();
                    if (data.Count != 0)
                        return false;
                    var user = entities.Users.First(x => x.Id == p_UserID);
                    if (user == null) return false;
                    SocialNetworkCupon SNCupon = new SocialNetworkCupon()
                    {
                        Name = user.UserName,
                        URL = url,
                        User = user
                    };
                    user.SocialNetworkCupons.Add(SNCupon);
                    entities.Cupons.Add(SNCupon);
                    entities.SaveChanges();
                    Logger.GetInstance.Log(LogType.Info, "User " + userId + " added new social cupon");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.GetInstance.Log(LogType.Trace, "Adding social cupon failed due to: " + ex.Message);
                    return false;
                }
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool CreateSocialNetworkCupon(string name, string url, int userId)
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
                    URL = url,
                    User = user
                };
                user.SocialNetworkCupons.Add(SNCupon);
                entities.Cupons.Add(SNCupon);
                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String GetAllUnAprrovedSocialCupons()
        {
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.OfType<SocialNetworkCupon>().Where(x => x.Approved == false).ToList();
                if (data.Count == 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                cuponsList.AddRange(data.Select(cupon => new SocialNetworkCupon()
                {
                    Name = cupon.Name,
                    URL = cupon.URL,
                    Approved = cupon.Approved,
                    Id = cupon.Id
                }).Reverse().Take(100));
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented);
            }
        }
        #endregion --------Social Network Cupons---------------

        #region-------------- Purchased Cupons------------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string PurchaseCupon(string cuponId, string userId)
        {
            int p_CuponId = int.Parse(cuponId);
            int p_UserId = int.Parse(userId);
            using (ModelContainer entities = new ModelContainer())
            {
                Random rnd = new Random();
                string serial = "" + rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9) + "-" +
                                rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9) + "-" + rnd.Next(0, 9) +
                                rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9);
                BasicUser user = entities.Users.OfType<BasicUser>().First(x => x.Id == p_UserId);
                if (user == null) return JsonConvert.SerializeObject(false, Formatting.Indented);
                BussinessCupon cupon = entities.Cupons.OfType<BussinessCupon>().First(x => x.Id == p_CuponId);
                if (cupon == null) return JsonConvert.SerializeObject(false, Formatting.Indented);
                PurchasedCupon purchasedCupon = new PurchasedCupon
                {
                    SerialKey = serial,
                    State = CuponState.Pending,
                    Rate = Rate.NA,
                    BasicUser = user,
                    BussinessCupon = cupon
                };
                entities.PurchasedCupons.Add(purchasedCupon);
                entities.SaveChanges();
                //SendEmail(user.Email,serial);
                Logger.GetInstance.Log(LogType.Info, user.UserName + " has purchased the cupon " + cupon.Name);
                return JsonConvert.SerializeObject(serial, Formatting.Indented);
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
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                purchasedCupon = new PurchasedCupon
                {
                    Id = data.Id,
                    SerialKey = data.SerialKey,
                    Rate = data.Rate,
                    State = data.State,
                    BasicUser = data.BasicUser,
                    BussinessCupon = data.BussinessCupon
                };
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(purchasedCupon, Formatting.Indented, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool UseCupon(string CuponId, string serialkey)
        {
            int p_CuponId = int.Parse(CuponId);
            using (ModelContainer entities = new ModelContainer())
            {
                BussinessCupon bussinessCupon = entities.Cupons.OfType<BussinessCupon>().First(x => x.Id == p_CuponId);
                if (bussinessCupon == null) return false;
                PurchasedCupon purchasedCupon = bussinessCupon.PurchasedCupons.First(x => x.SerialKey == serialkey);
                if (purchasedCupon == null) return false;

                purchasedCupon.State = CuponState.Used;

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool RateCupon(string purchasedCuponId, string rate)
        {
            int p_PurchasedCuponId = int.Parse(purchasedCuponId);
            Rate p_Rate = (Rate)int.Parse(rate);

            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.First(x => x.Id == p_PurchasedCuponId);
                if (data == null) return false;

                data.Rate = p_Rate;

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string CuponHistory(string basicUserId)
        {
            int p_BasicUserId = int.Parse(basicUserId);
            List<PurchasedCupon> purchasedCuponsList = new List<PurchasedCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.Where(x => x.BasicUser.Id == p_BasicUserId).ToList();
                if (data.Count == 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                purchasedCuponsList.AddRange(data.Select(purchasedCupon => new PurchasedCupon
                {
                    Id = purchasedCupon.Id,
                    SerialKey = purchasedCupon.SerialKey,
                    State = purchasedCupon.State,
                    Rate = purchasedCupon.Rate,
                    BasicUser = purchasedCupon.BasicUser,
                    BussinessCupon = purchasedCupon.BussinessCupon
                }));
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(purchasedCuponsList, Formatting.Indented, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string BussinessCuponHistory(string bussinessId)
        {
            int p_BussinessId = int.Parse(bussinessId);
            List<PurchasedCupon> purchasedCuponsList = new List<PurchasedCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.Where(x => x.BussinessCupon.Bussiness.Id == p_BussinessId).ToList();
                if (data.Count == 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                purchasedCuponsList.AddRange(data.Select(purchasedCupon => new PurchasedCupon
                {
                    Id = purchasedCupon.Id,
                    SerialKey = purchasedCupon.SerialKey,
                    State = purchasedCupon.State,
                    Rate = purchasedCupon.Rate,
                    BasicUser = purchasedCupon.BasicUser,
                    BussinessCupon = purchasedCupon.BussinessCupon
                }));
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(purchasedCuponsList, Formatting.Indented, settings);
            }
        }
        #endregion ------------------Purchased Cupons------------

        private void SendEmail(string recepientEmail, string body)
        {
            //Set Mail Definiotions
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            };
            NetworkCredential networkCred = new NetworkCredential
            {
                UserName = "cupon@gmail.com",//TODO
                Password = "cuponS"//TODO
            };
            smtp.Credentials = networkCred;

            //Set Mail For User
            MailMessage userMailMessage = new MailMessage
            {
                From = new MailAddress("cupon@gmail.com"),//TODO
                Subject = "CupoNoa",//TODO
                Body = body,
                IsBodyHtml = true
            };
            userMailMessage.To.Add(new MailAddress(recepientEmail));

            smtp.Send(userMailMessage);

        }
    }

    class BussinessCuponComparer : IEqualityComparer<BussinessCupon>
    {
        public bool Equals(BussinessCupon x, BussinessCupon y)
        {
            return x.Id == y.Id;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(BussinessCupon cupon)
        {
            return cupon.GetHashCode();
        }
    }
}
