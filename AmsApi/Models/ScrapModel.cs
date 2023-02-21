using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class ScrapModel
    {
        [DataMember(Name = "Scrapid")]
        public int Scrapid { get; set; }
        [DataMember(Name = "Asset")]
        public int Asset { get; set; }
        [DataMember(Name = "Branch")]
        public int Branch { get; set; }
        [DataMember(Name = "Last_user")]
        public int Last_user { get; set; }
        [DataMember(Name = "Vendor")]
        public int Vendor { get; set; }
        [DataMember(Name = "Created_at")]
        public DateTime Created_at { get; set; }
        [DataMember(Name = "active")]

        public bool active { get; set; }
        
                    
                    

    }
}
