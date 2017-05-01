using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using loconotes.Models.Profile;
using loconotes.Services;

namespace loconotes.Controllers
{
	[Route("api/users")]
	public class UserController : BaseIdentityController
	{

		private readonly IUserService _userService;

		public UserController(
			IUserService userService
		)
		{
			_userService = userService;
		}

        [HttpGet]
		[Route("profile")]
        public async Task<UserProfile> Get()
        {
			return await _userService.GetProfile(GetApplicationUser());
        }

		[HttpPost]
		[Route("profile")]
		public async Task<UserProfile> Post([FromBody] UpdateProfileModel updateProfileModel)
		{
			return await _userService.UpdateProfile(GetApplicationUser(), updateProfileModel);
		}
	}
}
