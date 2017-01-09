using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Models
{
    public class NoteCreateModel
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }

        // TODO: move to dedicated mapper
        public Note ToNote()
        {
            return new Note
            {
                Body = this.Body,
                Subject = string.IsNullOrWhiteSpace(this.Subject) ? null : this.Subject,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                Radius = this.Radius,
            };
        }
    }
}
