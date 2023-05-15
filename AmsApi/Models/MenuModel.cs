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
        [DataMember(Name = "menuId")]
        public Int64 MenuId { get; set; }
        [DataMember(Name = "menu")]
        public string Menu { get; set; }

        [DataMember(Name = "pageUrl")]
        public string PageUrl { get; set; }
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

    }
}
