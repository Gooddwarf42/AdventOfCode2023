
using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex NumberRegex = new Regex(@"\d+");
    private static void Main(string[] args)
    {
        const int day = 4;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);

        var totalPoints = 0;

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine()!;
            var game = ParseLine(line);

            var intersection = game.WinningNumbers.Intersect(game.NumbersYouHave);
            var intersectionCount = intersection.Count();
            if (intersectionCount > 0)
            {
                totalPoints += (int)Math.Pow(2, intersection.Count() - 1);
            }
        }

        System.Console.WriteLine(totalPoints);
    }

    private static Game ParseLine(string line)
    {
        var colonIndex = line.IndexOf(':');
        var gameId = int.Parse(NumberRegex.Match(line[..colonIndex]).Value);
        var numberSets = line[(colonIndex + 1)..].Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (numberSets.Length != 2)
        {
            throw new Exception("Damn");
        }

        var winningNumbers = GetNumbers(numberSets[0]);
        var myNumbers = GetNumbers(numberSets[1]);

        return new Game{
            Id = gameId,
            WinningNumbers = winningNumbers,
            NumbersYouHave = myNumbers
        };

        static List<int> GetNumbers(string stringyNumberSet)
            => stringyNumberSet.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
    }

    internal sealed record Game
    {
        public int? Id { get; set; }
        public required List<int> WinningNumbers { get; set; }
        public required List<int> NumbersYouHave { get; set; }
    }
}