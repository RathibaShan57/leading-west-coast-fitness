using WestCoastFitness.Domain.Bookings;
using WestCoastFitness.Domain.Classes;
using WestCoastFitness.Domain.Memberships;

namespace WestCoastFitness.Domain.Tests;

public class BookingPolicyTests
{
    private readonly BookingPolicy _sut = new();

    [Fact]
    public void Core_plan_waitlists_when_full()
    {
        var session = new ClassSession(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(1), 4);
        session.BookSeat();
        session.BookSeat();
        session.BookSeat();
        session.BookSeat();
        var status = _sut.Decide(session, PlanTier.Core, 1, 8);
        Assert.Equal(BookingStatus.Waitlisted, status);
    }

    [Fact]
    public void Core_plan_rejects_when_allotment_used()
    {
        var session = new ClassSession(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(1), 10);
        Assert.Throws<InvalidOperationException>(() => _sut.Decide(session, PlanTier.Core, 8, 8));
    }

    [Fact]
    public void Unlimited_ignores_allotment()
    {
        var session = new ClassSession(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(1), 10);
        Assert.Equal(BookingStatus.Confirmed, _sut.Decide(session, PlanTier.UnlimitedDesert, 40, 8));
    }

    [Fact]
    public void Late_cancel_is_penalised()
    {
        var start = DateTimeOffset.UtcNow.AddHours(3);
        Assert.False(_sut.CanCancelWithoutPenalty(start, DateTimeOffset.UtcNow));
    }
}
