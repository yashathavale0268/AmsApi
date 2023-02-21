using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    public class JwtPayLoad
    {
        public string unique_name { get; set; }
        public string email { get; set; }

        public string role { get; set; }
    
    }
}
