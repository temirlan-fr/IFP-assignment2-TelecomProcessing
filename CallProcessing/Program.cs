using CallProcessing;

CallRecord call1 =
    new CallRecord("C001", "KZ", 4, false);

CallRecord call2 =
    new CallRecord("C002", "KZ", 0.5, true);

CallRecord call3 =
    new CallRecord("C003", "US", 10, true);

CallRecord call4 =
    new CallRecord("C004", "XX", 2, false);

CallRecord[] records =
{
    call1,
    call2,
    call3,
    call4
};

decimal oneCallCost =
    CallPricing.CalculateCost(in call1);

decimal sequentialResult =
    CallProcessor.ProcessCallsSequential(records);

decimal parallelResult =
    CallProcessor.ProcessCallsParallel(records);

Console.WriteLine($"One call cost: {oneCallCost}");
Console.WriteLine($"Sequential result: {sequentialResult}");
Console.WriteLine($"Parallel result: {parallelResult}");