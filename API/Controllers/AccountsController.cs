using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountService _accountservice;

        public AccountsController(UserManager<User> userManager, 
            SignInManager<User> signInManager, IAccountService accountservice)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountservice = accountservice;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> Login(LoginModel loginModel)
        {
            //first check email, then password, all good then generate token
            var user = await _signInManager.UserManager.FindByEmailAsync(loginModel.EMail);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (result == null)
            {
                return Unauthorized();
            }
            if (result.Succeeded)
            {
                return new UserModel
                {
                    DisplayName = user.DisplayName,
                    Id = new System.Guid(),
                    UserName = user.UserName,
                    Token = _accountservice.CreateJWTToken(user)
                };
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserModel>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return _accountservice.CreateUserObject(user);
        }

    }
}