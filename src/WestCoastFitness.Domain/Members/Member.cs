using WestCoastFitness.Domain.Common;

namespace WestCoastFitness.Domain.Members;

public sealed class Member : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Guid HomeLocationId { get; private set; }
    public DateTimeOffset JoinedAt { get; private set; }
    public bool WaiverSigned { get; private set; }

    private Member()
    {
        FirstName = LastName = Email = string.Empty;
    }

    public Member(
        string firstName,
        string lastName,
        string email,
        DateOnly dateOfBirth,
        Guid homeLocationId,
        DateTimeOffset joinedAt)
    {
        FirstName = Guard.Required(firstName, nameof(firstName));
        LastName = Guard.Required(lastName, nameof(lastName));
        Email = Guard.Required(email, nameof(email)).ToLowerInvariant();
        if (!Email.Contains('@'))
        {
            throw new ArgumentException("Email must include an @.", nameof(email));
        }

        if (dateOfBirth > DateOnly.FromDateTime(joinedAt.UtcDateTime.Date.AddYears(-14)))
        {
            throw new ArgumentException("Members must be at least 14 years old.", nameof(dateOfBirth));
        }

        DateOfBirth = dateOfBirth;
        HomeLocationId = homeLocationId;
        JoinedAt = joinedAt;
    }

    public string DisplayName => $"{FirstName} {LastName}";

    public void SignWaiver() => WaiverSigned = true;
}
