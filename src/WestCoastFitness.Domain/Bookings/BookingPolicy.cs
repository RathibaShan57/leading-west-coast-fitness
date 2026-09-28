using WestCoastFitness.Domain.Classes;
using WestCoastFitness.Domain.Memberships;

namespace WestCoastFitness.Domain.Bookings;

public sealed class BookingPolicy
{
    public BookingStatus Decide(ClassSession session, PlanTier tier, int classesUsedThisMonth, int included)
    {
        if (session.Cancelled)
        {
            throw new InvalidOperationException("Cannot book a cancelled session.");
        }

        if (tier == PlanTier.DropIn)
        {
            return session.Remaining > 0 ? BookingStatus.Confirmed : BookingStatus.Waitlisted;
        }

        if (tier != PlanTier.UnlimitedDesert && classesUsedThisMonth >= included)
        {
            throw new InvalidOperationException("Monthly class allotment is exhausted.");
        }

        return session.Remaining > 0 ? BookingStatus.Confirmed : BookingStatus.Waitlisted;
    }

    public bool CanCancelWithoutPenalty(DateTimeOffset sessionStart, DateTimeOffset now)
    {
        return sessionStart - now >= TimeSpan.FromHours(8);
    }
}
