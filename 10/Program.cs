// See https://aka.ms/new-console-template for more information

internal class Program
{
    private static char[] PipeCharArray = ['|', '-', 'L', 'J', '7', 'F', 'S'];

    public static void Main(string[] args)
    {
        var inputPath = $"input.txt";
        using var streamReader = new StreamReader(inputPath);
        var map = ParseInput(streamReader).ToList();
        foreach (var line in map)
        {
            Console.WriteLine(string.Join(null, line));
        }

        var currentPosition = FindS(map);
        var nextPositionChar = 'N'; //random unused character to denote not initialized variable
        var countSteps = 0;
        var lastMovement = Directions.U;
        var newPosition = (i: -1, j: -1);

        while (nextPositionChar != 'S')
        {
            var cameFrom = lastMovement.ChangeMovement(2);

            //find next pipe
            for (int i = 1; i <= 3; i++)
            {
                lastMovement = cameFrom.ChangeMovement(i);
                newPosition = lastMovement.PerformOn(currentPosition);
                if (IsPipe(map[newPosition.i][newPosition.j]))
                {
                    break;
                }

                if (i == 3)
                {
                    throw new Exception("Sanity check qui non arrivo");
                }
            }

            currentPosition = newPosition;
            nextPositionChar = map[currentPosition.i][currentPosition.j];

            countSteps++;
        }

        Console.WriteLine(countSteps);
    }

    private static bool IsPipe(char c) => PipeCharArray.Contains(c);


    private static (int i, int j) FindS(List<char[]> map)
    {
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == 'S')
                {
                    return (i, j);
                }
            }
        }

        throw new Exception("no S found");
    }

    private static IEnumerable<char[]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine()!.ToCharArray();
        }
    }
}

internal enum Directions
{
    U = 0,
    R = 1,
    D = 2,
    L = 3,
}

internal static class DirectionsExtensions
{
    // assume positive offset
    public static Directions ChangeMovement(this Directions source, int offset) =>
        (Directions)((int)(source + offset) % 4);

    public static (int i, int j) PerformOn(this Directions source, (int i, int j) position) =>
        source switch
        {
            Directions.U => (position.i - 1, position.j),
            Directions.R => (position.i, position.j + 1),
            Directions.D => (position.i + 1, position.j),
            Directions.L => (position.i, position.j - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
}