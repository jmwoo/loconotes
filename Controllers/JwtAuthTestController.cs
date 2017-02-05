using System.Linq;
using loconotes.Models.User;
using loconotes.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace loconotes.Controllers
{
    [Route("api/test")]
    [Authorize]
    public class JwtAuthTestController : BaseIdentityController
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public JwtAuthTestController(
        )
        {
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpGet]
        public IActionResult Get()
        {
            var user = GetApplicationUser();

            var json = JsonConvert.SerializeObject(user, _serializerSettings);
            return new OkObjectResult(json);
        }
    }
}
