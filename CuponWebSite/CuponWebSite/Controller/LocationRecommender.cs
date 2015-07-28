using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CuponWebSite.Controller
{
    public class LocationRecommender : IRecommendation
    {
        public string RecommendCupons(Object o)
        {
            Location loc = o as Location;
            List<BussinessCupon> cupons = new List<BussinessCupon>();
            using (ModelContainer entities = new ModelContainer())
            {
                List<BussinessCupon> data = entities.Cupons.OfType<BussinessCupon>().ToList();
                List<BussinessCupon> result = data.Where(cupon => distance(loc.Latitude, loc.Longtitude, cupon.Location.Latitude, cupon.Location.Longtitude) > 10 && cupon.Approved && cupon.ExpirationDate>=DateTime.Now).Take(100).ToList();
                cupons = result;cupons.Union(data).ToList();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                return JsonConvert.SerializeObject(cupons, Formatting.Indented, settings);
            }
            
        }

        private double distance(double lat1, double lon1, double lat2, double lon2)
        {

            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;
            return (dist);
        }

        // This function converts decimal degrees to radians
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        // This function converts radians to decimal degrees
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

    }
}