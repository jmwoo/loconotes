using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using loconotes.Models.User;
using loconotes.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace loconotes.Controllers
{
    public class BaseController : Controller
    {

	    public override JsonResult Json(object data, JsonSerializerSettings serializerSettings)
	    {
			var stringEnumConverter = new StringEnumConverter(camelCaseText: true);
			serializerSettings.Converters.Add(stringEnumConverter);
		    return base.Json(data, serializerSettings);
	    }
    }
}
