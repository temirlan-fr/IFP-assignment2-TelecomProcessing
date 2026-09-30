using CallProcessing;
using Xunit;

namespace CallProcessing.Tests;

public class ValidationTests
{
    [Fact]
    public void BlankRecordIdIsRejected()
    {
        Assert.Throws<ArgumentException>(() =>
            new CallRecord("", "KZ", 2, false));
    }

    [Fact]
    public void BlankCountryIsRejected()
    {
        Assert.Throws<ArgumentException>(() =>
            new CallRecord("C001", "", 2, false));
    }

    [Fact]
    public void NegativeDurationIsRejected()
    {
        Assert.Throws<ArgumentException>(() =>
            new CallRecord("C001", "KZ", -1, false));
    }

    [Fact]
    public void NaNDurationIsRejected()
    {
        Assert.Throws<ArgumentException>(() =>
            new CallRecord("C001", "KZ", double.NaN, false));
    }

    [Fact]
    public void InfiniteDurationIsRejected()
    {
        Assert.Throws<ArgumentException>(() =>
            new CallRecord(
                "C001",
                "KZ",
                double.PositiveInfinity,
                false));
    }

    [Fact]
    public void DurationAbove10000IsRejected()
    {
        Assert.Throws<ArgumentException>(() =>
            new CallRecord("C001", "KZ", 10001, false));
    }

    [Fact]
    public void DefaultRecordIsRejected()
    {
        CallRecord record = default;

        Assert.Throws<ArgumentException>(() =>
            CallPricing.CalculateCost(in record));
    }

    [Fact]
    public void Duration999UsesShortRoamingTariff()
    {
        CallRecord record =
            new CallRecord("C001", "KZ", 0.999, true);

        decimal result =
            CallPricing.CalculateCost(in record);

        Assert.Equal(50.00m, result);
    }

    [Fact]
    public void Duration1UsesNormalFallbackTariff()
    {
        CallRecord record =
            new CallRecord("C001", "KZ", 1.0, true);

        decimal result =
            CallPricing.CalculateCost(in record);

        Assert.Equal(45.00m, result);
    }

    [Fact]
    public void ZeroMinuteNonRoamingCallIsValid()
    {
        CallRecord record =
            new CallRecord("C001", "US", 0, false);

        decimal result =
            CallPricing.CalculateCost(in record);

        Assert.Equal(0.00m, result);
    }
}