using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace loconotes.Models.Note
{
    public class NoteSearchRequest
    {
        [Range(typeof(decimal), "-90", "90")]
        public decimal Latitude { get; set; }

        [JsonIgnore]
        public double LatitudeD => Convert.ToDouble(this.Latitude);

        [Range(typeof(decimal), "-180", "180")]
        public decimal Longitude { get; set; }

        [JsonIgnore]
        public double LongitudeD => Convert.ToDouble(this.Longitude);

        [Range(1, int.MaxValue)]
        public decimal RangeKm { get; set; } = 10;

        [JsonIgnore]
        public double RangeKmD => Convert.ToDouble(this.RangeKm);

        [Range(1, int.MaxValue)]
        public int Take { get; set; } = 10;
    }
}
