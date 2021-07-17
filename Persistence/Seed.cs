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
                    User _user = new User{
                        DisplayName = $"Test user {i}",
                        UserName = $"testuser{i}",
                        Email=$"testuser{i}@tesstemail.com"
                    };

                    await userManager.CreateAsync(_user,"Pa$$W0rd1234");
                }
            }

        }
    }
}