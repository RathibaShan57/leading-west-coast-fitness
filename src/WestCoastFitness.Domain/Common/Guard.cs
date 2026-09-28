namespace WestCoastFitness.Domain.Common;

public static class Guard
{
    public static string Required(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{name} is required.", name);
        }

        return value.Trim();
    }

    public static T NotNull<T>(T? value, string name) where T : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }

        return value;
    }
}
