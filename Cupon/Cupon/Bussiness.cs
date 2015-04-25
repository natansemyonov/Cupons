using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cupon
{
    public class Bussiness
    {
        public int BussinessId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        //public virtual BussinessOwner Owner { get; set; }
        public virtual List<BussinessCupon> Bussiness_Cupons { get; set; }
    }
}
