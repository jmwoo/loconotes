using loconotes.Models.User;

namespace loconotes.Models.Note
{
    public class NoteCreateModel
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }
        public bool IsAnonymous { get; set; }

        // TODO: move to dedicated mapper
        public Note ToNote(ApplicationUser user)
        {
            return new Note
            {
                Body = this.Body,
                Subject = string.IsNullOrWhiteSpace(this.Subject) ? null : this.Subject,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                Radius = this.Radius,
                UserId = user.Id,
                IsAnonymous = this.IsAnonymous
            };
        }
    }
}
