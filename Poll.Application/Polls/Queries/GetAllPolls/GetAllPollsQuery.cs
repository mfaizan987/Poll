using MediatR;
using Poll.Application.Dtos;
using Poll.Application.DTOs.Poll;
using Poll.Domain.Entity;

namespace Poll.Application.Polls.Queries.GetAllPolls
{
    public class GetAllPollsQuery : IRequest<List<PollListDto>>
    {
    }
}
