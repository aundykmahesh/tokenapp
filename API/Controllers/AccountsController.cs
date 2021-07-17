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
        private readonly TokenService _jWTTokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountsController(TokenService jWTTokenService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _jWTTokenService = jWTTokenService;
            _userManager = userManager;
            _signInManager = signInManager;
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
                    Token = _jWTTokenService.CreateJWTToken(user)
                };
            }
            return Unauthorized();
        }



        // private UserModel CreateUser(UserModel userModel){

        // }
    }
}