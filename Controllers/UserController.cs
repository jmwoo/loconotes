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
		[Route("me/profile")]
        public async Task<UserProfile> Get()
        {
			return await _userService.GetProfile(GetApplicationUser());
        }

		[HttpPatch]
		[Route("me/profile")]
		public async Task UpdateProfile([FromBody] UpdateProfileModel updateProfileModel)
		{
			await _userService.UpdateProfile(GetApplicationUser(), updateProfileModel);
		}

		[HttpPatch]
		[Route("me/password")]
		public async Task UpdatePassword([FromBody] UpdatePasswordModel updatePasswordModel)
		{
			await _userService.UpdatePassword(GetApplicationUser(), updatePasswordModel);
		}
	}
}
