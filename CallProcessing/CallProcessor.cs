using System;
using System.Threading;

namespace CallProcessing;

public static class CallProcessor
{
    public static decimal ProcessCallsSequential(
        CallRecord[] records)
    {
        if (records == null)
            throw new ArgumentNullException(nameof(records));

        decimal total = 0;

        foreach (CallRecord record in records)
        {
            total += CallPricing.CalculateCost(in record);
        }

        return total;
    }

    public static decimal ProcessCallsParallel(
        CallRecord[] records)
    {
        if (records == null)
            throw new ArgumentNullException(nameof(records));

        if (records.Length == 0)
            return 0;

        if (records.Length % 2 != 0)
            throw new ArgumentException(
                "Array length must be even.");

        int middle = records.Length / 2;

        CallRecord[] firstPart = records[..middle];
        CallRecord[] secondPart = records[middle..];

        decimal[] firstResults =
            new decimal[firstPart.Length];

        decimal[] secondResults =
            new decimal[secondPart.Length];

        Exception? firstError = null;
        Exception? secondError = null;

        Thread firstThread = new Thread(() =>
        {
            try
            {
                for (int i = 0; i < firstPart.Length; i++)
                {
                    firstResults[i] =
                        CallPricing.CalculateCost(in firstPart[i]);
                }
            }
            catch (Exception error)
            {
                firstError = error;
            }
        });

        Thread secondThread = new Thread(() =>
        {
            try
            {
                for (int i = 0; i < secondPart.Length; i++)
                {
                    secondResults[i] =
                        CallPricing.CalculateCost(in secondPart[i]);
                }
            }
            catch (Exception error)
            {
                secondError = error;
            }
        });

        firstThread.Start();
        secondThread.Start();

        firstThread.Join();
        secondThread.Join();

        if (firstError != null)
            throw firstError;

        if (secondError != null)
            throw secondError;

        decimal total = 0;

        foreach (decimal result in firstResults)
        {
            total += result;
        }

        foreach (decimal result in secondResults)
        {
            total += result;
        }

        return total;
    }
}