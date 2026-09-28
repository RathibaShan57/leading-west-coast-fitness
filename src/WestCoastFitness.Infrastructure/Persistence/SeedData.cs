using WestCoastFitness.Domain.Classes;
using WestCoastFitness.Domain.Locations;
using WestCoastFitness.Domain.Memberships;
using WestCoastFitness.Domain.Trainers;

namespace WestCoastFitness.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task EnsureSeededAsync(FitnessDbContext db, CancellationToken cancellationToken)
    {
        if (await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AnyAsync(db.Locations, cancellationToken))
        {
            return;
        }

        var phoenix = new ClubLocation("Leading Camelback", "Phoenix", "85016", 0.0220m, 36, true);
        var scottsdale = new ClubLocation("Leading Old Town", "Scottsdale", "85251", 0.0165m, 28, true);
        var tempe = new ClubLocation("Leading Mill Avenue", "Tempe", "85281", 0.0180m, 32, false);
        var tucson = new ClubLocation("Leading Foothills", "Tucson", "85718", 0.0260m, 24, true);
        var flagstaff = new ClubLocation("Leading Pine Ridge", "Flagstaff", "86001", 0.0213m, 20, true);
        db.Locations.AddRange(phoenix, scottsdale, tempe, tucson, flagstaff);

        db.Plans.AddRange(
            new MembershipPlan("DROP", "Drop-in desert day", PlanTier.DropIn, 28m, 1, false),
            new MembershipPlan("CORE", "Core 8", PlanTier.Core, 89m, 8, false),
            new MembershipPlan("PEAK", "Peak 16", PlanTier.Peak, 129m, 16, true),
            new MembershipPlan("UNLT", "Unlimited Sonoran", PlanTier.UnlimitedDesert, 179m, 999, true));

        var riley = new Trainer("Riley Navarro", "Strength + desert hiking", 9, phoenix.Id);
        var amira = new Trainer("Amira Solano", "Yoga + heat recovery", 6, scottsdale.Id);
        var devon = new Trainer("Devon Hale", "HIIT + indoor cycling", 11, tempe.Id);
        db.Trainers.AddRange(riley, amira, devon);

        var sunriseHike = new FitnessClass("Camelback sunrise hike", "Outdoor", 75, 4, true, phoenix.Id, riley.Id);
        var hotYoga = new FitnessClass("Scottsdale hot yoga", "Yoga", 60, 3, false, scottsdale.Id, amira.Id);
        var millRide = new FitnessClass("Mill Avenue endurance ride", "Cycling", 45, 5, false, tempe.Id, devon.Id);
        db.Classes.AddRange(sunriseHike, hotYoga, millRide);

        var start = DateTimeOffset.UtcNow.AddDays(1).ToOffset(TimeSpan.FromHours(-7));
        db.Sessions.AddRange(
            new ClassSession(sunriseHike.Id, phoenix.Id, start.Date.AddHours(5.5), 12),
            new ClassSession(hotYoga.Id, scottsdale.Id, start.Date.AddHours(18), 18),
            new ClassSession(millRide.Id, tempe.Id, start.Date.AddHours(6), 20));

        await db.SaveChangesAsync(cancellationToken);
    }
}
