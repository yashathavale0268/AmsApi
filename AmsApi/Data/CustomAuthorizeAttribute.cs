using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace AmsApi.Data
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute
    {
            private readonly string _requiredRole;
            //private readonly string _registercontrll;

        public CustomAuthorizeAttribute( string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        //    public override Task<AuthorizeAttribute> Authorize(HttpContext httpContext)
        //{
        //    var userRole = httpContext.Session.GetString("user_role");

        //        if (userRole == "Admin")
        //        {
        //            userRole = _requiredRole;
        //        }
        //        else if (userRole == "User")
        //        {
        //            userRole = _requiredRole;
        //        }
        //       else
        //        {
        //        return null;
        //        }
        //}
    }
}
