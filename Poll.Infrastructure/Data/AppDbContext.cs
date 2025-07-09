using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Poll.Application.Interfaces;
using Poll.Domain.Entity;

namespace Poll.Infrastructure.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly ICurrentUserService? _currentUserService;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IServiceProvider serviceProvider)
            : base(options)
        {
            _currentUserService = serviceProvider.GetService<ICurrentUserService>();
        }

        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<PollEntity> Polls { get; set; } = null!;
        public DbSet<PollOption> PollOptions { get; set; } = null!;
        public DbSet<PollVote> PollVotes { get; set; } = null!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUserId = _currentUserService?.UserId ?? Guid.Empty;
            var currentWorkspaceId = _currentUserService?.WorkSpaceId ?? Guid.Empty;
            var currentStationId = _currentUserService?.StationId ?? Guid.Empty;
            var currentCompanyId = _currentUserService?.CompanyId ?? Guid.Empty;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created_At = DateTime.UtcNow;
                        entry.Entity.Updated_At = DateTime.UtcNow;
                        entry.Entity.CreatedById = currentUserId;
                        entry.Entity.UpdatedById = currentUserId;
                        entry.Entity.WorkSpaceId = currentWorkspaceId;
                        entry.Entity.StationId = currentStationId;
                        entry.Entity.CompanyId = currentCompanyId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.Updated_At = DateTime.UtcNow;
                        entry.Entity.UpdatedById = currentUserId;
                        entry.Entity.WorkSpaceId = currentWorkspaceId;
                        entry.Entity.StationId = currentStationId;
                        entry.Entity.CompanyId = currentCompanyId;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
