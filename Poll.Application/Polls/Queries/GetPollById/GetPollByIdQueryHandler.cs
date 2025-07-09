using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Poll.Application.DTOs.Poll;
using Poll.Application.Interfaces;
using Poll.Application.IRepositories;
using Poll.Application.Polls.Queries.GetPollById;

public class GetPollByIdQueryHandler : IRequestHandler<GetPollByIdQuery, PollDto?>
{
    private readonly IPollRepository _pollRepository;
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetPollByIdQueryHandler(
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

    public async Task<PollDto?> Handle(GetPollByIdQuery request, CancellationToken cancellationToken)
    {
        var pollEntity = await _pollRepository.GetPollByIdAsync(request.PollId, cancellationToken);
        if (pollEntity == null) return null;

        var pollDto = _mapper.Map<PollDto>(pollEntity);

        var userId = _currentUserService.UserId;
        var optionIds = pollEntity.Options.Select(o => o.Id).ToList();

        var userVotes = await _dbContext.PollVotes
            .Where(v =>
                optionIds.Contains(v.PollOptionId) &&
                v.UserId == userId &&
                !v.IsDeleted)
            .Include(v => v.PollOption)
            .ToListAsync(cancellationToken);

        if (userVotes.Any())
        {
            pollDto.HasVoted = true;
            pollDto.UserVotedOptionId = userVotes.Select(v => v.PollOptionId).ToList();
            pollDto.UserVotedOptionText = userVotes.Select(v => v.PollOption.OptionText).ToList();
        }
        else
        {
            pollDto.HasVoted = false;
            pollDto.UserVotedOptionId = new List<Guid>();
            pollDto.UserVotedOptionText = new List<string>();
        }

        return pollDto;
    }
}
