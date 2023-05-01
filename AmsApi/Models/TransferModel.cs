using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class TransferModel
    {
        [Key]

        [DataMember(Name = "TrId")]
        public int TrId { get; set; }

        [DataMember(Name = "Aid")]
        public int Aid { get; set; } 

        [DataMember(Name = "Branch")]
        public int Branch { get; set; } 
        ///
        [DataMember(Name = "Description")]
        public string Description { get; set; } 

        [DataMember(Name = "Qty")]
        public int Qty { get; set; } 

        [DataMember(Name = "Transferd_at")]
        public DateTime Transferd_at { get; set; } = DateTime.Now;

        [DataMember(Name = "TrfBranchName")]
        public string TrfBranchName { get; set; }

        [DataMember(Name = "TrfBranchName")]
        public int totalrecord { get; set; }


    }
}
