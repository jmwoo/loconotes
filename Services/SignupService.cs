using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Models.User;

namespace loconotes.Services
{
    public interface ISignupService
    {
        Task<User> Signup(UserSignup userSignup);
    }

    public class SignupService : ISignupService
    {
        public async Task<User> Signup(UserSignup userSignup)
        {
            throw new NotImplementedException();
        }
    }
}
