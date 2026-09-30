using CallProcessing;
using Xunit;

namespace CallProcessing.Tests;

public class TariffTests
{
    [Fact]
    public void KzNonRoamingCostIsCorrect()
    {
        CallRecord call =
            new CallRecord("C001", "KZ", 4, false);

        decimal result =
            CallPricing.CalculateCost(in call);

        Assert.Equal(60.00m, result);
    }

    [Fact]
    public void KzRoamingShortCallUsesFlatFee()
    {
        CallRecord call =
            new CallRecord("C002", "KZ", 0.5, true);

        decimal result =
            CallPricing.CalculateCost(in call);

        Assert.Equal(50.00m, result);
    }

    [Fact]
    public void RoamingLongCallUses120Rate()
    {
        CallRecord call =
            new CallRecord("C003", "US", 10, true);

        decimal result =
            CallPricing.CalculateCost(in call);

        Assert.Equal(1200.00m, result);
    }

    [Fact]
    public void UnknownCountryUsesFallbackRate()
    {
        CallRecord call =
            new CallRecord("C004", "XX", 2, false);

        decimal result =
            CallPricing.CalculateCost(in call);

        Assert.Equal(90.00m, result);
    }

    [Fact]
    public void ZeroMinuteNonRoamingCallCostsZero()
    {
        CallRecord call =
            new CallRecord("C005", "US", 0, false);

        decimal result =
            CallPricing.CalculateCost(in call);

        Assert.Equal(0.00m, result);
    }
}