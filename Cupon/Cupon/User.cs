using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cupon
{
    public class User
    {
        public int UserId { get; set; }
        public string User_name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public virtual List<SocialNetworkCupon> Social_Network_Cupons { get; set; }
    }
}
