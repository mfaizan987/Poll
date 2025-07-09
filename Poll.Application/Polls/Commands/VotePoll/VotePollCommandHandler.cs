using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Poll.Application.DTOs.Poll;
using Poll.Application.Interfaces;
using Poll.Application.IRepositories;
using Poll.Application.Polls.Commands.VotePoll;
using Poll.Domain.Entity;

public class VotePollCommandHandler : IRequestHandler<VotePollCommand, PollDto>
{
    private readonly IPollRepository _pollRepository;
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public VotePollCommandHandler(
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

    public async Task<PollDto> Handle(VotePollCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("Invalid token - user id missing.");

        if (request.PollVotes == null || !request.PollVotes.Any())
            throw new ArgumentException("No votes provided.");

        foreach (var voteDto in request.PollVotes)
        {
            if (voteDto.OptionIds == null || !voteDto.OptionIds.Any())
                throw new ArgumentException("Each vote must contain at least one option ID.");
        }

        var allOptionIds = request.PollVotes
            .SelectMany(v => v.OptionIds)
            .Distinct()
            .ToList();

        var options = await _pollRepository.GetPollOptionsByIdsAsync(allOptionIds, cancellationToken);

        if (options.Count != allOptionIds.Count)
            throw new Exception("One or more options not found.");

        var pollId = options.First().PollId;
        if (options.Any(o => o.PollId != pollId))
            throw new Exception("Options belong to different polls.");

        var poll = await _pollRepository.GetPollByIdAsync(pollId, cancellationToken);
        if (poll == null)
            throw new Exception("Poll not found.");

        if (!poll.AllowMultipleAnswers && allOptionIds.Count > 1)
            throw new Exception("Multiple answers are not allowed for this poll.");

        if (!poll.AllowMultipleAnswers)
        {
            var alreadyVoted = await _dbContext.PollVotes.AnyAsync(v =>
                v.UserId == userId &&
                poll.Options.Select(o => o.Id).Contains(v.PollOptionId) &&
                !v.IsDeleted,
                cancellationToken);

            if (alreadyVoted)
                throw new Exception("You have already voted on this poll.");
        }
        else
        {
            var alreadyVotedAny = await _dbContext.PollVotes.AnyAsync(v =>
                v.UserId == userId &&
                allOptionIds.Contains(v.PollOptionId) &&
                !v.IsDeleted,
                cancellationToken);

            if (alreadyVotedAny)
                throw new Exception("You have already voted for one or more of the selected options.");
        }

        var votes = options.Select(option => new PollVote
        {
            Id = Guid.NewGuid(),
            PollOptionId = option.Id,
            UserId = userId,
            Created_At = DateTime.UtcNow,
            Updated_At = DateTime.UtcNow,
            CreatedById = userId,
            UpdatedById = userId,
            IsDeleted = false,
            WorkSpaceId = _currentUserService.WorkSpaceId,
            StationId = _currentUserService.StationId,
            CompanyId = _currentUserService.CompanyId
        }).ToList();

        await _pollRepository.VotePollAsync(votes, cancellationToken);

        poll = await _pollRepository.GetPollByIdAsync(pollId, cancellationToken);

        var pollDto = _mapper.Map<PollDto>(poll);
        pollDto.HasVoted = true;

        // ✅ Add voted option IDs & Texts
        var userVotes = await _dbContext.PollVotes
            .Where(v => v.UserId == userId && !v.IsDeleted && v.PollOption != null && v.PollOption.PollId == pollId)
            .ToListAsync(cancellationToken);

        pollDto.UserVotedOptionId = userVotes.Select(v => v.PollOptionId).ToList();
        pollDto.UserVotedOptionText = poll.Options
            .Where(o => pollDto.UserVotedOptionId.Contains(o.Id))
            .Select(o => o.OptionText)
            .ToList();

        return pollDto;
    }
}
