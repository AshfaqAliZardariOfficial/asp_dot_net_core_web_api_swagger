using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Testing.Interfaces;

namespace Testing.Utils
{
    public class Auth : IJwtAuth
    {
        private readonly string username = "ashfaq";
        private readonly string password = "ashfaq";
        private readonly string key;
        public Auth(string key)
        {
            this.key = key;
        }

        public string Authentication(string username, string password)
        {
            if (!this.username.Equals(username) || !this.password.Equals(password))
            {
                return null;
            }

            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }), // Subject – New Claim identity

                Expires = DateTime.UtcNow.AddHours(1), // Expired – When it will be expired.

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature) // SigningCredentical – Private key + Algorithm

            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }
    }
}
