using Microsoft.EntityFrameworkCore;
using Poll.Application.IRepositories;
using Poll.Domain.Entity;
using Poll.Infrastructure.Data;
using Microsoft.AspNetCore.Http;

namespace Poll.Infrastructure.Repositories
{
    public class PollRepository : BaseRepository<PollEntity>, IPollRepository
    {
        public PollRepository(AppDbContext context, IHttpContextAccessor http)
            : base(context, http)
        {
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
            // get the poll option (needed to find the parent poll)
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
                    .AnyAsync(v => v.PollOptionId == vote.PollOptionId && v.UserId == vote.UserId, cancellationToken);

                if (alreadyVoted)
                    throw new InvalidOperationException("You have already voted for this option.");
            }
            else
            {
                var alreadyVotedAny = await _context.PollVotes
                    .Where(v => v.UserId == vote.UserId)
                    .AnyAsync(v =>
                        _context.PollOptions.Any(po => po.Id == v.PollOptionId && po.PollId == poll.Id),
                        cancellationToken);

                if (alreadyVotedAny)
                    throw new InvalidOperationException("You have already voted in this poll.");
            }

            await _context.PollVotes.AddAsync(vote, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }



        public async Task<PollOption?> GetPollOptionByIdAsync(Guid optionId, CancellationToken cancellationToken)
        {
            return await _context.PollOptions
                .FirstOrDefaultAsync(x => x.Id == optionId, cancellationToken);
        }

        public async Task<UserEntity?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted, cancellationToken);
        }
    }
}
