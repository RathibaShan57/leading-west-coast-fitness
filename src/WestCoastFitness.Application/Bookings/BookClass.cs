using FluentValidation;
using WestCoastFitness.Application.Abstractions;
using WestCoastFitness.Domain.Bookings;
using WestCoastFitness.Domain.Memberships;

namespace WestCoastFitness.Application.Bookings;

public sealed record BookClassCommand(Guid MemberId, Guid SessionId, string PlanCode);

public sealed record BookingDto(Guid Id, Guid MemberId, Guid SessionId, string Status);

public sealed class BookClassValidator : AbstractValidator<BookClassCommand>
{
    public BookClassValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty();
        RuleFor(x => x.SessionId).NotEmpty();
        RuleFor(x => x.PlanCode).NotEmpty();
    }
}

public sealed class BookClassHandler
{
    private readonly IMemberRepository _members;
    private readonly ISessionRepository _sessions;
    private readonly IMembershipPlanRepository _plans;
    private readonly IBookingRepository _bookings;
    private readonly IUnitOfWork _uow;
    private readonly BookingPolicy _policy;
    private readonly TimeProvider _clock;

    public BookClassHandler(
        IMemberRepository members,
        ISessionRepository sessions,
        IMembershipPlanRepository plans,
        IBookingRepository bookings,
        IUnitOfWork uow,
        BookingPolicy policy,
        TimeProvider clock)
    {
        _members = members;
        _sessions = sessions;
        _plans = plans;
        _bookings = bookings;
        _uow = uow;
        _policy = policy;
        _clock = clock;
    }

    public async Task<BookingDto> HandleAsync(BookClassCommand command, CancellationToken cancellationToken)
    {
        var member = await _members.GetAsync(command.MemberId, cancellationToken)
            ?? throw new InvalidOperationException("Member was not found.");
        if (!member.WaiverSigned)
        {
            throw new InvalidOperationException("Waiver must be signed before booking.");
        }

        var session = await _sessions.GetAsync(command.SessionId, cancellationToken)
            ?? throw new InvalidOperationException("Session was not found.");
        var plan = await _plans.GetByCodeAsync(command.PlanCode, cancellationToken)
            ?? throw new InvalidOperationException("Membership plan was not found.");

        var monthStart = new DateTimeOffset(
            _clock.GetUtcNow().Year,
            _clock.GetUtcNow().Month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero);
        var used = await _bookings.CountConfirmedThisMonthAsync(member.Id, monthStart, cancellationToken);
        var status = _policy.Decide(session, plan.Tier, used, plan.IncludedClassesPerMonth);
        if (status == BookingStatus.Confirmed)
        {
            session.BookSeat();
        }

        var booking = new Booking(member.Id, session.Id, status, _clock.GetUtcNow());
        await _bookings.AddAsync(booking, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return new BookingDto(booking.Id, booking.MemberId, booking.SessionId, booking.Status.ToString());
    }
}
