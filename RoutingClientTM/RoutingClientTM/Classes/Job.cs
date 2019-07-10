using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingClientTM.Classes
{
   
    public class Job
    {
        public string date { get; set; }
        public string job_id { get; set; }
        public string driver_id { get; set; }
        public string bus_reg { get; set; }             
        public string start_location { get; set; }
        public string end_location { get; set; }        
        public string pass_num { get; set; }        
        public string route { get; set; }
        public string is_complete { get; set; }
    }
}
