using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Models
{
    public class NoteSearchRequest
    {
        [Range(typeof(decimal), "-90", "90")]
        public decimal Latitude { get; set; }
        public double LatitudeD => Convert.ToDouble(this.Latitude);

        [Range(typeof(decimal), "-180", "180")]
        public decimal Longitude { get; set; }

        public double LongitudeD => Convert.ToDouble(this.Longitude);

        [Range(1, int.MaxValue)]
        public decimal RangeKm { get; set; } = 10;

        public double RangeKmD => Convert.ToDouble(this.RangeKm);

        [Range(1, int.MaxValue)]
        public int Take { get; set; } = 10;
    }
}
