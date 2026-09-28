using WestCoastFitness.Domain.Bookings;
using WestCoastFitness.Domain.Classes;
using WestCoastFitness.Domain.Locations;
using WestCoastFitness.Domain.Members;
using WestCoastFitness.Domain.Memberships;
using WestCoastFitness.Domain.Trainers;

namespace WestCoastFitness.Application.Abstractions;

public interface ILocationRepository
{
    Task<IReadOnlyList<ClubLocation>> ListAsync(CancellationToken cancellationToken);
    Task<ClubLocation?> GetAsync(Guid id, CancellationToken cancellationToken);
}

public interface IMemberRepository
{
    Task<Member?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(Member member, CancellationToken cancellationToken);
    Task<IReadOnlyList<Member>> ListAsync(CancellationToken cancellationToken);
}

public interface IMembershipPlanRepository
{
    Task<IReadOnlyList<MembershipPlan>> ListAsync(CancellationToken cancellationToken);
    Task<MembershipPlan?> GetByCodeAsync(string code, CancellationToken cancellationToken);
}

public interface IClassRepository
{
    Task<IReadOnlyList<FitnessClass>> ListAsync(CancellationToken cancellationToken);
    Task<FitnessClass?> GetAsync(Guid id, CancellationToken cancellationToken);
}

public interface ISessionRepository
{
    Task<ClassSession?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ClassSession>> UpcomingAsync(DateTimeOffset from, CancellationToken cancellationToken);
}

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken cancellationToken);
    Task<int> CountConfirmedThisMonthAsync(Guid memberId, DateTimeOffset monthStart, CancellationToken cancellationToken);
    Task<IReadOnlyList<Booking>> ForMemberAsync(Guid memberId, CancellationToken cancellationToken);
}

public interface ITrainerRepository
{
    Task<IReadOnlyList<Trainer>> ListAsync(CancellationToken cancellationToken);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
