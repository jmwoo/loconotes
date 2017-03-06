using System;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using loconotes.Data;
using loconotes.Models.User;
using Microsoft.EntityFrameworkCore;

namespace loconotes.Services
{
    public interface IAuthService
    {
        Task<JwtResult> Login(UserLogin userLogin);
        Task<JwtResult> Signup(UserSignup userSignup);
    }

    public class AuthService : IAuthService
    {
        private readonly LoconotesDbContext _dbContext;
        private readonly ICryptoService _cryptoService;
        private readonly IJwtService _jwtService;

        public AuthService(
            LoconotesDbContext dbContext
            ,ICryptoService cryptoService
            , IJwtService jwtService
        )
        {
            _dbContext = dbContext;
            _cryptoService = cryptoService;
            _jwtService = jwtService;
        }

        public async Task<JwtResult> Login(UserLogin userLogin)
        {
            // TODO: validate login

            var userDto = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == userLogin.Username).ConfigureAwait(false);
            if (userDto == null)
            {
                return null;
            }
            var passwordHash = _cryptoService.Hash(userLogin.Password);
            if (userDto.PasswordHash != passwordHash)
            {
                return null;
            }

            // TODO: mapping
            var user = new User { Id = userDto.Id, Uid = userDto.Uid.Value, Username = userDto.Username };

            var jwt = await _jwtService.MakeJwt(user).ConfigureAwait(false);
            return jwt;
        }

        public async Task<JwtResult> Signup(UserSignup userSignup)
        {
            // TODO: validate signup

            try
            {
                var passwordHash = _cryptoService.Hash(userSignup.Password);
                var entityEntry =
                    await
                        _dbContext.Users.AddAsync(new UserDto
                        {
                            Username = userSignup.Username,
                            PasswordHash = passwordHash
                        }).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                var userDto = entityEntry.Entity;

                // TODO: mapping
                var user = new User {Id = userDto.Id, Uid = userDto.Uid.Value, Username = userDto.Username};

                var jwt = await _jwtService.MakeJwt(user);

                return jwt;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new ConflictException(dbUpdateException.Message, dbUpdateException);
            }
        }

        public class CustomClaimTypes
        {
            public static string Username => "username";
            public static string UserId => "userid";
        }
    }
}
