//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CuponWebSite
{
    using System;
    using System.Collections.Generic;
    
    public partial class BussinessCupon : Cupon
    {
        public BussinessCupon()
        {
            this.PurchasedCupons = new HashSet<PurchasedCupon>();
            this.Location = new Location();
        }
    
        public string Description { get; set; }
        public double OriginalPrice { get; set; }
        public double Price { get; set; }
        public Rate Rate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public Category Category { get; set; }
        public bool Approved { get; set; }
        public string Photo { get; set; }
    
        public Location Location { get; set; }
    
        public virtual Bussiness Bussiness { get; set; }
        public virtual ICollection<PurchasedCupon> PurchasedCupons { get; set; }
    }
}
