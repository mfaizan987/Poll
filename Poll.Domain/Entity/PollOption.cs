using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Poll.Domain.Entity
{
    public class PollOption : BaseEntity
    {
        public Guid PollId { get; set; }
        public string OptionText { get; set; } = null!;
        [JsonIgnore]
        public PollEntity Poll { get; set; } = null!;
        public ICollection<PollVote> PollVotes { get; set; } = new List<PollVote>();
    }
}
