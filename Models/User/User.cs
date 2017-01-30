using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace loconotes.Models.User
{
    public class User
    {
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public int Id { get; set; }
    }
}
