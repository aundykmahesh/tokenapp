using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using Domain;
using Microsoft.IdentityModel.Tokens;
using System;
using API.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration) => _configuration = configuration;

        public string CreateJWTToken(User user)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

            var IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Utilities.GetTokenKey(_configuration)));
            var creds = new SigningCredentials(IssuerSigningKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokenDescriptor);

            return tokenhandler.WriteToken(token);

        }

        public string CreateAPIToken()
        {
            return RandomString(12);
        }
        //https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}