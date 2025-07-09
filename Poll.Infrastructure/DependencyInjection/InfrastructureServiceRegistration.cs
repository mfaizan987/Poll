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
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("AccountDb"));
            options.UseApplicationServiceProvider(serviceProvider);
        });

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IPollRepository, PollRepository>();
        
        return services;
    }
}


