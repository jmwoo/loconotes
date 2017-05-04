using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace loconotes.Models.Profile
{
    public class UpdateProfileModel
    {
	    [JsonProperty(PropertyName = "ChangePassword")]
		public UpdatePasswordModel UpdatePasswordModel { get; set; }
	}
}
