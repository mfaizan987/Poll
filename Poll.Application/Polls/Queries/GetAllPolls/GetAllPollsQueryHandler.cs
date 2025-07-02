using AutoMapper;
using MediatR;
using Poll.Application.Dtos;
using Poll.Application.IRepositories;
using Poll.Application.Polls.Queries.GetAllPolls;

public class GetAllPollsQueryHandler : IRequestHandler<GetAllPollsQuery, List<PollListDto>>
{
    private readonly IPollRepository _pollRepository;
    private readonly IMapper _mapper;

    public GetAllPollsQueryHandler(IPollRepository pollRepository, IMapper mapper)
    {
        _pollRepository = pollRepository;
        _mapper = mapper;
    }

    public async Task<List<PollListDto>> Handle(GetAllPollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await _pollRepository.GetAllPollsAsync(cancellationToken);
        return _mapper.Map<List<PollListDto>>(polls);
    }
}
