using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuponDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
             using (var ctx = new CuponContext())
            {
                BasicUser user = new BasicUser() { BasicUserId = 1, User_name = "Natan", Password = "a", Email = "", Gender = "", Birth_Date = DateTime.Now , LastKnownLocation = "", Phone_Number ="" };
                ctx.Basic_Users.Add(user);
                
                ctx.SaveChanges();
                }
        }
    }
}
