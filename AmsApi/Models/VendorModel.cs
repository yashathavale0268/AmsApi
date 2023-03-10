using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{[DataContract]
    public class VendorModel
    {
        [DataMember(Name = "Vendorid")]
        public int Vendorid { get; set; }

        [DataMember(Name = "Name")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string Name { get; set; }

        [DataMember(Name = "InvoiceNo")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string InvoiceNo { get; set; }

        [DataMember(Name = "InvoiceDate")]
        public DateTime? InvoiceDate { get; set; } 

        [DataMember(Name = "Warranty_Till")]
        public DateTime? Warranty_Till { get; set; }
        
        [DataMember(Name = "active")]
        public bool active { get; set; }
        








    }
}
