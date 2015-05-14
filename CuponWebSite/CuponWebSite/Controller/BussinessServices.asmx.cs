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
    // [System.Web.Script.Services.ScriptService]
    public class BussinessServices : System.Web.Services.WebService
    {

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
                    BussinessCupons = data.BussinessCupons,
                    BussinessOwner = data.BussinessOwner
                };
                return JsonConvert.SerializeObject(bussiness, Formatting.Indented);
            }
        }
    }
}
