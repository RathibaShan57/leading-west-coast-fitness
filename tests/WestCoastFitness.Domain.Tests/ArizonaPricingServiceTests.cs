using WestCoastFitness.Domain.Memberships;

namespace WestCoastFitness.Domain.Tests;

public class ArizonaPricingServiceTests
{
    private readonly ArizonaPricingService _sut = new();

    [Fact]
    public void Quote_applies_state_and_city_tax()
    {
        var total = _sut.QuoteMonthlyTotal(100m, 0.022m, outdoorPlan: false, new DateOnly(2026, 1, 15));
        Assert.Equal(107.80m, total);
    }

    [Fact]
    public void Quote_applies_summer_outdoor_discount()
    {
        var winter = _sut.QuoteMonthlyTotal(179m, 0.022m, true, new DateOnly(2026, 1, 15));
        var summer = _sut.QuoteMonthlyTotal(179m, 0.022m, true, new DateOnly(2026, 7, 4));
        Assert.True(summer < winter);
    }

    [Theory]
    [InlineData(6, 0, true)]
    [InlineData(12, 0, false)]
    [InlineData(17, 30, true)]
    public void Peak_hour_window(int hour, int minute, bool expected)
    {
        Assert.Equal(expected, _sut.IsPeakHour(new TimeOnly(hour, minute)));
    }

    [Fact]
    public void Peak_plan_has_no_premium()
    {
        Assert.Equal(0m, _sut.PeakClassPremium(PlanTier.Peak, new TimeOnly(6, 0)));
        Assert.Equal(8m, _sut.PeakClassPremium(PlanTier.DropIn, new TimeOnly(6, 0)));
    }
}
