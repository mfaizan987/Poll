using MediatR;
using Poll.Application.DTOs.Poll;

public class CreatePollCommand : IRequest<PollDto>
{
    public PollDto PollDto { get; set; }
}
