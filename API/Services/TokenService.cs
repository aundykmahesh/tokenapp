using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API.Services
{
    public class TokenService : ControllerBase, ITokenService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<TokenService> _logger;

        public TokenService( DataContext dataContext, ILogger<TokenService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<IActionResult> DisableToken(Guid tokenId)
        {
            var token = await _dataContext.Tokens.FirstAsync(c => c.Guid == tokenId);
            token.Status = TokenStatus.disabled;
            var result = await _dataContext.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Update Failed");

            return Ok();
        }

        public async Task<IActionResult> EnableToken(Guid tokenId)
        {
            var token = await _dataContext.Tokens.FirstAsync(c => c.Guid == tokenId);
            token.Status = TokenStatus.enabled;
            token.CreatedDate = DateTime.Now;
            var result = await _dataContext.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Update Failed");

            return Ok();
        }

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
                    TokenString = CreateAPIToken(),
                    CreatedDate = DateTime.Now,
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

        public async Task<ActionResult<List<Token>>> GetTokens()
        {
            return await _dataContext.Tokens.ToListAsync();
        }

        public async Task<ActionResult<bool>> ValidateToken(TokenValidateModel tokenValidateModel)
        {
            var results = await GetToken(tokenValidateModel);

            if (results == null || results.TokenString != tokenValidateModel.tokenkey)
            {
                return BadRequest("API token  Not Found");
            }
            else if (results.Status == TokenStatus.expired ||
                results.Status == TokenStatus.disabled ||
                results.CreatedDate.AddDays(7) < DateTime.Now)
            {
                if (results.CreatedDate.AddDays(7) < DateTime.Now)
                {
                    await ExpireToken(results.Guid);
                }
                return BadRequest("API token  expired");
            }
            else
            {
                return Ok("API Token is valid");
            }
        }

        private async Task<Token> GetTokenFromURL(string url)
        {
            var results = await _dataContext.Tokens.AsQueryable().Where(c => c.AppUrl == url).ToListAsync();
            return results.FirstOrDefault();
        }

        private async Task<Token> GetToken(TokenValidateModel tokenValidateModel)
        {
            if (tokenValidateModel.tokenkey.Length != 12)
            {
                return null;
            }

            return await GetTokenFromURL(tokenValidateModel.appUrl);
        }

        private async Task<ActionResult> ExpireToken(Guid tokenId)
        {
            var token = await _dataContext.Tokens.FirstAsync(c => c.Guid == tokenId);
            token.Status = TokenStatus.expired;
            var result = await _dataContext.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Update Failed");

            return Ok();
        }

        private string CreateAPIToken()
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