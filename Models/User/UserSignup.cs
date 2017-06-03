using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Models.User
{
    public class UserSignup
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and numbers are allowed in usernames.")]
		public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
