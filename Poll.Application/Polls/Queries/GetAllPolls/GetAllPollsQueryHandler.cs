using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Poll.Application.Dtos;
using Poll.Application.Interfaces;
using Poll.Application.IRepositories;
using Poll.Application.Polls.Queries.GetAllPolls;

public class GetAllPollsQueryHandler : IRequestHandler<GetAllPollsQuery, List<PollListDto>>
{
    private readonly IPollRepository _pollRepository;
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetAllPollsQueryHandler(
        IPollRepository pollRepository,
        IAppDbContext dbContext,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _pollRepository = pollRepository;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<PollListDto>> Handle(GetAllPollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await _pollRepository.GetAllPollsAsync(cancellationToken);
        var userId = _currentUserService.UserId;

        var pollListDtos = _mapper.Map<List<PollListDto>>(polls);

        var allOptionIds = polls
            .SelectMany(p => p.Options)
            .Select(o => o.Id)
            .ToList();

        var votedOptionIds = await _dbContext.PollVotes
            .Where(v => allOptionIds.Contains(v.PollOptionId)
                     && v.UserId == userId
                     && !v.IsDeleted)
            .Select(v => v.PollOptionId)
            .ToListAsync(cancellationToken);

        foreach (var pollDto in pollListDtos)
        {
            var poll = polls.First(p => p.Id == pollDto.Id);

            var userVotes = poll.Options
                .SelectMany(o => o.PollVotes)
                .Where(v => v.UserId == userId && !v.IsDeleted)
                .ToList();

            if (userVotes.Any())
            {
                pollDto.HasVoted = true;
                pollDto.UserVotedOptionId = userVotes.Select(v => v.PollOptionId).ToList();
                pollDto.UserVotedOptionText = poll.Options
                    .Where(o => pollDto.UserVotedOptionId.Contains(o.Id))
                    .Select(o => o.OptionText)
                    .ToList();
            }
            else
            {
                pollDto.HasVoted = false;
                pollDto.UserVotedOptionId = new List<Guid>();
                pollDto.UserVotedOptionText = new List<string>();
            }
        }



        return pollListDtos;
    }
}
