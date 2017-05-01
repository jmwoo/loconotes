using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Models.Profile
{
    public class UpdateProfileModel
    {
		[Required(AllowEmptyStrings = false)]
		[StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string Username { get; set; }
    }
}
