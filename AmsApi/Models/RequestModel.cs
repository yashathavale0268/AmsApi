using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class RequestModel
    {
        [DataMember(Name = "Requestid")]
        public int Requestid { get; set; }
        [DataMember(Name = "Empid")]
        public int Empid { get; set; }
        //[DataMember(Name = "Assets")]
      [DataMember(Name = "Assetid")]
      //  public List<int> Assets { get; set; }
     //   public string Assets { get; set; }
        public int Assetid { get; set; }
        [DataMember(Name = "Created_at")]
        public DateTime Created_at { get; set; }
        [DataMember(Name = "Justify")]
        public string Justify { get; set; }
        [DataMember(Name = "Status")]
        public int Status { get; set; }
        [DataMember(Name = "active")]
        public bool active { get; set; }

        
                    
                    
                    
                   
                    

    }
}
