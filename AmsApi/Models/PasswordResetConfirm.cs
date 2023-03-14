using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    public class PasswordResetConfirm
    {
        public string Email { get; set; }
        public string Token { get; set; }
        //public string Password { get; set; }
    }
}
