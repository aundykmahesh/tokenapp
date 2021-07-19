using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.Utils;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class AccountService : ControllerBase, IAccountService
    {

        private readonly IConfiguration _configuration;

        public AccountService( UserManager<User> userManager, 
            SignInManager<User> signInManager, IConfiguration configuration)
        {

            _configuration = configuration;
        }

        public UserModel CreateUserObject(User user)
        {
            return new UserModel
            {
                DisplayName = user.DisplayName,
                Token = CreateJWTToken(user),
                UserName = user.UserName
            };
        }

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

    }
}