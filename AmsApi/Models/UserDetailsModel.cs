using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    [DataContract]
    public class UserDetailsModel
    {
        [DataMember(Name = "Empid")]
        public int Empid { get; set; }
        [DataMember(Name = "Users")]
        public int Users { get; set; }
        //[DataMember(Name = "Email")]
        //public string Email { get; set; }
        //[DataMember(Name = "Username")]
        //public string Username { get; set; }
        [DataMember(Name = "First_name")]
        public string First_name { get; set; }
        [DataMember(Name = "Last_name")]
        public string Last_name { get; set; }
        [DataMember(Name = "Department")]
        public int? Department {get; set; }

        [DataMember(Name = "Branch")]
        public int? Branch { get; set; }
        [DataMember(Name = "Floor")]
        public int? Floor { get; set; }
        [DataMember(Name = "Company")]
        public int? Company { get; set; }

        [DataMember(Name = "Created_at")]
        public DateTime Created_at { get; set; } = DateTime.Now;

        [DataMember(Name = "active")]
        public bool active { get; set; }
        
    }
}
