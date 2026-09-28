using WestCoastFitness.Domain.Classes;
using WestCoastFitness.Domain.Memberships;

namespace WestCoastFitness.Api.Tests;

public class DomainSmokeThroughApiAssemblyTests
{
    [Fact]
    public void Heat_policy_is_reachable_from_api_test_host()
    {
        Assert.True(HeatAdvisoryPolicy.MustCancelOutdoor(true, 112, 3));
    }

    [Fact]
    public void Pricing_service_is_deterministic()
    {
        var pricing = new ArizonaPricingService();
        Assert.Equal(pricing.QuoteMonthlyTotal(89m, 0.022m, false, new DateOnly(2026, 2, 1)),
            pricing.QuoteMonthlyTotal(89m, 0.022m, false, new DateOnly(2026, 2, 1)));
    }
}
