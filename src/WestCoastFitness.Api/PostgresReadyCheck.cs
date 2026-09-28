using Microsoft.Extensions.Diagnostics.HealthChecks;
using WestCoastFitness.Infrastructure.Persistence;

namespace WestCoastFitness.Api;

public sealed class PostgresReadyCheck : IHealthCheck
{
    private readonly IServiceScopeFactory _scopes;

    public PostgresReadyCheck(IServiceScopeFactory scopes) => _scopes = scopes;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var scope = _scopes.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<FitnessDbContext>();
            var ok = await db.Database.CanConnectAsync(cancellationToken);
            return ok
                ? HealthCheckResult.Healthy("PostgreSQL is reachable.")
                : HealthCheckResult.Unhealthy("PostgreSQL refused the connection.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("PostgreSQL health check failed.", ex);
        }
    }
}
