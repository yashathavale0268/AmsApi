﻿using System;
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
        [DataMember(Name = "Userid")]
        public int Userid { get; set; }

        
        [DataMember(Name = "Assetid")]
      //  public List<int> Assets { get; set; }
     
        public int Assetid { get; set; }
        [DataMember(Name = "Created_at")]
        public DateTime Created_at { get; set; } = DateTime.Now;
        [DataMember(Name = "Justify")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string Justify { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }

       

        [DataMember(Name = "active")]
        public bool active { get; set; }
        [DataMember(Name = "isworking")]
        public bool isworking { get; set; }
        [DataMember(Name = "inuse")]
        public bool inuse { get; set; } = false;

        [DataMember(Name = "Asset")]
        public string Asset { get; set; }
        [DataMember(Name = "CurrentStatus")]
        public string CurrentStatus { get; set; }
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }






    }
}
