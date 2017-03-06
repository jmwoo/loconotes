using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using loconotes.Models.User;
using loconotes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace loconotes.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService
        )
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var jwtResult = await _authService.Login(userLogin);

            if (jwtResult == null)
            {
                return BadRequest("Invalid credentials");
            }

            return Ok(jwtResult);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody] UserSignup userSignup)
        {
            try
            {
                var jwtResult = await _authService.Signup(userSignup).ConfigureAwait(false);
                return Ok(jwtResult);
            }
            catch (ConflictException conflictException)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
        }
    }
}
