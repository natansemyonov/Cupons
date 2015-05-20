using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

namespace CuponWebSite.Controller
{
    /// <summary>
    /// Summary description for BussinessServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class BussinessServices : System.Web.Services.WebService
    {

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool AddBussiness(string name, string description, string category, string latitude, string longtitude, string bussinessOwnerId)
        {
            Location p_Location = new Location();
            p_Location.Latitude = double.Parse(latitude);
            p_Location.Longtitude = double.Parse(longtitude);
            Category p_Category = (Category)int.Parse(category);
            int p_bussinessOwnerId = int.Parse(bussinessOwnerId);

            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.Where(x => x.Name == name).ToList();
                if (data.Count != 0)
                    return false;
                var bussinessOwner = entities.Users.OfType<BussinessOwner>().First(x => x.Id == p_bussinessOwnerId);
                if (bussinessOwner == null)
                    return false;
                Bussiness bussiness = new Bussiness
                {
                    Name = name,
                    Description = description,
                    Category = p_Category,
                    Location = p_Location,
                    BussinessOwner = (BussinessOwner)bussinessOwner
                };
                ((BussinessOwner)bussinessOwner).Bussinesses.Add(bussiness);
                entities.Bussinesses.Add(bussiness);
                entities.SaveChanges();
                return true;
            }
        }
        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool DeleteBussiness(string bussinessId)
        {
            int p_bussinessId = int.Parse(bussinessId);
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.First(x => x.Id == p_bussinessId);
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
        public bool UpdateBussiness(string bussinessId, string name, string description, string category, string latitude, string longtitude)
        {
            Location p_Location = new Location();
            p_Location.Latitude = double.Parse(latitude);
            p_Location.Longtitude = double.Parse(longtitude);
            Category p_Category = (Category)int.Parse(category);
            int p_bussinessId = int.Parse(bussinessId);

            using (ModelContainer entities = new ModelContainer())
            {
                var bussiness = entities.Bussinesses.First(x => x.Id == p_bussinessId);
                if (bussiness == null)
                    return false;
                bussiness.Name = name;
                bussiness.Description = description;
                bussiness.Category = p_Category;
                bussiness.Location = p_Location;
                bussiness.Id = p_bussinessId;

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
                    BussinessCupons = data.BussinessCupons,
                    BussinessOwner = data.BussinessOwner
                };
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(bussiness, Formatting.Indented, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public String FindBussinessByOwner(string ownerID)
        {
            Cupon cupon;
            int p_OwnerID = int.Parse(ownerID);

            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.Where(x => x.BussinessOwner.Id == p_OwnerID);
                if (!data.Any())
                    return "";
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            }
        }
    }
}
