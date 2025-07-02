using Microsoft.EntityFrameworkCore;
using Poll.Application.Interfaces;
using Poll.Application.IRepositories;
using Poll.Domain.Entity;
using Poll.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Infrastructure.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly AppDbContext _context;

        public PollRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PollEntity> CreatePollAsync(PollEntity poll, CancellationToken cancellationToken)
        {
            await _context.Polls.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return poll;
        }

        public async Task<PollEntity?> GetPollByIdAsync(Guid pollId, CancellationToken cancellationToken)
        {
            return await _context.Polls
                .Include(p => p.Options)
                .ThenInclude(o => o.PollVotes)
                .FirstOrDefaultAsync(p => p.Id == pollId && !p.IsDeleted, cancellationToken);
        }

        public async Task<List<PollEntity>> GetAllPollsAsync(CancellationToken cancellationToken)
        {
            return await _context.Polls
                .Include(p => p.Options)
                    .ThenInclude(o => o.PollVotes)
                .Where(p => !p.IsDeleted)
                .ToListAsync(cancellationToken);
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


    }

}
