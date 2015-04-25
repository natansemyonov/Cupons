using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuponDataBase
{
    public class BasicUser : User
    {
        public int BasicUserId { get; set; }
        public string Gender { get; set; }
        public string Phone_Number { get; set; }
        public string LastKnownLocation { get; set; }
        public DateTime Birth_Date { get; set; }


        public CuponHistory History { get; set; }
        public virtual List<Preference> Posts { get; set; } 
    }
}
