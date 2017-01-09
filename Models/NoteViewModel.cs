using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Models
{
    public class NoteViewModel
    {
        public Guid Uid { get; set; }
        public int Score { get; set; }
        public DateTime DateCreated { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }
    }
}
