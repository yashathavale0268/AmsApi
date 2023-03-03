using System;
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
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        //[RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", ErrorMessage = "Invalid email address.")]

       
        public string Email { get; set; }
        [DataMember(Name = "Username")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        //ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        [DataMember(Name = "Password")]
        public string Password { get; set; }

        [DataMember(Name = "FullName")]
        
        public string FullName { get; set; }

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
        public int? Floor { get; set; }
        [DataMember(Name = "Company")]
        public int? Company { get; set; }
        [DataMember(Name = "Created_at")]
        public DateTime Created_at { get; set; } = DateTime.Now;
        [DataMember(Name = "active")]
        public bool Active { get; set; }
        
        [DataMember(Name = "Role")]
        public string Role { get; internal set; }

    }
}
