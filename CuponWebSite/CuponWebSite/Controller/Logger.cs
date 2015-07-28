using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuponWebSite.Controller
{
    public class Logger
    {
        private static Logger _logger = null;
        public static Logger GetInstance {get { return _logger ?? (_logger = new Logger()); }
        }

        private Logger()
        {
            
        }

        public void Log(LogType type, string message)
        {
            using (ModelContainer entities = new ModelContainer())
            {
                Log l = new Log {Timestamp = DateTime.Now, Data = message, Type = type};
                entities.Logs.Add(l);
                entities.SaveChanges();
            }
        }
    }
}