using MediatR;
using Poll.Application.DTOs.Poll;

public class CreatePollCommand : IRequest<PollDto>
{
    public CreatePollDto PollDto { get; set; }
}
