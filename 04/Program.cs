
using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex NumberRegex = new Regex(@"\d+");
    private static void Main(string[] args)
    {
        const int day = 4;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);

        var gamesList = ParseInput(streamReader);

        for (int i = 0; i < gamesList.Count; i++)
        {
            int wins = gamesList[i].CountWins();
            for (int j = 1; j <= wins; j++)
            {
                // this does not go out of bounds due to assumptions in the specification
                gamesList[i + j].InstanceCount += gamesList[i].InstanceCount;
            }
        }

        var totalInstances = gamesList.Select(g => g.InstanceCount).Sum();
        System.Console.WriteLine(totalInstances);
    }

    private static List<Game> ParseInput(StreamReader streamReader)
    {
        var gamesList = new List<Game>();
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine()!;
            var game = ParseLine(line);
            gamesList.Add(game);
        }
        return gamesList;
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

        return new Game
        {
            Id = gameId,
            WinningNumbers = winningNumbers,
            NumbersYouHave = myNumbers
        };

        static List<int> GetNumbers(string stringyNumberSet)
            => stringyNumberSet.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
    }
}

internal sealed record Game
{
    public int? Id { get; set; }
    public required List<int> WinningNumbers { get; set; }
    public required List<int> NumbersYouHave { get; set; }
    public int InstanceCount { get; set; } = 1;
}

internal static class GameExtensions
{
    public static int CountWins(this Game game)
        => game.WinningNumbers.Intersect(game.NumbersYouHave).Count();
}