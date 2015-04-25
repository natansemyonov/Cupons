using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuponDataBase
{
    public class Cupon
    {
        //public int CuponId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Original_Price { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string Category { get; set; }
        public string Location { get; set; }
    }
}
