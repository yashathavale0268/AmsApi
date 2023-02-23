using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CoreApiAdoDemo.Model
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "IsSuccess")]
        public bool  IsSuccess { get; set; }
        [DataMember(Name = "ItExists")]
        public bool ItExists { get; set; }

        [DataMember(Name = "ReturnMessage")]
        public string ReturnMessage { get; set; }

        [DataMember(Name = "Data")]
        public object Data { get; set; }


       
    }
}
