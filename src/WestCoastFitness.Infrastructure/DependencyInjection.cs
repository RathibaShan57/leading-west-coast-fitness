using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WestCoastFitness.Application.Abstractions;
using WestCoastFitness.Infrastructure.Persistence;

namespace WestCoastFitness.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("FitnessDatabase")
            ?? throw new InvalidOperationException("Connection string FitnessDatabase is not configured.");

        services.AddDbContext<FitnessDbContext>(options =>
            options.UseNpgsql(
                connection,
                npgsql =>
                {
                    npgsql.MigrationsHistoryTable("__ef_migrations", "fitness");
                    npgsql.MigrationsAssembly("WestCoastFitness.Infrastructure");
                }));
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<FitnessDbContext>());
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();
        return services;
    }
}
