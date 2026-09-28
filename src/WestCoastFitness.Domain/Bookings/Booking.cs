using WestCoastFitness.Domain.Common;

namespace WestCoastFitness.Domain.Bookings;

public enum BookingStatus
{
    Confirmed = 0,
    Waitlisted = 1,
    Cancelled = 2,
    CheckedIn = 3,
    NoShow = 4,
}

public sealed class Booking : Entity
{
    public Guid MemberId { get; private set; }
    public Guid SessionId { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? CheckedInAt { get; private set; }

    private Booking()
    {
    }

    public Booking(Guid memberId, Guid sessionId, BookingStatus status, DateTimeOffset createdAt)
    {
        MemberId = memberId;
        SessionId = sessionId;
        Status = status;
        CreatedAt = createdAt;
    }

    public void CheckIn(DateTimeOffset at)
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException("Only confirmed bookings can check in.");
        }

        Status = BookingStatus.CheckedIn;
        CheckedInAt = at;
    }

    public void Cancel()
    {
        if (Status is BookingStatus.CheckedIn or BookingStatus.NoShow)
        {
            throw new InvalidOperationException("Completed visits cannot be cancelled.");
        }

        Status = BookingStatus.Cancelled;
    }

    public void MarkNoShow()
    {
        if (Status == BookingStatus.Confirmed)
        {
            Status = BookingStatus.NoShow;
        }
    }
}
