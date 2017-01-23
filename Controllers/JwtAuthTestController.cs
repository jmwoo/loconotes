using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace loconotes.Controllers
{
    [Route("api/test")]
    public class JwtAuthTestController : Controller
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public JwtAuthTestController()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var response = new
            {
                made_it = "hi sacks"
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }
    }
}
