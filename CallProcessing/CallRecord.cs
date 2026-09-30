namespace CallProcessing;

public readonly record struct CallRecord
{
    public string RecordId { get; }
    public string DestinationCountry { get; }
    public double DurationMinutes { get; }
    public bool IsRoaming { get; }

    public CallRecord(
        string recordId,
        string destinationCountry,
        double durationMinutes,
        bool isRoaming)
    {
        if (string.IsNullOrWhiteSpace(recordId))
            throw new ArgumentException("RecordId cannot be empty.");

        if (string.IsNullOrWhiteSpace(destinationCountry))
            throw new ArgumentException("Country code cannot be empty.");

        if (double.IsNaN(durationMinutes))
            throw new ArgumentException("Duration cannot be NaN.");

        if (double.IsInfinity(durationMinutes))
            throw new ArgumentException("Duration cannot be infinite.");

        if (durationMinutes < 0 || durationMinutes > 10000)
            throw new ArgumentException(
                "Duration must be between 0 and 10000.");

        RecordId = recordId;
        DestinationCountry = destinationCountry;
        DurationMinutes = durationMinutes;
        IsRoaming = isRoaming;
    }
}