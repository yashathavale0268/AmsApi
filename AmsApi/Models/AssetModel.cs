using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{[DataContract]
    public class AssetModel
    {
        [Key]

        [DataMember(Name = "Assetid")]
        public int Assetid { get; set; } = 0;
        
        [DataMember(Name = "SerialNo")]
        public String SerialNo { get; set; }
        
        [DataMember(Name = "Branch")]
        public int Branch { get; set; }
        
        [DataMember(Name = "Brand")]
        public String Brand { get; set; }
        
        [DataMember(Name = "Type")]
        public int Type { get; set; }
        
        [DataMember(Name = "Model")]
        public String Model { get; set; }
        
        [DataMember(Name = "Processor_Type")]
        public String Processor_Type { get; set; }
        
        [DataMember(Name = "Monitor_Type")]
        public String Monitor_Type { get; set; }
       
        [DataMember(Name = "Range_Type")]
        public String Range_Type { get; set; }
       
        [DataMember(Name = "Battery_Type")] 
        public String Battery_Type { get; set; }
        
        [DataMember(Name = "Battery_Ampere")]
        public String Battery_Ampere { get; set; }
       
        [DataMember(Name = "Battery_Capacity")]
        public String Battery_Capacity { get; set; }
        
        [DataMember(Name = "GrachicsCard")]
        public String GraphicsCard { get; set; }
       
        [DataMember(Name = "Optional_Drive")]
        public String Optical_Drive { get; set; }
        
        [DataMember(Name = "HDD")]
        public String HDD { get; set; }
        
        [DataMember(Name = "RAM")]
        public String RAM { get; set; }
       
        [DataMember(Name = "Inches")]
        public string Inches { get; set; }
        
        [DataMember(Name = "Port_Switch")]
        public String Port_Switch { get; set; }
       
        [DataMember(Name = "Nos")]
        public int Nos { get; set; }
        
        [DataMember(Name = "Specification")]
        public String Specification { get; set; }
        
        [DataMember(Name = "Vendorid")]
        public int Vendorid { get; set; }
        
        [DataMember(Name = "Status")]
        public int Status { get; set; }
        
        [DataMember(Name = "Allocated_to")]
        public int Allocated_to { get; set; }

        [DataMember(Name = "Remarks")]
        public String Remarks { get; set; }

        [DataMember(Name = "Created_at")]
        public DateTime Created_at { get; set; }
        
        [DataMember(Name = "active")]

        public bool active { get; set; }
       




















        
               
               
                
                
               

    }
}
