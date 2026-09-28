using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WestCoastFitness.Domain.Bookings;
using WestCoastFitness.Domain.Classes;
using WestCoastFitness.Domain.Locations;
using WestCoastFitness.Domain.Members;
using WestCoastFitness.Domain.Memberships;
using WestCoastFitness.Domain.Trainers;

namespace WestCoastFitness.Infrastructure.Persistence;

internal sealed class ClubLocationConfiguration : IEntityTypeConfiguration<ClubLocation>
{
    public void Configure(EntityTypeBuilder<ClubLocation> builder)
    {
        builder.ToTable("locations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.Property(x => x.City).HasMaxLength(80).IsRequired();
        builder.Property(x => x.State).HasMaxLength(2).IsRequired();
        builder.Property(x => x.PostalCode).HasMaxLength(16).IsRequired();
        builder.Property(x => x.TimeZone).HasMaxLength(64).IsRequired();
        builder.Property(x => x.CityPrivilegeTaxRate).HasPrecision(5, 4);
    }
}

internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("members");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.FirstName).HasMaxLength(80).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(80).IsRequired();
    }
}

internal sealed class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
{
    public void Configure(EntityTypeBuilder<MembershipPlan> builder)
    {
        builder.ToTable("plans");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(32).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.MonthlyPriceUsd).HasPrecision(10, 2);
    }
}

internal sealed class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.ToTable("trainers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FullName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Specialty).HasMaxLength(80).IsRequired();
    }
}

internal sealed class FitnessClassConfiguration : IEntityTypeConfiguration<FitnessClass>
{
    public void Configure(EntityTypeBuilder<FitnessClass> builder)
    {
        builder.ToTable("classes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Discipline).HasMaxLength(80).IsRequired();
    }
}

internal sealed class ClassSessionConfiguration : IEntityTypeConfiguration<ClassSession>
{
    public void Configure(EntityTypeBuilder<ClassSession> builder)
    {
        builder.ToTable("sessions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CancellationReason).HasMaxLength(240);
    }
}

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(24);
    }
}
