using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Dictionary<string, int> DigitMapping = new()
    {
        {"zero", 0},
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9},
    };

    private static void Main(string[] args)
    {
        const string inputPath = "/home/marco/share/AdventOfCode2023/01/input";

        var validDigitPatterns = DigitMapping.Keys.ToList();
        validDigitPatterns.Add(@"\d");
        var digitRegex = new Regex(string.Join("|", validDigitPatterns));

        System.Console.WriteLine(digitRegex.ToString());

        var total = 0;

        using var streamReader = new StreamReader(inputPath);
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine()!;

            var matches = Regex.Matches(line, digitRegex.ToString());
            var matchesRightToLeft = Regex.Matches(line, digitRegex.ToString(), RegexOptions.RightToLeft);
            if (matches.Count == 0 || matchesRightToLeft.Count == 0)
            {
                throw new Exception("Occazzo");
            }

            var numberToAdd = ParseDigitFigoMaAncheNo(matches.First().Value) * 10 + ParseDigitFigoMaAncheNo(matchesRightToLeft.First().Value);
            total += numberToAdd;
        }

        System.Console.WriteLine(total);
    }

    private static int ParseDigitFigoMaAncheNo(string input)
        => DigitMapping.ContainsKey(input) ? DigitMapping[input] : int.Parse(input);

}