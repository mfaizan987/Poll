using AutoMapper;
using MediatR;
using Poll.Application.DTOs.Poll;
using Poll.Application.IRepositories;
using Poll.Application.Polls.Queries.GetPollById;

public class GetPollByIdQueryHandler : IRequestHandler<GetPollByIdQuery, PollDto?>
{
    private readonly IPollRepository _pollRepository;
    private readonly IMapper _mapper;

    public GetPollByIdQueryHandler(IPollRepository pollRepository, IMapper mapper)
    {
        _pollRepository = pollRepository;
        _mapper = mapper;
    }

    public async Task<PollDto?> Handle(GetPollByIdQuery request, CancellationToken cancellationToken)
    {
        var pollEntity = await _pollRepository.GetPollByIdAsync(request.PollId, cancellationToken);
        return pollEntity != null ? _mapper.Map<PollDto>(pollEntity) : null;
    }
}
