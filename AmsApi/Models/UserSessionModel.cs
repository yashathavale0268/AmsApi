using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    
    public class UserSessionModel
    {

      
        public int Userid { get; set; }
      
        public string Email { get; set; }
       
        public string Username { get; set; }
      
        public string Role { get; internal set; }
    }
}
