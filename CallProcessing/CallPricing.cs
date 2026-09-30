namespace CallProcessing;

public static class CallPricing
{
    public static decimal CalculateCost(in CallRecord record)
    {
        decimal cost = record switch
        {
            _ when string.IsNullOrWhiteSpace(record.RecordId) => throw new ArgumentException("Invalid record ID."),

            _ when string.IsNullOrWhiteSpace(record.DestinationCountry) => throw new ArgumentException("Invalid country code."),

            _ when double.IsNaN(record.DurationMinutes) => throw new ArgumentException("Duration is NaN."),

            _ when double.IsInfinity(record.DurationMinutes) => throw new ArgumentException("Duration is infinite."),

            _ when record.DurationMinutes < 0 || record.DurationMinutes > 10000 => throw new ArgumentException("Invalid duration."),

            _ when record.IsRoaming && record.DestinationCountry == "KZ" && record.DurationMinutes < 1 => 50m,

            _ when !record.IsRoaming &&record.DestinationCountry == "KZ" => (decimal)record.DurationMinutes * 15m,

            _ when record.IsRoaming &&record.DurationMinutes >= 10 => (decimal)record.DurationMinutes * 120m,

            _ => (decimal)record.DurationMinutes * 45m
        };

        return decimal.Round(
            cost,
            2,
            MidpointRounding.AwayFromZero);
    }
}