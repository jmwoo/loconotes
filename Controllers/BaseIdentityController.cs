using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace loconotes.Controllers
{
    public class BaseIdentityController : Controller
    {
        public ApplicationUser GetApplicationUser()
        {
            var claims = HttpContext.User.Claims.ToArray();

            var applicationUser = new ApplicationUser();

            var usernameClaim = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Username);
            var userIdClaim = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId);

            applicationUser.Username = usernameClaim?.Value;
            applicationUser.Id = Convert.ToInt32(userIdClaim?.Value ?? "0");

            return applicationUser;
        }
    }
}
