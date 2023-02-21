using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{ [DataContract]
    public class CompanyModel
    {
        [Key]
        [DataMember(Name="Companyid")]
        public int Companyid { get; set; }

        [DataMember(Name = "Name")]
        public String Name { get; set; }

        [DataMember(Name = "Remarks")]
        public String Remarks { get; set; }

        [DataMember(Name = "active")]
        public bool active { get;set; }
       
    }
}
