using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web.Services;
using Newtonsoft.Json;

namespace CuponWebSite.Controller
{
    /// <summary>
    /// Summary description for CuponSystemWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
 //   [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class CuponSystemWebService : System.Web.Services.WebService
    {
        public CuponSystemWebService()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent();
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

        [WebMethod]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
        public string about()
        {
            bool res = true;
            string d = "{\"a\":\"a\"}";
            return d;
        }

    }
}
