using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class StatusModel
    {
        [DataMember(Name ="Statusid")]
        public int Statusid { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }

       
        
        [DataMember(Name = "Userid")]
        public int Userid { get; set; }

        [DataMember(Name = "Assetid")]

        public int Assetid { get; set; }

        [DataMember(Name = "Requestid")]
        public int Requestid { get; set; }

        [DataMember(Name = "Created_at")]
        public  DateTime Created_at { get; set; }

        [DataMember(Name = "active")]
        public bool active { get; set; }

        [DataMember(Name = "StatusNow")]
        public string StatusNow { get; set; }
        [DataMember(Name = "Request")]
        public string Request { get; set; }
        [DataMember(Name = "Asset")]
        public string Asset { get; set; }
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
    }
}
