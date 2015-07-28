using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CuponWebSite.Controller
{
    public class PreferencesRecommender : IRecommendation
    {
        public string RecommendCupons(Object o)
        {
            BasicUser user = o as BasicUser;
            if (user != null)
            {
                List<BussinessCupon> cupons = new List<BussinessCupon>();
                using (ModelContainer entities = new ModelContainer())
                {
                    cupons = user.Preferences.Select(preference => entities.Cupons.OfType<BussinessCupon>()
                        .Where(x => x.Category == preference.Category && x.Approved && x.ExpirationDate >= DateTime.Now).ToList())
                        .Aggregate(cupons, (current, data) => current.Union(data).ToList());
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    return JsonConvert.SerializeObject(cupons, Formatting.Indented, settings);
                }
                
            }
            return "";
        }
    }
}