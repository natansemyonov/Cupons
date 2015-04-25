using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuponDataBase
{
    public class Preference
    {
        public int PreferenceId { get; set; }
        public string Category { get; set; }
        public BasicUser Basic_User { get; set; }
    }
}
