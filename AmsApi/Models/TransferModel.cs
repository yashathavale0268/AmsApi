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
        public int TrId { get; set; } = 0;

        [DataMember(Name = "Aid")]
        public int Aid { get; set; } = 0;

        [DataMember(Name = "Branch")]
        public int Branch { get; set; } = 0;
        ///
        [DataMember(Name = "Description")]
        public string Description { get; set; } = "";

        [DataMember(Name = "Qty")]
        public int Qty { get; set; } = 0;

        [DataMember(Name = "Transferd_at")]
        public DateTime Transferd_at { get; set; } = DateTime.Now;

        [DataMember(Name = "TrfBranchName")]
        public string TrfBranchName { get; set; } = "";

    }
}
