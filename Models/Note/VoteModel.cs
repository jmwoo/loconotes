using Newtonsoft.Json;

namespace loconotes.Models.Note
{
    public class VoteModel
    {
        public VoteEnum Vote { get; set; }
        
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
