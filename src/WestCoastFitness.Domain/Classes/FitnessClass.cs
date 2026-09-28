using WestCoastFitness.Domain.Common;

namespace WestCoastFitness.Domain.Classes;

public sealed class FitnessClass : Entity
{
    public string Title { get; private set; }
    public string Discipline { get; private set; }
    public int DurationMinutes { get; private set; }
    public int Intensity { get; private set; }
    public bool Outdoor { get; private set; }
    public Guid LocationId { get; private set; }
    public Guid TrainerId { get; private set; }

    private FitnessClass()
    {
        Title = Discipline = string.Empty;
    }

    public FitnessClass(
        string title,
        string discipline,
        int durationMinutes,
        int intensity,
        bool outdoor,
        Guid locationId,
        Guid trainerId)
    {
        Title = Guard.Required(title, nameof(title));
        Discipline = Guard.Required(discipline, nameof(discipline));
        if (durationMinutes is < 20 or > 120)
        {
            throw new ArgumentOutOfRangeException(nameof(durationMinutes));
        }

        if (intensity is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(intensity));
        }

        DurationMinutes = durationMinutes;
        Intensity = intensity;
        Outdoor = outdoor;
        LocationId = locationId;
        TrainerId = trainerId;
    }
}
