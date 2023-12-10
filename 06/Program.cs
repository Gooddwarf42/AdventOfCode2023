using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex NumberRegex = new(@"\d+");

    private static void Main(string[] args)
    {
        const int day = 6;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);
        var times = streamReader.ReadLine()!.GetNumbers().ToArray();
        var distances = streamReader.ReadLine()!.GetNumbers().ToArray();

        var result = 1L;
        for (int i = 0; i < times.Length; i++)
        {
            result *= GetNumberOfWaysToWin(times[i], distances[i]);
        }
        System.Console.WriteLine(result);
    }

    private static long GetNumberOfWaysToWin(int time, int distanceToBeat)
    {
        var buttonTime = time / 2;
        var intervalSize = time / 2;
        var isLastButtonTimeWin = true;

        while (intervalSize != 0)
        {
            var distanceTraveled = buttonTime * (time - buttonTime);
            isLastButtonTimeWin = distanceTraveled > distanceToBeat;

            intervalSize /= 2;
            buttonTime = isLastButtonTimeWin ? buttonTime - intervalSize : buttonTime + intervalSize;
        }

        //inelegant adjustment
        if (!isLastButtonTimeWin)
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
}