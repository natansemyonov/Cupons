using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

namespace CuponWebSite.Controller
{
    /// <summary>
    /// Summary description for UserServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //   [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class UserServices : System.Web.Services.WebService
    {
        #region Authentications
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string AuthenticateUser(string userName, string password, double latitude, double longitude)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                BasicUser data = entities.Users.OfType<BasicUser>().First(x => x.UserName == userName && x.Password == password);
                if (data != null)
                {
                    data.Location.Latitude = latitude;
                    data.Location.Longtitude = longitude;
                    entities.SaveChanges();
                    Logger.GetInstance.Log(LogType.Trace, "User " + data.Id + " Authenticated");
                    return data.Id.ToString();
                }
                Logger.GetInstance.Log(LogType.Trace, "User authentication failed for " + userName + " with " + password);
                return JsonConvert.SerializeObject(false, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string AuthenticateOwner(string userName, string password)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                User data = entities.Users.OfType<BussinessOwner>().First(x => x.UserName == userName && x.Password == password);
                if (data != null)
                {
                    Logger.GetInstance.Log(LogType.Trace, "Owner " + data.Id + " Authenticated");
                    return data.Id.ToString();
                }
                Logger.GetInstance.Log(LogType.Trace,
                    "Owner authentication for " + userName + " with " + password + " failed");
                return JsonConvert.SerializeObject(false, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string AuthenticateSystemAdmin(string userName, string password)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                User data = entities.Users.OfType<SystemAdmin>().First(x => x.UserName == userName && x.Password == password);
                if (data != null)
                {
                    Logger.GetInstance.Log(LogType.Trace, "Administrator " + data.Id + " Authenticated");
                    return data.Id.ToString();
                }
                Logger.GetInstance.Log(LogType.Trace,
                    "Administrator authentication for " + userName + " with " + password + " failed");
                return JsonConvert.SerializeObject(false, Formatting.Indented);
            }
        }
        #endregion

        #region Registraions
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string BussinessOwnerRegister(string userName, string password, string email, string photo)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Users.OfType<BussinessOwner>().Where(x => x.UserName == userName).ToList();
                if (data.Count != 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                try
                {
                    BussinessOwner user = new BussinessOwner
                    {
                        UserName = userName,
                        Password = password,
                        Email = email,
                        Photo = photo
                    };
                    entities.Users.Add(user);
                    entities.SaveChanges();
                    Logger.GetInstance.Log(LogType.Trace, "User " + user.Id + " successfuly registered");
                    return user.Id.ToString();
                }
                catch (Exception ex)
                {
                    Logger.GetInstance.Log(LogType.Trace, "User registration failed due to: " + ex.Message);
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                }
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string BasicUserRegister(string userName, string password, string email, string gender,
            string phoneNumber, string birthDate, string longitude, string latitude, string photo)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                Location location = new Location
                {
                    Latitude = double.Parse(latitude),
                    Longtitude = double.Parse(longitude)
                };
                var data = entities.Users.OfType<BasicUser>().Where(x => x.UserName == userName).ToList();
                if (data.Count != 0)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                BasicUser basicUser = new BasicUser
                {
                    UserName = userName,
                    Password = password,
                    Email = email,
                    Gender = (Gender)int.Parse(gender),
                    PhoneNumber = phoneNumber,
                    BirthDate = DateTime.Parse(birthDate),
                    Location = location,
                    Photo = photo
                };
                entities.Users.Add(basicUser);
                entities.SaveChanges();
                return basicUser.Id.ToString();
            }
        }
        #endregion
       

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool UpdateBasicUser(string userId, string userName, string password, string email, string phoneNumber,
            string latitude, string longtitude, string photo)
        {
            Location p_Location = new Location();
            p_Location.Latitude = double.Parse(latitude);
            p_Location.Longtitude = double.Parse(longtitude);
            int P_UserID = int.Parse(userId);

            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.OfType<BasicUser>().First(x => x.Id == P_UserID);
                if (user == null)
                    return false;
                user.UserName = userName;
                user.Password = password;
                user.Email = email;
                user.Photo = photo;
                user.PhoneNumber = phoneNumber;
                user.Location = p_Location;

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public string UpdateAlertMode(int id, int mode)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                BasicUser data = entities.Users.OfType<BasicUser>().First(x => x.Id == id);
                if (data != null)
                {
                    data.Alerts = (RecommendationType) mode;
                    entities.SaveChanges();
                    return data.Id.ToString();
                }
                return JsonConvert.SerializeObject(false, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindBasicUserByName_Email(string userName, string email)
        {
            BasicUser bUser;
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.OfType<BasicUser>().First(x => x.UserName == userName & x.Email == email);
                if (user == null)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                bUser = new BasicUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Photo = user.Photo,
                    PhoneNumber = user.PhoneNumber,
                    Location = user.Location,
                    Password = user.Password,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    Preferences = user.Preferences,
                    PurchasedCupons = user.PurchasedCupons,
                    SocialNetworkCupons = user.SocialNetworkCupons,
                    Alerts = user.Alerts
                };
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(bUser, Formatting.Indented, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindBasicUserByID(string id)
        {
            int i = int.Parse(id);
            BasicUser bUser;
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.OfType<BasicUser>().First(x => x.Id == i);
                if (user == null)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                bUser = new BasicUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Photo = user.Photo,
                    PhoneNumber = user.PhoneNumber,
                    Location = user.Location,
                    Password = user.Password,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    Preferences = user.Preferences,
                    PurchasedCupons = user.PurchasedCupons,
                    SocialNetworkCupons = user.SocialNetworkCupons,
                    Alerts = user.Alerts
                };
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(bUser, Formatting.Indented,settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindBussinessOwnerByID(string id)
        {
            int i = int.Parse(id);
            BasicUser bUser;
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.OfType<BussinessOwner>().First(x => x.Id == i);
                if (user == null)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                bUser = new BasicUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Photo = user.Photo,
                };
                return JsonConvert.SerializeObject(bUser, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool ChangePassword(string userId, string newPassword)
        {
            int p_UserID = int.Parse(userId);
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.First(x => x.Id == p_UserID);
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
        public string GenerateNewPassword(string userId)
        {
            int p_UserID = int.Parse(userId);
            using (ModelContainer entities = new ModelContainer())
            {
                int length = 8;
                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                StringBuilder res = new StringBuilder();
                Random rnd = new Random();
                while (0 < length--)
                {
                    res.Append(valid[rnd.Next(valid.Length)]);
                }
                var user = entities.Users.First(x => x.Id == p_UserID);
                if (user == null)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                user.Password = res.ToString();
                entities.SaveChanges();
                return res.ToString();
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddPreference(string category, string userId)
        {
            int p_UserID = int.Parse(userId);
            Category p_Category = (Category)int.Parse(category);
            using (ModelContainer entities = new ModelContainer())
            {
                var user = entities.Users.OfType<BasicUser>().First(x => x.Id == p_UserID);
                if (user == null)
                    return false;
                var data = entities.Preferences.Where(x => x.Category == p_Category & x.BasicUser.Id == p_UserID).ToList();
                if (data.Count != 0)
                {
                    entities.Preferences.Remove(data[0]);
                    entities.SaveChanges();
                    return true;
                }
                Preference preference = new Preference
                {
                    Category = p_Category,
                    BasicUser = user
                };
                user.Preferences.Add(preference);
                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public string GetLoggingHistory(string adminId)
        {
            int p_UserID = int.Parse(adminId);
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Logs.Where(x=>x.Type == LogType.Info).ToList();
                return JsonConvert.SerializeObject(data, Formatting.Indented);
            }
        }
    }
}
