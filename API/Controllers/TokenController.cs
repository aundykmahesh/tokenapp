using System;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistence;
using Microsoft.EntityFrameworkCore;
using API.Models;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost("generate")]
        [Authorize]
        public async Task<ActionResult<Token>> GenerateToken([FromBody] TokenFormModel tokenmodel)
        {
            return await _tokenService.GenerateToken(tokenmodel);
        }

        [HttpPost("validate")]
        [AllowAnonymous]
        public async Task<ActionResult<Boolean>> ValidateToken(TokenValidateModel tokenValidateModel)
        {
            //check whether token exists in token table
            //if exists validate date, if expired change the status and send expired message
           
            return await _tokenService.ValidateToken(tokenValidateModel);           

        }

        [HttpGet("tokens")]
        [Authorize]
        public async Task<ActionResult<List<Token>>> GetTokens()
        {
            return await _tokenService.GetTokens();
        }

        [HttpPost("disabletoken")]
        [Authorize]
        public async Task<IActionResult> DisableToken(Guid tokenId)
        {
           return await _tokenService.DisableToken(tokenId);
        }

 [      HttpPost("enabletoken")]
        [Authorize]
        public async Task<IActionResult> EnableToken(Guid tokenId)
        {
            return await _tokenService.EnableToken(tokenId);
        }

       
    }
}