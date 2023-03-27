using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class LocationModel
    {
        [Key]
        [DataMember(Name = "locid")]
        public int locid { get; set; }

        [DataMember(Name = "uid")]
        public int uid { get; set; }

        [DataMember(Name = "reqid")]
        public int reqid { get; set; }

        [DataMember(Name = "Asset")]
        public int Asset { get; set; }

        [DataMember(Name = "type")]
        public int type { get; set; }

        [DataMember(Name = "branch")]
        public int branch { get; set; }

        [DataMember(Name = "department")]
        public int department { get; set; }

        [DataMember(Name = "company")]
        public int company { get; set; }

        [DataMember(Name = "floor ")]
        public int floor { get; set; }

        [DataMember(Name = "Extradetails")]
        public string Extradetails { get; set; }

        [DataMember(Name = "active")]
        public bool active { get; set; }

        

        [DataMember(Name = "status ")]
        public int status { get; set; }

        [DataMember(Name = "Users")]
        public string Users { get; set; }

        [DataMember(Name = "Assetname")]
        public string Assetname { get; set; }

        [DataMember(Name = "typename")]
        public string typename { get; set; }

        [DataMember(Name = "branchname")]
        public string branchname { get; set; }

        [DataMember(Name = "departmentname")]
        public string departmentname { get; set; }

        [DataMember(Name = "companyname")]
        public string companyname { get; set; }

        [DataMember(Name = "requested")]
        public string requested{ get; set; }
        [DataMember(Name = "StatusNow")]
        public string StatusNow { get; set; }

        [DataMember(Name = "totalrecord")]
        public int totalrecord { get; set; }

    }
}
