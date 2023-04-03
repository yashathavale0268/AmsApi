using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class ReportModel
    {
        [DataMember(Name = "userinfo")]
     public string userinfo { get; set; }

        [DataMember(Name = "branch")]

        public string branch { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }

        //---------------
        //numbers from qty not Nos
        //---------------
        [DataMember(Name = "assetsinUse")]
        public int assetsinUse { get; set; } = 0;

        [DataMember(Name = "workingassets")]
        public int workingassets { get; set; } = 0;

        [DataMember(Name = "spareassets")]
        public int spareassets { get; set; } = 0;

        [DataMember(Name = "totalrecord")]
        public int totalrecord { get; set; } = 0;

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "Statid")]
        public int Statid { get; set; }

        
    }
    
     

    


    

    
}
