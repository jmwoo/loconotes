using loconotes.Data;
using loconotes.Models.Profile;
using loconotes.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Services
{
	public interface IUserService
	{
		Task<UserProfile> UpdateProfile(ApplicationUser applicationUser, UpdateProfileModel updateProfileModel);
		Task<UserProfile> GetProfile(ApplicationUser appUser);
	}

    public class UserService : IUserService
    {
		private readonly LoconotesDbContext _dbContext;

		public UserService(
			LoconotesDbContext dbContext
		)
		{
			_dbContext = dbContext;
		}

		public async Task<UserProfile> UpdateProfile(ApplicationUser applicationUser, UpdateProfileModel updateProfileModel)
		{
			var userDto = await _dbContext.Users.FindAsync(applicationUser.Id);

			var profile = new UserProfile();

			if (updateProfileModel.Username != null)
			{
				userDto.Username = updateProfileModel.Username;
				await _dbContext.SaveChangesAsync();
				profile.Username = userDto.Username;
			}

			return profile;
		}

		public async Task<UserProfile> GetProfile(ApplicationUser appUser)
		{
			var userDto = await _dbContext.Users.FindAsync(appUser.Id);
			
			// TODO: mapping
			return new UserProfile { Username = userDto.Username };
		}
	}
}
