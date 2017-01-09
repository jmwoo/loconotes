using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Business.GeoLocation;
using Newtonsoft.Json;

namespace loconotes.Models
{
    [Table("Notes")]
    public class Note : IGeoCode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid? Uid { get; set; }

        public int Score { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateCreated { get; set; }

        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Body { get; set; }

        [Range(typeof(decimal), "-90", "90")]
        public decimal Latitude { get; set; }

        [JsonIgnore]
        public double LatitudeD => Convert.ToDouble(this.Latitude);

        [Range(typeof(decimal), "-180", "180")]
        public decimal Longitude { get; set; }

        [JsonIgnore]
        public double LongitudeD => Convert.ToDouble(this.Longitude);

        [Range(1, int.MaxValue)]
        public int Radius { get; set; }

        double? IGeoCode.LatitudeGeoCode => this.LatitudeD;
        double? IGeoCode.LongitudeGeoCode => this.LongitudeD;


        // TODO: move this to dedicated mapper
        public NoteViewModel ToNoteViewModel()
        {
            return new NoteViewModel
            {
                Uid = this.Uid.Value,
                Score = this.Score,
                DateCreated = this.DateCreated.Value,
                Subject = this.Subject,
                Body = this.Body,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                Radius = this.Radius
            };
        }
    }
}
