using WestCoastFitness.Domain.Common;

namespace WestCoastFitness.Domain.Trainers;

public sealed class Trainer : Entity
{
    public string FullName { get; private set; }
    public string Specialty { get; private set; }
    public int YearsExperience { get; private set; }
    public Guid HomeLocationId { get; private set; }

    private Trainer()
    {
        FullName = Specialty = string.Empty;
    }

    public Trainer(string fullName, string specialty, int yearsExperience, Guid homeLocationId)
    {
        FullName = Guard.Required(fullName, nameof(fullName));
        Specialty = Guard.Required(specialty, nameof(specialty));
        if (yearsExperience < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(yearsExperience));
        }

        YearsExperience = yearsExperience;
        HomeLocationId = homeLocationId;
    }
}
