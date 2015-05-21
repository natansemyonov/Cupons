using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

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
        public bool AddCupon(string name, string description, string originalPrice, string price, string rate, string expirationDate, string category, string approved, string latitude, string longtitude)
        {
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
                var data = entities.Cupons.Where(x => x.Name == name).ToList();
                if (data.Count != 0)
                    return false;
                Cupon cupon = new Cupon()
                 {
                     Name = name,
                     Description = description,
                     Category = p_Category,
                     Location = p_Location,
                     OriginalPrice = p_OriginalPrice,
                     Price = p_Price,
                     Rate = p_Rate,
                     ExpirationDate = p_ExpirationDate,
                     Approved = p_Approved
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
        public bool UpdateCupon(string cuponId, string name, string description, string originalPrice, string price, string rate, string expirationDate, string category, string approved, string latitude, string longtitude)
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
                var cupon = entities.Cupons.First(x => x.Id == p_CuponID);
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

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public String GetAllCupons()
        {
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.OfType<BussinessCupon>().ToList();
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
                }).Where(x => x.Approved).Reverse().Take(100));
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented);
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
                var data = entities.Cupons.OfType<BussinessCupon>().Where(x=> x.Approved == false).ToList();
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
                }).Reverse().Take(100));
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String FindCuponById(string cuponId)
        {
            int p_CuponID = int.Parse(cuponId);
            Cupon cupon;
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.First(x => x.Id == p_CuponID);
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
        public String FindCuponByLocation(string latitude, string longtitude)
        {
            Location p_Location = new Location();
            p_Location.Latitude = double.Parse(latitude);
            p_Location.Longtitude = double.Parse(longtitude);
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.Where(x => x.Location.Latitude == p_Location.Latitude & x.Location.Longtitude == p_Location.Longtitude).ToList();
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
        public string FindCuponByPreference(string ctegory)
        {
            Category p_Category = (Category)int.Parse(ctegory);
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.Where(x => x.Category == p_Category).ToList();
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
                return JsonConvert.SerializeObject(cuponsList.GetRange(0, cuponsList.Count < 100 ? cuponsList.Count : 100), Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool ApproveCupon(string cuponId)
        {
            int p_CuponID = int.Parse(cuponId);
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.First(x => x.Id == p_CuponID);
                if (data == null)
                    return false;
                data.Approved = true;
                entities.SaveChanges();
                return true;
            }
        }
        #endregion -------- Cupons---------------

        #region --------Bussiness Cupons---------------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddBussinessCupon(string name, string description, double originalPrice,
            double price, string expirationDate, int category,
            string longitude, string latitude, int bussinessId)
        {
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
                var bussiness = entities.Bussinesses.First(x => x.Id == bussinessId);
                if (bussiness == null) return false;
                BussinessCupon cupon = new BussinessCupon()
                {
                    Name = name,
                    Description = description,
                    Category = (Category)category,
                    Location = location,
                    OriginalPrice = originalPrice,
                    Price = price,
                    Rate = Rate.NA,
                    ExpirationDate = date,
                    Approved = false,
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
        public String FindCuponByBussiness(int bussinessId)
        {
            List<BussinessCupon> cuponsList = new List<BussinessCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.Where(x => ((BussinessCupon)x).Bussiness.Id == bussinessId).ToList();
                if (data.Count == 0)
                    return "";
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
                    Id = cupon.Id,
                    Bussiness = ((BussinessCupon)cupon).Bussiness,
                    PurchasedCupons = ((BussinessCupon)cupon).PurchasedCupons
                }));
                return JsonConvert.SerializeObject(cuponsList, Formatting.Indented);
            }
        }
        #endregion --------Bussiness Cupons---------------

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
                user.SocialNetworkCupons.Add(SNCupon);
                entities.Cupons.Add(SNCupon);
                entities.SaveChanges();
                return true;
            }
        }

        #endregion --------Social Network Cupons---------------

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
}
