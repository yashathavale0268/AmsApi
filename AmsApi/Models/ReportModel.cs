using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class ReportModel
    {
       

        //---------------
        //numbers from qty not Nos
        //---------------
        [DataMember(Name = "assetsinUse")]
        public int assetsinUse { get; set; }

        [DataMember(Name = "workingassets")]
        public int workingassets { get; set; }

        [DataMember(Name = "spareassets")]
        public int spareassets { get; set; }
            
        
        [DataMember(Name = "scrapedassets")]
        public int scrapedassets { get; set; }
        /// <summary>
        /// //////////////////////////////////////////////////////
        /// </summary>
        [DataMember(Name = "SentForFix")]
        public int SentForFix { get; set; }

        [DataMember(Name = "Assetid")]
        public int Assetid { get; set; }

        [DataMember(Name = "Requestid")]
        public int Requestid { get; set; }

        [DataMember(Name = "Uniqueid")]
        public string Uniqueid { get; set; }


        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "SentToVendor")]
        public string SentToVendor { get; set; }

        [DataMember(Name = "SentOnDate")]
        public string SentOnDate { get; set; }

        [DataMember(Name = "Created_at")]
        public string Created_at { get; set; }
        /// <summary>
        /// //////////////////////////////////////////////////////
        /// </summary>

        [DataMember(Name = "AssetsRequested")]
        public int AssetsRequested { get; set; } 

        [DataMember(Name = "totalrecord")]
        public int totalrecord { get; set; } 

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "NameofLastUsedBy")]
        public string NameofLastUsedBy { get; set; }
        
        [DataMember(Name = "LastUsedBy")]
        public int LastUsedBy { get; set; }

        [DataMember(Name = "userinfo")]
        public string userinfo { get; set; }

        [DataMember(Name = "branch")]

        public string branch { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "Statid")]
        public int Statid { get; set; }

        [DataMember(Name = "locid")]
        public int locid { get; set; }

        [DataMember(Name = "brchid ")]

        public int brchid { get; set; }

        [DataMember(Name = "typid")]
        public int typid { get; set; }
        [DataMember(Name = "Qty")]
        public int Qty{ get; set; }

    }
    
     

    


    

    
}
