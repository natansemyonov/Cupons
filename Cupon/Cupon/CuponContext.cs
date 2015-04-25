using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Cupon
{
    public class CuponContext : DbContext
    {
        enum Category
        {
            restaurants,
            shopping,
            healthCare,
            officeAndElectronics,
            vacations,
            pleasure
        }
        public CuponContext():base()
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<BasicUser> Basic_Users { get; set; }
        public DbSet<BussinessOwner> Bussiness_Owners { get; set; }
        public SystemAdmin Admin { get; set; }
        public DbSet<Bussiness> Bussiness { get; set; }
        public DbSet<BussinessCupon> Bussiness_Cupons { get; set; }
        public DbSet<CuponHistory> Cupon_Historys { get; set; }
        public DbSet<Cupon> Cupons { get; set; }
        public DbSet<PurchasedCupon> Purchased_Cupons { get; set; }
        public DbSet<SocialNetworkCupon> Social_Network_Cupons { get; set; }
        public DbSet<Preference> Preferences { get; set; }

    }
}
