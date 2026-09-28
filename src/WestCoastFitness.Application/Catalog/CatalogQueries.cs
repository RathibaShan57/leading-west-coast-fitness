using WestCoastFitness.Application.Abstractions;
using WestCoastFitness.Domain.Memberships;

namespace WestCoastFitness.Application.Catalog;

public sealed record LocationDto(Guid Id, string Name, string City, string PostalCode, int StudioCapacity, bool OutdoorPatio);

public sealed record PlanQuoteDto(string Code, string Name, string Tier, decimal MonthlyBase, decimal QuotedPhoenixTotal);

public sealed record TrainerDto(Guid Id, string FullName, string Specialty, int YearsExperience);

public sealed record SessionDto(Guid Id, Guid FitnessClassId, DateTimeOffset StartsAt, int Remaining, bool Cancelled);

public sealed class CatalogQueries
{
    private readonly ILocationRepository _locations;
    private readonly IMembershipPlanRepository _plans;
    private readonly ITrainerRepository _trainers;
    private readonly ISessionRepository _sessions;
    private readonly IClassRepository _classes;
    private readonly ArizonaPricingService _pricing;
    private readonly TimeProvider _clock;

    public CatalogQueries(
        ILocationRepository locations,
        IMembershipPlanRepository plans,
        ITrainerRepository trainers,
        ISessionRepository sessions,
        IClassRepository classes,
        ArizonaPricingService pricing,
        TimeProvider clock)
    {
        _locations = locations;
        _plans = plans;
        _trainers = trainers;
        _sessions = sessions;
        _classes = classes;
        _pricing = pricing;
        _clock = clock;
    }

    public async Task<IReadOnlyList<LocationDto>> LocationsAsync(CancellationToken cancellationToken)
    {
        var rows = await _locations.ListAsync(cancellationToken);
        return rows.Select(l => new LocationDto(l.Id, l.Name, l.City, l.PostalCode, l.StudioCapacity, l.OutdoorPatio)).ToList();
    }

    public async Task<IReadOnlyList<PlanQuoteDto>> PlansAsync(CancellationToken cancellationToken)
    {
        var phoenixRate = 0.022m;
        var asOf = DateOnly.FromDateTime(_clock.GetUtcNow().UtcDateTime);
        var rows = await _plans.ListAsync(cancellationToken);
        return rows.Select(p => new PlanQuoteDto(
            p.Code,
            p.Name,
            p.Tier.ToString(),
            p.MonthlyPriceUsd,
            _pricing.QuoteMonthlyTotal(p.MonthlyPriceUsd, phoenixRate, p.Tier == PlanTier.UnlimitedDesert, asOf))).ToList();
    }

    public Task<IReadOnlyList<TrainerDto>> TrainersAsync(CancellationToken cancellationToken) =>
        _trainers.ListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<TrainerDto>)t.Result
                .Select(x => new TrainerDto(x.Id, x.FullName, x.Specialty, x.YearsExperience))
                .ToList(), cancellationToken);

    public async Task<IReadOnlyList<SessionDto>> UpcomingAsync(CancellationToken cancellationToken)
    {
        var rows = await _sessions.UpcomingAsync(_clock.GetUtcNow(), cancellationToken);
        return rows.Select(s => new SessionDto(s.Id, s.FitnessClassId, s.StartsAt, s.Remaining, s.Cancelled)).ToList();
    }

    public async Task<int> ClassCountAsync(CancellationToken cancellationToken)
    {
        var rows = await _classes.ListAsync(cancellationToken);
        return rows.Count;
    }
}
