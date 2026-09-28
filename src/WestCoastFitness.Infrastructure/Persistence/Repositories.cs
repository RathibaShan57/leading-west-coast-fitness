using Microsoft.EntityFrameworkCore;
using WestCoastFitness.Application.Abstractions;
using WestCoastFitness.Domain.Bookings;
using WestCoastFitness.Domain.Classes;
using WestCoastFitness.Domain.Locations;
using WestCoastFitness.Domain.Members;
using WestCoastFitness.Domain.Memberships;
using WestCoastFitness.Domain.Trainers;

namespace WestCoastFitness.Infrastructure.Persistence;

public sealed class LocationRepository : ILocationRepository
{
    private readonly FitnessDbContext _db;
    public LocationRepository(FitnessDbContext db) => _db = db;
    public Task<IReadOnlyList<ClubLocation>> ListAsync(CancellationToken cancellationToken) =>
        _db.Locations.AsNoTracking().OrderBy(x => x.City).ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<ClubLocation>)t.Result, cancellationToken);
    public Task<ClubLocation?> GetAsync(Guid id, CancellationToken cancellationToken) =>
        _db.Locations.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}

public sealed class MemberRepository : IMemberRepository
{
    private readonly FitnessDbContext _db;
    public MemberRepository(FitnessDbContext db) => _db = db;
    public Task<Member?> GetAsync(Guid id, CancellationToken cancellationToken) =>
        _db.Members.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    public Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        _db.Members.FirstOrDefaultAsync(x => x.Email == email.ToLowerInvariant(), cancellationToken);
    public async Task AddAsync(Member member, CancellationToken cancellationToken) =>
        await _db.Members.AddAsync(member, cancellationToken);
    public Task<IReadOnlyList<Member>> ListAsync(CancellationToken cancellationToken) =>
        _db.Members.AsNoTracking().OrderBy(x => x.LastName).ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<Member>)t.Result, cancellationToken);
}

public sealed class MembershipPlanRepository : IMembershipPlanRepository
{
    private readonly FitnessDbContext _db;
    public MembershipPlanRepository(FitnessDbContext db) => _db = db;
    public Task<IReadOnlyList<MembershipPlan>> ListAsync(CancellationToken cancellationToken) =>
        _db.Plans.AsNoTracking().OrderBy(x => x.MonthlyPriceUsd).ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<MembershipPlan>)t.Result, cancellationToken);
    public Task<MembershipPlan?> GetByCodeAsync(string code, CancellationToken cancellationToken) =>
        _db.Plans.FirstOrDefaultAsync(x => x.Code == code.ToUpperInvariant(), cancellationToken);
}

public sealed class ClassRepository : IClassRepository
{
    private readonly FitnessDbContext _db;
    public ClassRepository(FitnessDbContext db) => _db = db;
    public Task<IReadOnlyList<FitnessClass>> ListAsync(CancellationToken cancellationToken) =>
        _db.Classes.AsNoTracking().ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<FitnessClass>)t.Result, cancellationToken);
    public Task<FitnessClass?> GetAsync(Guid id, CancellationToken cancellationToken) =>
        _db.Classes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}

public sealed class SessionRepository : ISessionRepository
{
    private readonly FitnessDbContext _db;
    public SessionRepository(FitnessDbContext db) => _db = db;
    public Task<ClassSession?> GetAsync(Guid id, CancellationToken cancellationToken) =>
        _db.Sessions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    public Task<IReadOnlyList<ClassSession>> UpcomingAsync(DateTimeOffset from, CancellationToken cancellationToken) =>
        _db.Sessions.AsNoTracking().Where(x => x.StartsAt >= from).OrderBy(x => x.StartsAt).Take(50)
            .ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<ClassSession>)t.Result, cancellationToken);
}

public sealed class BookingRepository : IBookingRepository
{
    private readonly FitnessDbContext _db;
    public BookingRepository(FitnessDbContext db) => _db = db;
    public async Task AddAsync(Booking booking, CancellationToken cancellationToken) =>
        await _db.Bookings.AddAsync(booking, cancellationToken);
    public Task<int> CountConfirmedThisMonthAsync(Guid memberId, DateTimeOffset monthStart, CancellationToken cancellationToken)
    {
        var monthEnd = monthStart.AddMonths(1);
        return _db.Bookings.CountAsync(
            x => x.MemberId == memberId
                && x.CreatedAt >= monthStart
                && x.CreatedAt < monthEnd
                && (x.Status == BookingStatus.Confirmed || x.Status == BookingStatus.CheckedIn),
            cancellationToken);
    }
    public Task<IReadOnlyList<Booking>> ForMemberAsync(Guid memberId, CancellationToken cancellationToken) =>
        _db.Bookings.AsNoTracking().Where(x => x.MemberId == memberId).ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<Booking>)t.Result, cancellationToken);
}

public sealed class TrainerRepository : ITrainerRepository
{
    private readonly FitnessDbContext _db;
    public TrainerRepository(FitnessDbContext db) => _db = db;
    public Task<IReadOnlyList<Trainer>> ListAsync(CancellationToken cancellationToken) =>
        _db.Trainers.AsNoTracking().OrderBy(x => x.FullName).ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<Trainer>)t.Result, cancellationToken);
}
