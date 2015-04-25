using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuponDataBase
{
    public class SocialNetworkCupon :Cupon
    {
        public int SocialNetworkCuponId { get; set; }
        public string URL { get; set; }
        public string PhoneNumber { get; set; }

        public virtual BasicUser User { get; set; }
    }
}
