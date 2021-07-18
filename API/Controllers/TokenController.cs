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
        private readonly TokenService _tokenService;
        private readonly DataContext _dataContext;
        private readonly ILogger _logger;

        public TokenController(TokenService tokenService, DataContext dataContext, ILogger<TokenController> logger)
        {
            _tokenService = tokenService;
            _dataContext = dataContext;
            _logger = logger;
        }
        [HttpPost("generate")]
        [Authorize]
        public async Task<ActionResult<Token>> GenerateToken([FromBody] TokenFormModel tokenmodel)
        {
            if (tokenmodel == null) return BadRequest("Invalid test request parameters");

            if (GetTokenFromURL(tokenmodel.appUrl).Result != null)
            {
                return BadRequest("Failed to generate API token");
            }

            try
            {
                Token token = new Token
                {
                    Guid = new System.Guid(),
                    Status = TokenStatus.enabled,
                    TokenString = _tokenService.CreateAPIToken(),
                    CreatedDate = DateTime.Now.Date,
                    AppUrl = tokenmodel.appUrl
                };

                await _dataContext.Tokens.AddAsync(token);
                await _dataContext.SaveChangesAsync();
                return Ok(token);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("validate")]
        [AllowAnonymous]
        public async Task<ActionResult<Boolean>> ValidateToken(TokenValidateModel tokenValidateModel)
        {
            //check whether token exists in token table
            //if exists validate date, if expired change the status and send expired message
           

            var results = await GetToken(tokenValidateModel);

            if (results == null || results.TokenString != tokenValidateModel.tokenkey)
            {
                return BadRequest("API token  Not Found");
            }
            else if (results.Status == TokenStatus.expired || 
                results.Status == TokenStatus.disabled ||
                results.CreatedDate.AddDays(7) < DateTime.Now)
            {
                return BadRequest("API token  expired");
            }
            else
            {
                return Ok("API Token is valid");
            }

        }

        [HttpGet("tokens")]
        [Authorize]
        public async Task<ActionResult<List<Token>>> GetTokens()
        {
            return await _dataContext.Tokens.ToListAsync();
        }

        [HttpPost("disabletoken")]
        [Authorize]
        public async Task<IActionResult> DisableToken(Guid tokenId)
        {
            var token = await _dataContext.Tokens.FirstAsync(c=>c.Guid==tokenId);
            token.Status = TokenStatus.disabled;
            var result = await _dataContext.SaveChangesAsync() > 0;

            if(!result) return BadRequest("Update Failed");

            return Ok();
        }

        private async Task<Token> GetTokenFromURL(string url)
        {
            var results = await _dataContext.Tokens.AsQueryable().Where(c => c.AppUrl == url).ToListAsync();
            return results.FirstOrDefault();
        }

        protected async Task<Token> GetToken(TokenValidateModel tokenValidateModel)
        {
            if (tokenValidateModel.tokenkey.Length != 12)
            {
                return null;
            }

            return await GetTokenFromURL(tokenValidateModel.appUrl);
        }
    }
}