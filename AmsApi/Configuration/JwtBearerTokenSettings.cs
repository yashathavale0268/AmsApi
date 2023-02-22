using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Configuration
{
    public class JwtBearerTokenSettings
    {
        public string SecretKey { get; set; } //= "qwertyuiopasdfghjklzxcvbnmqwerty";
        public string ValidAudience { get; set; } 
        public string ValidIssuer { get; set; }
        public int ExpiryTimeInMinutes { get; set; } = 30;
    }
}
