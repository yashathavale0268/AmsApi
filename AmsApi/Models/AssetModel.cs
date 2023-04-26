﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string SerialNo { get; set; } = "";
        
        [DataMember(Name = "Branch")]
        public int Branch { get; set; } = 0;


        [DataMember(Name = "Brand")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string Brand { get; set; } = "";

        [DataMember(Name = "Type")]
        public int Type { get; set; } = 0;
        
        [DataMember(Name = "Model")]

        public string Model { get; set; } = "";

        [DataMember(Name = "Processor_Type")]
        public string Processor_Type { get; set; } = "";

        [DataMember(Name = "Monitor_Type")]
        public string Monitor_Type { get; set; } = "";

        [DataMember(Name = "Range_Type")]
        public string Range_Type { get; set; } = "";

        [DataMember(Name = "Battery_Type")] 
        public string Battery_Type { get; set; } = "";

        [DataMember(Name = "Battery_Ampere")]
        public string Battery_Ampere { get; set; } = "";

        [DataMember(Name = "Battery_Capacity")]
        public string Battery_Capacity { get; set; } = "";

        [DataMember(Name = "GraphicsCard")]
        public string GraphicsCard { get; set; } = "";

        [DataMember(Name = "Optical_Drive")]
        public string Optical_Drive { get; set; } = "";

        [DataMember(Name = "HDD")]
        public string HDD { get; set; } = "";

        [DataMember(Name = "RAM")]
        public string RAM { get; set; } = "";

        [DataMember(Name = "Inches")]
        public int Inches { get; set; } =0;

        [DataMember(Name = "Port_Switch")]
        public string Port_Switch { get; set; } = "";

        [DataMember(Name = "Nos")]
        public int Nos { get; set; } = 0;

        [DataMember(Name = "Specification")]
        public string Specification { get; set; } = "";

        [DataMember(Name = "Vendorid")]
        public int Vendorid { get; set; } = 0;

        [DataMember(Name = "Status")]
        public int Status { get; set; }

        [DataMember(Name = "Remarks")]
        public string Remarks { get; set; } = "";

        [DataMember(Name = "Created_at")]
        public DateTime Created_at { get; set; } = DateTime.Now;

        [DataMember(Name = "InvoiceDate")]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        [DataMember(Name = "Warranty_Till")]
        public DateTime Warranty_Till{ get; set; } = DateTime.Now;

        [DataMember(Name = "active")]

        public bool active { get; set; } = true;


        [DataMember(Name = "Invoice_No")]
        public string Invoice_No { get; set; } = "";

        [DataMember(Name = "Location")]
        public int Location { get; set; } = 0;

        [DataMember(Name = "Uid")]
        public string Uid { get; set; } = "";

        /// 
        /// tables join columns~
        /// 

        ///
        /// transfertable ----------------------------------------------
        /// 
        //[DataMember(Name = "TrfBranch")]
        //public Int64 TrfBranch { get; set; }
        //[DataMember(Name = "Qty")]
       // public Int64 Qty { get; set; }
        [DataMember(Name = "Description")]
        public string Description { get; set; }
        /// -------------------------------------------------------------
        [DataMember(Name = "Branches")]
        public string Branches { get; set; } = "";

        [DataMember(Name = "TypeName")]
        public string TypeName { get; set; } = "";

        [DataMember(Name = "Vendors")]
        public string Vendors { get; set; } = "";
        [DataMember(Name = "StatusName")]
        public string StatusName { get; set; } = "";

        [DataMember(Name = "totalrecord")]
        public int totalrecord { get; set; } = 0;
        [DataMember(Name = "LocationName")]
        public string LocationName { get; set; } = "";
        
    }
}
