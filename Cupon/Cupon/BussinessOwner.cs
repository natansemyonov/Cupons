using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cupon
{
    public class BussinessOwner:User
    {
        public virtual List<Bussiness> Business { get; set; }
    }
}
