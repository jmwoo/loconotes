﻿using System;
using System.Collections.Generic;
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
                Password = "sackface123",
                Uid = new Guid("D053117F-0788-4038-B275-C1F6C3309E06")
            },
            new User
            {
                Id = 2,
                Username = "jim",
                Password = "jimface123",
                Uid = new Guid("C57263B8-7031-4703-81B4-8A659CFA4A57")
            },
            new User
            {
                Id = 3,
                Username = "jaron",
                Password = "jaronface123",
                Uid = new Guid("1E398326-04AF-46AE-A09E-01964E1BD311")
            },
        };
    }


}
