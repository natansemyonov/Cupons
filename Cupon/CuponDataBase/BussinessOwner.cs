using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuponDataBase
{
    public class BussinessOwner:User
    {
        public int BussinessOwnerId { get; set; }
        public virtual List<Bussiness> Business { get; set; }
    }
}
