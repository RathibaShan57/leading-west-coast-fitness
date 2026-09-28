using WestCoastFitness.Domain.Classes;

namespace WestCoastFitness.Domain.Tests;

public class HeatAdvisoryPolicyTests
{
    [Fact]
    public void Indoor_classes_are_never_cancelled_for_heat()
    {
        Assert.False(HeatAdvisoryPolicy.MustCancelOutdoor(false, 118, 5));
    }

    [Fact]
    public void Outdoor_cancels_at_one_ten()
    {
        Assert.True(HeatAdvisoryPolicy.MustCancelOutdoor(true, 110, 2));
    }

    [Fact]
    public void High_intensity_cancels_at_one_oh_five()
    {
        Assert.True(HeatAdvisoryPolicy.MustCancelOutdoor(true, 105, 4));
        Assert.False(HeatAdvisoryPolicy.MustCancelOutdoor(true, 105, 2));
    }
}
