using Microsoft.EntityFrameworkCore;
using Poll.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Application.Interfaces
{
    public interface IAppDbContext
    {
         DbSet<PollEntity> Polls { get; set; }
         DbSet<PollOption> PollOptions { get; set; }
         DbSet<PollVote> PollVotes { get; set; }
    }
}
