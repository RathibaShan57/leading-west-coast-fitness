using FluentValidation;
using WestCoastFitness.Application.Abstractions;
using WestCoastFitness.Domain.Members;

namespace WestCoastFitness.Application.Members;

public sealed record RegisterMemberCommand(
    string FirstName,
    string LastName,
    string Email,
    DateOnly DateOfBirth,
    Guid HomeLocationId);

public sealed record MemberDto(
    Guid Id,
    string DisplayName,
    string Email,
    Guid HomeLocationId,
    bool WaiverSigned);

public sealed class RegisterMemberValidator : AbstractValidator<RegisterMemberCommand>
{
    public RegisterMemberValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(80);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.HomeLocationId).NotEmpty();
    }
}

public sealed class RegisterMemberHandler
{
    private readonly IMemberRepository _members;
    private readonly ILocationRepository _locations;
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _clock;

    public RegisterMemberHandler(
        IMemberRepository members,
        ILocationRepository locations,
        IUnitOfWork uow,
        TimeProvider clock)
    {
        _members = members;
        _locations = locations;
        _uow = uow;
        _clock = clock;
    }

    public async Task<MemberDto> HandleAsync(RegisterMemberCommand command, CancellationToken cancellationToken)
    {
        var location = await _locations.GetAsync(command.HomeLocationId, cancellationToken)
            ?? throw new InvalidOperationException("Home club was not found.");

        var existing = await _members.GetByEmailAsync(command.Email, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("A member with that email already exists.");
        }

        var member = new Member(
            command.FirstName,
            command.LastName,
            command.Email,
            command.DateOfBirth,
            location.Id,
            _clock.GetUtcNow());
        member.SignWaiver();
        await _members.AddAsync(member, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return new MemberDto(member.Id, member.DisplayName, member.Email, member.HomeLocationId, member.WaiverSigned);
    }
}
