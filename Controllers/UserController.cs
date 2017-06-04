using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using loconotes.Models.Note;
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
		private readonly INoteService _noteService;

		public UserController(
			IUserService userService,
			INoteService noteService
		)
		{
			_userService = userService;
			_noteService = noteService;
		}

		//[HttpDelete]
		//[Route("me")]
		//public async Task DeleteMe()
		//{
		//	throw new NotImplementedException();

		//	//_userService.DeleteMe(ApplicationUser);
		//}

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

		[HttpGet]
		[Route("{username}/notes")]
		[AllowAnonymous]
		public async Task<IEnumerable<NoteViewModel>> GetNotesByUser(string username)
		{
			return await _noteService.GetNotesByUser(ApplicationUser, username);
		}

		[HttpGet]
		[Route("me/notes")]
		public async Task<IEnumerable<NoteViewModel>> GetMyNotes()
		{
			return await _noteService.GetNotesByUser(ApplicationUser, ApplicationUser.Username);
		}

		[HttpGet]
		[Route("me/test")]
		[AllowAnonymous]
		public async Task Test()
		{
			throw new ValidationException();
		}
	}
}
