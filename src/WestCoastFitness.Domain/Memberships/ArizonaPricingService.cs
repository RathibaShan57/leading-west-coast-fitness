namespace WestCoastFitness.Domain.Memberships;

/// <summary>
/// Arizona transaction privilege tax (state 5.6%) plus city privilege tax.
/// Phoenix metro clubs also apply a summer heat-surcharge waiver in June–August
/// for outdoor-only plans.
/// </summary>
public sealed class ArizonaPricingService
{
    public const decimal StateTransactionPrivilegeRate = 0.056m;

    public decimal QuoteMonthlyTotal(decimal basePrice, decimal cityRate, bool outdoorPlan, DateOnly asOf)
    {
        if (basePrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(basePrice));
        }

        var taxable = basePrice;
        if (outdoorPlan && asOf.Month is >= 6 and <= 8)
        {
            taxable = decimal.Round(basePrice * 0.92m, 2, MidpointRounding.AwayFromZero);
        }

        var combined = StateTransactionPrivilegeRate + cityRate;
        return decimal.Round(taxable * (1 + combined), 2, MidpointRounding.AwayFromZero);
    }

    public bool IsPeakHour(TimeOnly startLocal)
    {
        return startLocal >= new TimeOnly(5, 30) && startLocal < new TimeOnly(8, 0)
            || startLocal >= new TimeOnly(17, 0) && startLocal < new TimeOnly(19, 30);
    }

    public decimal PeakClassPremium(PlanTier tier, TimeOnly startLocal)
    {
        if (!IsPeakHour(startLocal))
        {
            return 0m;
        }

        return tier switch
        {
            PlanTier.DropIn => 8m,
            PlanTier.Core => 4m,
            PlanTier.Peak => 0m,
            PlanTier.UnlimitedDesert => 0m,
            _ => 6m,
        };
    }
}
