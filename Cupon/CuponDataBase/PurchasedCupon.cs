using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuponDataBase
{
    public class PurchasedCupon
    {
        public int PurchasedCuponId { get; set; }
        public int Serial_Key { get; set; }
        public string Cupon_State { get; set; }
        public double Rate { get; set; }

        public virtual BussinessCupon Bussiness_Cupon { get; set; }
    }
}
