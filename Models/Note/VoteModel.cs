using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace loconotes.Models.Note
{
    public class VoteModel
    {
        public VoteEnum Vote { get; set; }
        
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
