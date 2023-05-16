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
        [DataMember(Name = "Name")]
        public string Name { get; set; }
        [DataMember(Name = "Created_at")]
        public DateTime Created_at { get; set; }
    }
}
