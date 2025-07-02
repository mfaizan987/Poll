using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Domain.Entity
{
    public class PollVote : BaseEntity
    {
            public Guid PollOptionId { get; set; }
            public PollOption PollOption { get; set; } = null!;
    }
}
