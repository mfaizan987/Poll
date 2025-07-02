using MediatR;
using Poll.Application.DTOs.Poll;

namespace Poll.Application.Polls.Queries.GetPollById
{
    public class GetPollByIdQuery : IRequest<PollDto?>
    {
        public Guid PollId { get; set; }

        public GetPollByIdQuery(Guid pollId)
        {
            PollId = pollId;
        }
    }
}
