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
        public bool AddBussiness(string name, string description, string category, string latitude, string longtitude, string photo, string bussinessOwnerId)
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
                var bussinessOwner = entities.Users.OfType<BussinessOwner>().FirstOrDefault(x => x.Id == p_bussinessOwnerId);
                if (bussinessOwner == null)
                    return false;
                Bussiness bussiness = new Bussiness
                {
                    Name = name,
                    Description = description,
                    Category = p_Category,
                    Location = p_Location,
                    Photo = photo,
                    Approved = false,
                    BussinessOwner = (BussinessOwner)bussinessOwner
                };
                ((BussinessOwner)bussinessOwner).Bussinesses.Add(bussiness);
                entities.Bussinesses.Add(bussiness);
                entities.SaveChanges();
                Logger.GetInstance.Log(LogType.Info, "Bussiness owner " + bussinessOwnerId + " has created the business " + bussiness.Name);
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
                var data = entities.Bussinesses.FirstOrDefault(x => x.Id == p_bussinessId);
                if (data == null)
                    return false;
                entities.Bussinesses.Remove(data);
                entities.SaveChanges();
                Logger.GetInstance.Log(LogType.Info, "Bussiness  " + bussinessId + " has been deleted");
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool ApproveBussiness(string bussinessId)
        {
            int p_bussinessId = int.Parse(bussinessId);
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.FirstOrDefault(x => x.Id == p_bussinessId);
                if (data == null)
                    return false;
                data.Approved = true;
                entities.SaveChanges();
                Logger.GetInstance.Log(LogType.Info, "Bussiness " + bussinessId + " has been approved");
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public bool UpdateBussiness(string bussinessId, string name, string description, string category, string latitude, string longtitude, string photo)
        {
            Location p_Location = new Location();
            p_Location.Latitude = double.Parse(latitude);
            p_Location.Longtitude = double.Parse(longtitude);
            Category p_Category = (Category)int.Parse(category);
            int p_bussinessId = int.Parse(bussinessId);

            using (ModelContainer entities = new ModelContainer())
            {
                var bussiness = entities.Bussinesses.FirstOrDefault(x => x.Id == p_bussinessId);
                if (bussiness == null)
                    return false;
                bussiness.Name = name;
                bussiness.Description = description;
                bussiness.Category = p_Category;
                bussiness.Location = p_Location;
                bussiness.Id = p_bussinessId;
                bussiness.Photo = photo;

                entities.SaveChanges();
                return true;
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindBussinessByName(string bussinessName)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.FirstOrDefault(x => x.Name == bussinessName);
                if (data == null)
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                Bussiness bussiness = new Bussiness
                {
                    Name = data.Name,
                    Description = data.Description,
                    Category = data.Category,
                    Location = data.Location,
                    Photo = data.Photo,
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
        public string FindBussinessOfOwner(string ownerID)
        {
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

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string FindBussinessByCategory(string category)
        {
            Category p_Category = (Category)int.Parse(category);

            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.Where(x => x.Category == p_Category).ToList();
                if (!data.Any())
                    return JsonConvert.SerializeObject(false, Formatting.Indented); 
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            }
        }

        [WebMethod]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        public string GetAllUnapprovedBussiness()
        {
            using (ModelContainer entities = new ModelContainer())
            {
                var data = entities.Bussinesses.Where(x => x.Approved == false).ToList();
                if (!data.Any())
                    return JsonConvert.SerializeObject(false, Formatting.Indented);
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            }
        }
    }
}
