using System.Linq;
using System.Threading.Tasks;
using loconotes.Models.User;

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
