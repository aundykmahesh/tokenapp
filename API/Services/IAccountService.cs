using API.Models;
using Domain;

namespace API.Services
{
    public interface IAccountService
    {
        string CreateJWTToken(User user);
        UserModel CreateUserObject(User appUser);
    }
}