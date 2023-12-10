using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex NumberRegex = new(@"\d+");

    private static void Main(string[] args)
    {
        const int day = 6;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);
        var times = streamReader.ReadLine()!
            .Replace(" ", null)
            .GetNumbersLong()
            .ToArray();
        var distances = streamReader.ReadLine()!
            .Replace(" ", null)
            .GetNumbersLong()
            .ToArray();

        var result = 1L;
        for (int i = 0; i < times.Length; i++)
        {
            result *= GetNumberOfWaysToWin(times[i], distances[i]);
        }
        System.Console.WriteLine(result);
    }

    private static long GetNumberOfWaysToWin(long time, long distanceToBeat)
    {
        var buttonTime = time / 2;
        var intervalSize = (long)Math.Ceiling(time / 2.0);
        var isLastButtonTimeWin = true;
        var endIteration = false;

        while (!endIteration)
        {
            var distanceTraveled = buttonTime * (time - buttonTime);
            isLastButtonTimeWin = distanceTraveled > distanceToBeat;

            var newIntervalSize = (long)Math.Ceiling(intervalSize / 2.0); // overapproximating to be absolutely sure to get the right value 
            buttonTime = isLastButtonTimeWin ? buttonTime - newIntervalSize : buttonTime + newIntervalSize;

            endIteration = newIntervalSize == intervalSize; // end iteration if we have tried intervalSize of 1 twice
            intervalSize = newIntervalSize;
        }

        //inelegant adjustment - last shift performed is 1, so if it was a winning time we have now overshoot it into the losing times
        if (isLastButtonTimeWin)
        {
            buttonTime++;
        }

        //here buttonTime is the minimum time to press the button to win. Due to simmetry we can say that
        return (time + 1) - 2 * buttonTime;
    }
}

internal static class StringExtensions
{
    private static readonly Regex NumberRegex = new(@"\d+");

    public static IEnumerable<int> GetNumbers(this string s) =>
        NumberRegex.Matches(s)
            .Select(m => int.Parse(m.Value));
    public static IEnumerable<long> GetNumbersLong(this string s) =>
        NumberRegex.Matches(s)
            .Select(m => long.Parse(m.Value));
}