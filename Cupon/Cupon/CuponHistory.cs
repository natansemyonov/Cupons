using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cupon
{
    public class CuponHistory
    {
        public int CuponHistoryId { get; set; }
       // public BasicUser Basic_User { get; set; }
        public virtual List<PurchasedCupon> Purchased_Cupons { get; set; }
    }
}
