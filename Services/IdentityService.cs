using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Models.User;

namespace loconotes.Services
{
    public interface IIdentityService
    {
    }

    public class IdentityService : IIdentityService
    {
        public static IEnumerable<User> Users => new[]
        {
            new User
            {
                Id = 1,
                Username = "sacks",
                Password = "sackface123"
            },
            new User
            {
                Id = 2,
                Username = "jim",
                Password = "jimface123"
            },
        };
    }


}
