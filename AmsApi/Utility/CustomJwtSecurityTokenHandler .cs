using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;

namespace AmsApi.Utility
{
    public class CustomJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token cannot be null or empty.");
            }

            if (validationParameters == null)
            {
                throw new ArgumentNullException(nameof(validationParameters));
            }

            // Set the algorithm to use for token validation
            //validationParameters.AlgorithmValidator = (string algorithm, SecurityToken token, TokenValidationParameters validationParams) =>
            //{
            //    if (algorithm == "HS256") // replace with the actual algorithm used to sign the token
            //    {
            //        return true;
            //    }

            //    return false;
            //};

            // Set the key used to sign the token
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_custom_key_here"));

            return base.ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}
