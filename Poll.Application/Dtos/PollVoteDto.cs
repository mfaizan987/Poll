using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Application.Dtos
{
    public class PollVoteDto
    {
            public Guid PollId { get; set; }
            public List<Guid> OptionIds { get; set; }
    }
}
