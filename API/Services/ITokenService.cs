using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface ITokenService
    {
        Task<ActionResult<Token>> GenerateToken([FromBody] TokenFormModel tokenmodel);
        Task<ActionResult<bool>> ValidateToken(TokenValidateModel tokenValidateModel);

        Task<ActionResult<List<Token>>> GetTokens();

        Task<IActionResult> DisableToken(Guid tokenId);
        Task<IActionResult> EnableToken(Guid tokenId);
    }
}