using Microsoft.EntityFrameworkCore;
using Poll.Application.IRepositories;
using Poll.Domain.Entity;
using Poll.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Poll.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Poll.Infrastructure.Repositories
{
    public class PollRepository : BaseRepository<PollEntity>, IPollRepository
    {
        private readonly ICurrentUserService _currentUser;

        public PollRepository(
            AppDbContext context,
            IHttpContextAccessor http,
            ICurrentUserService currentUser
        )
            : base(context, http)
        {
            _currentUser = currentUser;
        }

        public async Task<PollEntity?> GetPollByIdAsync(Guid pollId, CancellationToken cancellationToken)
        {
            return await FilteredQuery()
                .Include(p => p.Options)
                    .ThenInclude(o => o.PollVotes)
                .FirstOrDefaultAsync(p => p.Id == pollId, cancellationToken);
        }

        public async Task<List<PollEntity>> GetAllPollsAsync(CancellationToken cancellationToken)
        {
            return await FilteredQuery()
                .Include(p => p.Options)
                    .ThenInclude(o => o.PollVotes)
                .ToListAsync(cancellationToken);
        }

        public async Task<PollEntity> CreatePollAsync(PollEntity poll, CancellationToken cancellationToken)
        {
            await _context.Polls.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return poll;
        }

        public async Task VotePollAsync(PollVote vote, CancellationToken cancellationToken)
        {
            vote.WorkSpaceId = _currentUser.WorkSpaceId;
            vote.StationId = _currentUser.StationId;
            vote.CompanyId = _currentUser.CompanyId;
            vote.CreatedById = _currentUser.UserId;
            vote.UpdatedById = _currentUser.UserId;

            var option = await _context.PollOptions
                .Include(o => o.Poll)
                .FirstOrDefaultAsync(o => o.Id == vote.PollOptionId, cancellationToken);

            if (option == null)
                throw new InvalidOperationException("Option not found.");

            var poll = option.Poll;

            if (poll == null)
                throw new InvalidOperationException("Poll not found for this option.");

            if (poll.AllowMultipleAnswers)
            {
                var alreadyVoted = await _context.PollVotes
                    .AnyAsync(v => v.PollOptionId == vote.PollOptionId && v.UserId == vote.UserId && !v.IsDeleted, cancellationToken);

                if (alreadyVoted)
                    throw new InvalidOperationException("You have already voted for this option.");
            }
            else
            {
                var alreadyVotedAny = await _context.PollVotes
                    .Where(v => v.UserId == vote.UserId && !v.IsDeleted)
                    .AnyAsync(v =>
                        _context.PollOptions.Any(po => po.Id == v.PollOptionId && po.PollId == poll.Id),
                        cancellationToken);

                if (alreadyVotedAny)
                    throw new InvalidOperationException("You have already voted in this poll.");
            }

            await _context.PollVotes.AddAsync(vote, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task VotePollAsync(List<PollVote> votes, CancellationToken cancellationToken)
        {
            foreach (var vote in votes)
            {
                vote.WorkSpaceId = _currentUser.WorkSpaceId;
                vote.StationId = _currentUser.StationId;
                vote.CompanyId = _currentUser.CompanyId;
                vote.CreatedById = _currentUser.UserId;
                vote.UpdatedById = _currentUser.UserId;
            }

            await _context.PollVotes.AddRangeAsync(votes, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PollOption?> GetPollOptionByIdAsync(Guid optionId, CancellationToken cancellationToken)
        {
            return await _context.PollOptions
                .FirstOrDefaultAsync(x => x.Id == optionId, cancellationToken);
        }

        public async Task<List<PollOption>> GetPollOptionsByIdsAsync(List<Guid> optionIds, CancellationToken cancellationToken)
        {
            return await _context.PollOptions
                .Where(po => optionIds.Contains(po.Id))
                .Include(po => po.Poll)
                .ToListAsync(cancellationToken);
        }
    }
}
