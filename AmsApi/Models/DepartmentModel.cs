using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AmsApi.Models
{

   [DataContract]
    public class DepartmentModel
    {

      [Key]
      [DataMember(Name ="Depid")]
        public int Depid { get; set; }
        [DataMember(Name = "Name")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string Name { get; set; }
        [DataMember(Name = "active")]
        public  bool active { get; set; }
        [DataMember(Name = "Remarks")]
        //[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
        public string Remarks { get; set; }
        [FromQuery]
        [DataMember(Name = "Created_at")]
        public DateTime? Created_at { get; set; } = DateTime.Now;
    }
}
