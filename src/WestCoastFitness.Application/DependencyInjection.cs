using Microsoft.Extensions.DependencyInjection;
using WestCoastFitness.Application.Bookings;
using WestCoastFitness.Application.Catalog;
using WestCoastFitness.Application.Members;
using WestCoastFitness.Domain.Bookings;
using WestCoastFitness.Domain.Memberships;

namespace WestCoastFitness.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ArizonaPricingService>();
        services.AddSingleton<BookingPolicy>();
        services.AddScoped<RegisterMemberHandler>();
        services.AddScoped<BookClassHandler>();
        services.AddScoped<CatalogQueries>();
        return services;
    }
}
