using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Application.Dtos
{
    public class PollOptionDto:BaseDto
    {
        public Guid Id { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public int VoteCount { get; set; }
    }
}
