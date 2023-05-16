using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{ [DataContract]
    public class EnvTypeModel
    {
        [DataMember(Name = "EnvTypid")]
        public int EnvTypid { get; set; }

        [DataMember(Name = "EnvTyp")]
        public string EnvTyp { get; set; }

        //[DataMember(Name = "Created_at")]
        //public string Created_at { get; set; }
    }
}
