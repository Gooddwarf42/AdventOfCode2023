using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        const string inputPath = "/home/marco/share/AdventOfCode2023/01/input";

        var inputLines = new List<string>();
        var digitRegex = new Regex(@"\d");

        using var streamReader = new StreamReader(inputPath);
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine()!;
            System.Console.WriteLine(line);
            inputLines.Add(line);
        }

        var total = 0;

        foreach (var line in inputLines)
        {
            var matches = digitRegex.Matches(line);
            if (matches.Count == 0)
            {
                throw new Exception("Occazzo");
            }

            var numberToAdd = int.Parse(matches.First().Value) * 10 + int.Parse(matches.Last().Value);
            total += numberToAdd;
        }

        System.Console.WriteLine(total);
    }
}