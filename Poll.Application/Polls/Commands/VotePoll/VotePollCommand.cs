using MediatR;
using Poll.Application.Dtos;
using Poll.Application.DTOs.Poll;

namespace Poll.Application.Polls.Commands.VotePoll
{
    public class VotePollCommand : IRequest<PollDto>
    {
      public PollVoteDto PollVote { get; set; }
    }
}
