using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuponDataBase
{
    public class BussinessCupon :Cupon
    {

       // public Bussiness Bussiness { get; set; }
        public virtual List<PurchasedCupon> PurchasedCupons { get; set; } 
    }
}
