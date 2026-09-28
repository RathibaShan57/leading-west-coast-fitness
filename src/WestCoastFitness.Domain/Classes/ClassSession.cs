using WestCoastFitness.Domain.Common;

namespace WestCoastFitness.Domain.Classes;

public sealed class ClassSession : Entity
{
    public Guid FitnessClassId { get; private set; }
    public Guid LocationId { get; private set; }
    public DateTimeOffset StartsAt { get; private set; }
    public int Capacity { get; private set; }
    public int BookedCount { get; private set; }
    public bool Cancelled { get; private set; }
    public string? CancellationReason { get; private set; }

    private ClassSession()
    {
    }

    public ClassSession(
        Guid fitnessClassId,
        Guid locationId,
        DateTimeOffset startsAt,
        int capacity)
    {
        FitnessClassId = fitnessClassId;
        LocationId = locationId;
        StartsAt = startsAt;
        if (capacity < 4)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        Capacity = capacity;
    }

    public int Remaining => Math.Max(0, Capacity - BookedCount);

    public void BookSeat()
    {
        if (Cancelled)
        {
            throw new InvalidOperationException("Session was cancelled.");
        }

        if (Remaining == 0)
        {
            throw new InvalidOperationException("Session is full.");
        }

        BookedCount++;
    }

    public void ReleaseSeat()
    {
        if (BookedCount > 0)
        {
            BookedCount--;
        }
    }

    public void Cancel(string reason)
    {
        Cancelled = true;
        CancellationReason = Guard.Required(reason, nameof(reason));
    }
}
