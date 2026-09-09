using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Infrastructure.Persistence;
using BhumiLogistics.Infrastructure.Persistence.Interceptors;
using BhumiLogistics.Infrastructure.Persistence.Repositories;
using BhumiLogistics.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BhumiLogistics.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

            options.AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>());
        });

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<ILandPlotRepository, LandPlotRepository>();
        services.AddScoped<ILeaseOfferRepository, LeaseOfferRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<INotificationService, EmailNotificationService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
