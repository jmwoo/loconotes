using System;

namespace loconotes.Models.Note
{
    public class NoteViewModel
    {
        public int Id { get; set; }
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
