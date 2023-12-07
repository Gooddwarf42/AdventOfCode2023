
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
    }


    private static Game ParseGame(string line)
    {
        var splitHeaderReveals = line.Split(':');
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

    private static int ParseHeader(string v)
    {
        throw new NotImplementedException();
    }
    
    private static IEnumerable<(int Red, int Green, int Blue)> ParseReveals(string v)
    {
        throw new NotImplementedException();
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