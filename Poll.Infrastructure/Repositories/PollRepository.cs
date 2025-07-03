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
