using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class CustomerModel
    {
        [DataMember(Name="Custid")]
         public int Custid { get; set; }

        [DataMember(Name = "Cid")]
        public string Cid { get; set; }
    }
}
