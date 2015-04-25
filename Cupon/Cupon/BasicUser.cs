using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cupon
{
    public class BasicUser : User
    {
        public string Gender { get; set; }
        public string Phone_Number { get; set; }
        public string LastKnownLocation { get; set; }
        public DateTime Birth_Date { get; set; }


       // public CuponHistory History { get; set; }
        public virtual List<Preference> Posts { get; set; } 
    }
}
