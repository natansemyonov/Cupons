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
    
    public partial class BasicUser : User
    {
        public BasicUser()
        {
            this.Preferences = new HashSet<Preference>();
            this.PurchasedCupons = new HashSet<PurchasedCupon>();
            this.Location = new Location();
        }
    
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime BirthDate { get; set; }
        public RecommendationType Alerts { get; set; }
    
        public Location Location { get; set; }
    
        public virtual ICollection<Preference> Preferences { get; set; }
        public virtual ICollection<PurchasedCupon> PurchasedCupons { get; set; }
    }
}
