using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace loconotes.Controllers
{
    [Route("")]
    public class HomeController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public string Get()
        {
            var ipAddr = Request.HttpContext.Connection.RemoteIpAddress;
            var now = DateTime.UtcNow;
            var msg = $"Hi '{ipAddr}' at '{now}'. Would you like some loconotes?";
            return msg;
        }
    }
}
