
internal class Program
{
    private static void Main(string[] args)
    {
        const int day = 2;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);

        var total = 0;
        const int maxRed = 12;
        const int maxGreen = 13;
        const int maxBlue = 14;

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine()!;
            var game = ParseGame(line);
            if (game.IsPossible((maxRed, maxGreen, maxBlue)))
            {
                total += game.Id;
            }
        }

        System.Console.WriteLine(total);
    }


    private static Game ParseGame(string line)
    {
        var splitHeaderReveals = line.Split(':', StringSplitOptions.TrimEntries);
        if (splitHeaderReveals.Length != 2)
        {
            throw new Exception("Occazzo");
        }

        return new Game
        {
            Id = ParseHeader(splitHeaderReveals[0]),
            Reveals = ParseReveals(splitHeaderReveals[1]).ToArray()
        };

    }

    private static int ParseHeader(string header) =>
        // Discard prefix "Game "
        int.Parse(header[5..]);

    private static IEnumerable<(int Red, int Green, int Blue)> ParseReveals(string reveals)
    {
        var revealsArray = reveals.Split(';', StringSplitOptions.TrimEntries);
        return revealsArray.Select(ParseReveal);
    }

    private static (int Red, int Green, int Blue) ParseReveal(string revealString)
    {
        (int Red, int Green, int Blue) reveal = (0, 0, 0);

        var singleColorReveals = revealString.Split(',', StringSplitOptions.TrimEntries);

        foreach (var colorReveal in singleColorReveals)
        {
            var yetAnotherSplit = colorReveal.Split(' ', StringSplitOptions.TrimEntries);
            if (yetAnotherSplit.Length != 2)
            {
                throw new Exception("Occazzo");
            }
            switch (yetAnotherSplit[1])
            {
                case "green":
                    reveal.Green = int.Parse(yetAnotherSplit[0]);
                    break;

                case "red":
                    reveal.Red = int.Parse(yetAnotherSplit[0]);
                    break;

                case "blue":
                    reveal.Blue = int.Parse(yetAnotherSplit[0]);
                    break;

                default:
                    throw new Exception("Le brutte cose!");
            }
        }

        return reveal;
    }
}

internal sealed class Game
{
    public int Id { get; set; }
    public (int Red, int Green, int Blue)[] Reveals { get; set; } = null!;
}
internal static class GameExtensions
{
    public static bool IsPossible(this Game game, (int maxRed, int maxGreen, int maxBlue) constraints)
        => game.Reveals.All(t => t.Red <= constraints.maxRed && t.Green <= constraints.maxGreen && t.Blue <= constraints.maxBlue);
}