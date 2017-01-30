using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using loconotes.Models.User;
using System.Security.Principal;

namespace loconotes.Services
{
    public interface ILoginService
    {
        Task<User> Login(UserLogin userLogin);
    }

    public class LoginService : ILoginService
    {
        public LoginService()
        {
        }

        public async Task<User> Login(UserLogin userLogin)
        {
            await Task.Delay(0);

            return IdentityService.Users.FirstOrDefault(u => u.Username == userLogin.Username && u.Password == userLogin.Password);
        }

        public class CustomClaimTypes
        {
            public static string Username => "username";
            public static string UserId => "userid";
        }
    }
}
