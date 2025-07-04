using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poll.Application.Interfaces;
using Poll.Application.IRepositories;
using Poll.Infrastructure.Data;
using Poll.Infrastructure.Repositories;
using Poll.Infrastructure.Services;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AccountDb")));

        services.AddHttpContextAccessor();


        services.AddScoped<IPollRepository, PollRepository>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}
