using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using loconotes.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace loconotes.Controllers
{
    public class BaseIdentityController : BaseController
	{
        protected ApplicationUser GetApplicationUser()
        {
            // TODO: throw unauthorized if no user or claims

            try
            {
                var claims = HttpContext.User.Claims.ToArray();

                var applicationUser = new ApplicationUser();

                var usernameClaim = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Username);
                var userIdClaim = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId);
                var userUidClaim = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserUid);

                applicationUser.Username = usernameClaim?.Value;
                applicationUser.Id = Convert.ToInt32(userIdClaim?.Value ?? "0");
                applicationUser.Uid = Guid.Parse(userUidClaim.Value);

                return applicationUser;
            }
            catch (Exception)
            {
	            throw new UnauthorizedAccessException();
            }
        }
    }
}
