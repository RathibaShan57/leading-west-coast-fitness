using Microsoft.EntityFrameworkCore;
using WestCoastFitness.Application.Abstractions;
using WestCoastFitness.Domain.Bookings;
using WestCoastFitness.Domain.Classes;
using WestCoastFitness.Domain.Locations;
using WestCoastFitness.Domain.Members;
using WestCoastFitness.Domain.Memberships;
using WestCoastFitness.Domain.Trainers;

namespace WestCoastFitness.Infrastructure.Persistence;

public sealed class FitnessDbContext : DbContext, IUnitOfWork
{
    public FitnessDbContext(DbContextOptions<FitnessDbContext> options) : base(options)
    {
    }

    public DbSet<ClubLocation> Locations => Set<ClubLocation>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<MembershipPlan> Plans => Set<MembershipPlan>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<FitnessClass> Classes => Set<FitnessClass>();
    public DbSet<ClassSession> Sessions => Set<ClassSession>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("fitness");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitnessDbContext).Assembly);
    }
}
