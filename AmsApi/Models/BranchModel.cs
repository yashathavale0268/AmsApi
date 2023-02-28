using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class BranchModel
    {
        [Key]
        [DataMember(Name = "Branchid")]
        public int Branchid { get; set; }
        [DataMember(Name = "Name")]
        public string Name { get; set; }
        [DataMember(Name = "Created_at")]
        public DateTime? Created_at { get; set; } = DateTime.Now;
        [DataMember(Name = "active")]
        public bool Active { get; set; }
        
    }
}
