using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext dbcontext, UserManager<User> userManager)
        {

            if (!userManager.Users.Any())
            {

                for (int i = 0; i < 11; i++)
                {
                    User _user = new User
                    {
                        DisplayName = $"Test user {i}",
                        UserName = $"testuser{i}",
                        Email = $"testuser{i}@tesstemail.com"
                    };

                    await userManager.CreateAsync(_user, "Pa$$W0rd1234");
                }
            }

            if (dbcontext.Tokens.Count() <= 3)
            {
                for (int i = 0; i < 200; i++)
                {
                    Token token = new Token
                    {
                        AppUrl = $"www.testurl{i}.com",
                        CreatedDate = DateTime.Now,
                        Guid = new Guid(),
                        Status = TokenStatus.enabled,
                        TokenString = RandomString(12)
                    };

                    await dbcontext.Tokens.AddAsync(token);
                    await dbcontext.SaveChangesAsync();
            }
        }

    }
    private static string RandomString(int length)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
}