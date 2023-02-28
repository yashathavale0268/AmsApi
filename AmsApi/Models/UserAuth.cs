using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AmsApi.Models
{
    [DataContract]
    public class UserAuth  
    {
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "password/PasswordHash")]
        public string PasswordHash { get; set; }

        [DataMember(Name = "Role/UserRole")]
        public string Role { get; set; }
  //      "id": "string",
  //"userName": "string",
  //"normalizedUserName": "string",
  //"email": "string",
  //"normalizedEmail": "string",
  //"emailConfirmed": true,
  //"passwordHash": "string",
  //"securityStamp": "string",
  //"concurrencyStamp": "string",
  //"phoneNumber": "string",
  //"phoneNumberConfirmed": true,
  //"twoFactorEnabled": true,
  //"lockoutEnd": "2023-02-13T06:15:53.654Z",
  //"lockoutEnabled": true,
  //"accessFailedCount": 0
    }
}
