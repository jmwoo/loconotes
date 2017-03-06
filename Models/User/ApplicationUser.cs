using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Models.User
{
    public class ApplicationUser
    {
        public string Username { get; set; }
        public int Id { get; set; }
        public Guid Uid { get; set; }
    }
}
