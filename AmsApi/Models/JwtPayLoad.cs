using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    public class JwtPayLoad
    {
       public string Issuer { get; set; }

        public string Subject { get; set; }
        public IEnumerable<string> Audience { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime? NotBefore { get; set; }
        public DateTime? Expiration { get; set; }
        public Dictionary<string, object> Claims { get; set; }
    
    }
}
