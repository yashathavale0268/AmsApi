using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class MenuModel
    {
        [DataMember(Name = "MenuId")]
        public Int64 MenuId { get; set; }
        [DataMember(Name = "Menu")]
        public string Menu { get; set; }
        
        
    }
}
