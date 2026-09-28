using WestCoastFitness.Domain.Common;

namespace WestCoastFitness.Domain.Memberships;

public sealed class MembershipPlan : Entity
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public PlanTier Tier { get; private set; }
    public decimal MonthlyPriceUsd { get; private set; }
    public int IncludedClassesPerMonth { get; private set; }
    public bool IncludesGuestPass { get; private set; }

    private MembershipPlan()
    {
        Code = Name = string.Empty;
    }

    public MembershipPlan(
        string code,
        string name,
        PlanTier tier,
        decimal monthlyPriceUsd,
        int includedClassesPerMonth,
        bool includesGuestPass)
    {
        Code = Guard.Required(code, nameof(code)).ToUpperInvariant();
        Name = Guard.Required(name, nameof(name));
        Tier = tier;
        if (monthlyPriceUsd < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(monthlyPriceUsd));
        }

        MonthlyPriceUsd = monthlyPriceUsd;
        IncludedClassesPerMonth = includedClassesPerMonth;
        IncludesGuestPass = includesGuestPass;
    }
}
