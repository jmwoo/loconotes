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

		[HttpDelete]
		[Route("me")]
		public async Task DeleteMe()
		{
			throw new NotImplementedException();

			//_userService.DeleteMe(ApplicationUser);
		}

		[HttpGet]
		[Route("me/profile")]
        public async Task<UserProfile> Get()
        {
			return await _userService.GetProfile(ApplicationUser);
        }

		[HttpPatch]
		[Route("me/profile")]
		public async Task UpdateProfile([FromBody] UpdateProfileModel updateProfileModel)
		{
			await _userService.UpdateProfile(ApplicationUser, updateProfileModel);
		}

		[HttpPatch]
		[Route("me/password")]
		public async Task UpdatePassword([FromBody] UpdatePasswordModel updatePasswordModel)
		{
			await _userService.UpdatePassword(ApplicationUser, updatePasswordModel);
		}
	}
}
