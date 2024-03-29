﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AmsApi.Models
{
    [DataContract]

    public class UserModel 
    {
        [DataMember(Name = "Userid")]
        [Key]
        public int Userid { get; set; }

        [DataMember(Name = "Email")]
        
        //[RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", ErrorMessage = "Invalid email address.")]

       
        public string Email { get; set; }
        [DataMember(Name = "Username")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string Username { get; set; }
        
        [DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        //ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        [DataMember(Name = "Password")]
        public string Password { get; set; }


        [DataMember(Name = "First_name")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string First_name { get; set; }
        [DataMember(Name = "Last_name")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string Last_name { get; set; }
        [DataMember(Name = "Department")]
        public int? Department { get; set; }

        [DataMember(Name = "Branch")]
        public int? Branch { get; set; }
        [DataMember(Name = "Floor")]
        public string Floor { get; set; }
        [DataMember(Name = "Company")]
        public int? Company { get; set; }
        [DataMember(Name = "Created_at")]
        public DateTime Created_at { get; set; } = DateTime.Now;
        [DataMember(Name = "active")]
        public bool active { get; set; }
        
        [DataMember(Name = "Role")]
        public int Role { get;  set; }

        [DataMember(Name = "RoleName")]
        public string RoleName { get; set; }
        [DataMember(Name = "Full_name")]

        public string Full_name { get; set; }

        [DataMember(Name = "DepartmentName")]
        public string DepartmentName { get; set; }

        [DataMember(Name = "BranchName")]
        public string BranchName { get; set; }

        [DataMember(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "totalrecord")]
        public int totalrecord { get; set; }

        [DataMember(Name = "token")]
        public string token { get; set; }
    }
}
