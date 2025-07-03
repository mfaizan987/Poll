using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Application.Dtos
{
    public class PollVoteDto : BaseDto
    {
        public Guid PollId { get; set; }
        public Guid OptionId { get; set; }
        public Guid UserId { get; set; }
    }
}
