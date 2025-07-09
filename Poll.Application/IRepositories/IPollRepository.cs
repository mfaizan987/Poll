using Poll.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Poll.Application.IRepositories
{
    public interface IPollRepository
    {
        Task<PollEntity> CreatePollAsync(PollEntity poll, CancellationToken cancellationToken);
        Task<PollEntity?> GetPollByIdAsync(Guid pollId, CancellationToken cancellationToken);
        Task<List<PollEntity>> GetAllPollsAsync(CancellationToken cancellationToken);
        Task VotePollAsync(PollVote vote, CancellationToken cancellationToken);
        Task VotePollAsync(List<PollVote> votes, CancellationToken cancellationToken);
        Task<PollOption?> GetPollOptionByIdAsync(Guid optionId, CancellationToken cancellationToken);
        Task<List<PollOption>> GetPollOptionsByIdsAsync(List<Guid> optionIds, CancellationToken cancellationToken);
    }
}
