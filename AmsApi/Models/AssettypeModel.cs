using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    
    public class AssettypeModel
    {
       [Key]
        [DataMember(Name = "Typeid")]
        public int Typeid { get; set; }
        [DataMember(Name = "Name")]
        public string Name { get; set; }
        [DataMember(Name = "Remerks")]
        public string Remarks { get; set; }
        [DataMember(Name = "Active")]
        public bool Active { get; set; }
      
    }
}
