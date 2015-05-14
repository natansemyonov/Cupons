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
            List<Cupon> cuponsList = new List<Cupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Cupons.Where(x => x.Location.Latitude == latitude & x.Location.Longtitude == longtitude).ToList();
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
        public string FindCuponByPreference(string c)
        {
            Category category = (Category)int.Parse(c);
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
                return JsonConvert.SerializeObject(cuponsList.GetRange(0, cuponsList.Count < 100 ? cuponsList.Count : 100), Formatting.Indented);
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

        #region ---------Purchased Cupons --------
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string PurchaseCupon(int cuponId, int userId)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                Random rnd = new Random();
                string serial = "" + rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9) + "-" +
                                rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9) + "-" + rnd.Next(0, 9) +
                                rnd.Next(0, 9) + rnd.Next(0, 9) + rnd.Next(0, 9);
                User user = entities.Users.First(x => x.Id == userId);
                if (user == null) return JsonConvert.SerializeObject(false, Formatting.Indented);
                user = entities.Users.OfType<BasicUser>().First(x => x.Id == userId);
                Cupon cupon = entities.Cupons.First(x => x.Id == cuponId);
                if (cupon == null) return JsonConvert.SerializeObject(false, Formatting.Indented);
                PurchasedCupon purchasedCupon = new PurchasedCupon
                {
                    SerialKey = serial,
                    State = CuponState.Pending,
                    Rate = Rate.NA,
                    BasicUser = (BasicUser)user,
                    BussinessCupon = (BussinessCupon)cupon
                };
                entities.PurchasedCupons.Add(purchasedCupon);
                entities.SaveChanges();
                //SendEmail(user.Email,serial);
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


        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String CuponHistory(int basicUserId)
        {
            List<PurchasedCupon> purchasedCuponsList = new List<PurchasedCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.Where(x => x.BasicUser.Id == basicUserId).ToList();
                if (data.Count == 0)
                    return "";
                purchasedCuponsList.AddRange(data.Select(purchasedCupon => new PurchasedCupon
                {
                    Id = purchasedCupon.Id,
                    SerialKey = purchasedCupon.SerialKey,
                    State = purchasedCupon.State,
                    Rate = purchasedCupon.Rate,
                    BasicUser = purchasedCupon.BasicUser,
                    BussinessCupon = purchasedCupon.BussinessCupon
                }));
                return JsonConvert.SerializeObject(purchasedCuponsList, Formatting.Indented);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String BussinessCuponHistory(int bussinessId)
        {
            List<PurchasedCupon> purchasedCuponsList = new List<PurchasedCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.Where(x => x.BussinessCupon.Bussiness.Id == bussinessId).ToList();
                if (data.Count == 0)
                    return "";
                purchasedCuponsList.AddRange(data.Select(purchasedCupon => new PurchasedCupon
                {
                    Id = purchasedCupon.Id,
                    SerialKey = purchasedCupon.SerialKey,
                    State = purchasedCupon.State,
                    Rate = purchasedCupon.Rate,
                    BasicUser = purchasedCupon.BasicUser,
                    BussinessCupon = purchasedCupon.BussinessCupon
                }));
                return JsonConvert.SerializeObject(purchasedCuponsList, Formatting.Indented);
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
