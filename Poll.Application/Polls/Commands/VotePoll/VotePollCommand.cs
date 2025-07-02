using MediatR;
using Poll.Application.DTOs.Poll;

namespace Poll.Application.Polls.Commands.VotePoll
{
    public class VotePollCommand : IRequest<PollDto>
    {
        public Guid PollId { get; set; }
        public Guid OptionId { get; set; }
    }
}
