using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Models.Profile
{
    public class UpdatePasswordModel
    {
	    [Required(AllowEmptyStrings = false)]
	    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
	    [DataType(DataType.Password)]
		public string CurrentPassword { get; set; }

	    [Required(AllowEmptyStrings = false)]
	    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
	    [DataType(DataType.Password)]
	    public string NewPassword { get; set; }
	}
}
