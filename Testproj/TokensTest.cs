using System;
using System.Linq;
using API.Controllers;
using API.Models;
using API.Services;
using Domain;
using Moq;
using Xunit;

namespace Testproj
{
    public class TokensTest
    {
        private Mock<ITokenService> mock = new Mock<ITokenService>();

        [Fact]
        public async void GenerateAndValidateToken(){

            var tokenmodel = new TokenFormModel {
                appUrl = $"http://token{RandomString(8)}.com"
            };

            var tokenstring = string.Empty;

            Token token = new Token
            {
                Guid = new Guid(),
                Status = TokenStatus.enabled,
                TokenString = RandomString(12),
                CreatedDate = DateTime.Now,
                AppUrl = tokenmodel.appUrl
            };

            mock.Setup(p => p.GenerateToken(tokenmodel)).ReturnsAsync(token);

            TokenController tokenController = new TokenController(mock.Object);

            var result = await tokenController.GenerateToken(tokenmodel);

            tokenstring = result.Value.TokenString;

            Assert.True(result.Value.AppUrl == tokenmodel.appUrl);
            Assert.True(result.Value.Status == TokenStatus.enabled);
            Assert.True(tokenstring.Length == 12);

            var tokenvalidationmodel = new TokenValidateModel
            {
                appUrl = tokenmodel.appUrl,
                tokenkey = tokenstring
            };

            bool validationresult=true;

            mock.Setup(p => p.ValidateToken(tokenvalidationmodel)).ReturnsAsync(validationresult);

            TokenController tokenController1 = new TokenController(mock.Object);

            var result1 = await tokenController1.ValidateToken(tokenvalidationmodel);

            Assert.True(result1.Value);
            
        }

        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}