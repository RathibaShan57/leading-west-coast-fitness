using WestCoastFitness.Application.Abstractions;
using WestCoastFitness.Application.Members;
using WestCoastFitness.Domain.Locations;
using WestCoastFitness.Domain.Members;

namespace WestCoastFitness.Application.Tests;

public class RegisterMemberHandlerTests
{
    [Fact]
    public async Task Registers_unique_email()
    {
        var location = new ClubLocation("Leading Camelback", "Phoenix", "85016", 0.022m, 36, true);
        var members = new InMemoryMembers();
        var locations = new InMemoryLocations(location);
        var handler = new RegisterMemberHandler(members, locations, new ImmediateUow(), TimeProvider.System);
        var dto = await handler.HandleAsync(
            new RegisterMemberCommand("Avery", "Quinn", "avery@example.com", new DateOnly(1994, 4, 12), location.Id),
            CancellationToken.None);
        Assert.Equal("Avery Quinn", dto.DisplayName);
        Assert.True(dto.WaiverSigned);
    }

    [Fact]
    public async Task Rejects_duplicate_email()
    {
        var location = new ClubLocation("Leading Camelback", "Phoenix", "85016", 0.022m, 36, true);
        var members = new InMemoryMembers();
        var locations = new InMemoryLocations(location);
        var handler = new RegisterMemberHandler(members, locations, new ImmediateUow(), TimeProvider.System);
        var cmd = new RegisterMemberCommand("Avery", "Quinn", "avery@example.com", new DateOnly(1994, 4, 12), location.Id);
        await handler.HandleAsync(cmd, CancellationToken.None);
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(cmd, CancellationToken.None));
    }

    private sealed class ImmediateUow : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => Task.FromResult(1);
    }

    private sealed class InMemoryLocations : ILocationRepository
    {
        private readonly ClubLocation _location;
        public InMemoryLocations(ClubLocation location) => _location = location;
        public Task<IReadOnlyList<ClubLocation>> ListAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<ClubLocation>>(new[] { _location });
        public Task<ClubLocation?> GetAsync(Guid id, CancellationToken cancellationToken) =>
            Task.FromResult(id == _location.Id ? _location : null);
    }

    private sealed class InMemoryMembers : IMemberRepository
    {
        private readonly List<Member> _items = new();
        public Task<Member?> GetAsync(Guid id, CancellationToken cancellationToken) =>
            Task.FromResult(_items.FirstOrDefault(x => x.Id == id));
        public Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
            Task.FromResult(_items.FirstOrDefault(x => x.Email == email.ToLowerInvariant()));
        public Task AddAsync(Member member, CancellationToken cancellationToken)
        {
            _items.Add(member);
            return Task.CompletedTask;
        }
        public Task<IReadOnlyList<Member>> ListAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<Member>>(_items);
    }
}
