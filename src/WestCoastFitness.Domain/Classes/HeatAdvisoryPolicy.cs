namespace WestCoastFitness.Domain.Classes;

/// <summary>
/// Arizona summer outdoor-class policy. NWS excessive-heat warnings typically
/// start around 110 F in the Phoenix metro.
/// </summary>
public static class HeatAdvisoryPolicy
{
    public const int CancelOutdoorAtFahrenheit = 110;

    public static bool MustCancelOutdoor(bool outdoor, int forecastHighF, int intensity)
    {
        if (!outdoor)
        {
            return false;
        }

        if (forecastHighF >= CancelOutdoorAtFahrenheit)
        {
            return true;
        }

        return forecastHighF >= 105 && intensity >= 4;
    }
}
