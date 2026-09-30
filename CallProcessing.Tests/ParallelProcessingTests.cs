using System;
using CallProcessing;
using Xunit;

namespace CallProcessing.Tests;

public class ParallelProcessingTests
{
    [Fact]
    public void SequentialAndParallelResultsAreEqual()
    {
        CallRecord[] records = new CallRecord[1000];

        for (int i = 0; i < records.Length; i++)
        {
            if (i % 4 == 0)
            {
                records[i] =
                    new CallRecord($"C{i}", "KZ", 2, false);
            }
            else if (i % 4 == 1)
            {
                records[i] =
                    new CallRecord($"C{i}", "KZ", 0.5, true);
            }
            else if (i % 4 == 2)
            {
                records[i] =
                    new CallRecord($"C{i}", "US", 10, true);
            }
            else
            {
                records[i] =
                    new CallRecord($"C{i}", "XX", 2, false);
            }
        }

        CallRecord[] originalRecords =
            (CallRecord[])records.Clone();

        decimal sequentialResult =
            CallProcessor.ProcessCallsSequential(records);

        for (int i = 0; i < 100; i++)
        {
            decimal parallelResult =
                CallProcessor.ProcessCallsParallel(records);

            Assert.Equal(sequentialResult, parallelResult);
        }

        Assert.Equal(originalRecords, records);
    }

    [Fact]
    public void EmptyArrayReturnsZero()
    {
        CallRecord[] records = Array.Empty<CallRecord>();

        decimal result =
            CallProcessor.ProcessCallsParallel(records);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void OddLengthArrayIsRejected()
    {
        CallRecord[] records =
        {
            new CallRecord("C001", "KZ", 2, false)
        };

        Assert.Throws<ArgumentException>(() =>
            CallProcessor.ProcessCallsParallel(records));
    }
}