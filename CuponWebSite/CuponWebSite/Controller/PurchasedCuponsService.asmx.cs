using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activities;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

namespace CuponWebSite.Controller
{
    /// <summary>
    /// Summary description for PurchasedCuponsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PurchasedCuponsService : System.Web.Services.WebService
    {
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
        public String CuponHistory(string basicUserId)
        {
            int p_BasicUserId = int.Parse(basicUserId);
            List<PurchasedCupon> purchasedCuponsList = new List<PurchasedCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.Where(x => x.BasicUser.Id == p_BasicUserId).ToList();
                if (data.Count == 0)
                    return "false";
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
        public String BussinessCuponHistory(string bussinessId)
        {
            int p_BussinessId = int.Parse(bussinessId);
            List<PurchasedCupon> purchasedCuponsList = new List<PurchasedCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.PurchasedCupons.Where(x => x.BussinessCupon.Bussiness.Id == p_BussinessId).ToList();
                if (data.Count == 0)
                    return "false";
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
    }
}
